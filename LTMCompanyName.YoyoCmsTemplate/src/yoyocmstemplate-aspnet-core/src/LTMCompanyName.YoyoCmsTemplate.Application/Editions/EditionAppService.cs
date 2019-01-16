﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;

using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.Runtime.Session;
using LTMCompanyName.YoyoCmsTemplate.Editions.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Editions
{
    [AbpAuthorize(EditionAppPermissions.Query)]
    public class EditionAppService : YoyoCmsTemplateAppServiceBase, IEditionAppService
    {
        private readonly EditionManager _editionManager;
        private readonly IRepository<SubscribableEdition> _subscribableEditionRepository;

        public EditionAppService(
            EditionManager editionManager,
            IRepository<SubscribableEdition> subscribableEditionRepository)
        {
            _editionManager = editionManager;
            _subscribableEditionRepository = subscribableEditionRepository;
        }



        public async Task<ListResultDto<EditionListDto>> GetEditions()
        {
            var editions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
                .OrderBy(e => e.MonthlyPrice);
            return new ListResultDto<EditionListDto>(
                ObjectMapper.Map<List<EditionListDto>>(editions)
                );
        }

        [AbpAuthorize(EditionAppPermissions.Create, EditionAppPermissions.Edit)]
        public async Task<GetEditionEditOutput> GetEditionForEdit(NullableIdDto input)
        {
            var features = FeatureManager.GetAll()
                .Where(f => f.Scope.HasFlag(FeatureScopes.Edition));

            EditionEditDto editionEditDto;
            List<NameValue> featureValues;

            if (input.Id.HasValue) //Editing existing edition?
            {
                var edition = await _editionManager.FindByIdAsync(input.Id.Value);
                featureValues = (await _editionManager.GetFeatureValuesAsync(input.Id.Value)).ToList();
                editionEditDto = ObjectMapper.Map<EditionEditDto>(edition);
            }
            else
            {
                editionEditDto = new EditionEditDto();
                featureValues = features.Select(f => new NameValue(f.Name, f.DefaultValue)).ToList();
            }

            var featureDtos = ObjectMapper.Map<List<FlatFeatureDto>>(features).OrderBy(f => f.DisplayName).ToList();

            return new GetEditionEditOutput
            {
                Edition = editionEditDto,
                Features = featureDtos,
                FeatureValues = featureValues.Select(fv => new NameValueDto(fv)).ToList()
            };
        }

        [AbpAuthorize(EditionAppPermissions.Create, EditionAppPermissions.Edit)]
        public async Task CreateOrUpdateEdition(CreateOrUpdateEditionDto input)
        {
            if (!input.Edition.Id.HasValue)
            {
                await CreateEditionAsync(input);
            }
            else
            {
                await UpdateEditionAsync(input);
            }
        }

        [AbpAuthorize(EditionAppPermissions.Delete)]
        public async Task DeleteEdition(EntityDto input)
        {
            var edition = await _editionManager.GetByIdAsync(input.Id);
            await _editionManager.DeleteAsync(edition);
        }

        public async Task<List<SubscribableEditionComboboxItemDto>> GetEditionComboboxItems(int? selectedEditionId = null, bool addAllItem = false, bool onlyFreeItems = false)
        {
            var editions = await _editionManager.Editions.ToListAsync();
            var subscribableEditions = editions.Cast<SubscribableEdition>()
                .WhereIf(onlyFreeItems, e => e.IsFree)
                .OrderBy(e => e.MonthlyPrice);

            var editionItems =
                new ListResultDto<SubscribableEditionComboboxItemDto>(
                    subscribableEditions
                    .Select(e => new SubscribableEditionComboboxItemDto(e.Id, e.DisplayName, e.IsFree)).ToList())
                    .Items.ToList();


            if (selectedEditionId.HasValue)
            {
                var selectedEdition = editionItems.FirstOrDefault(e => e.Value == selectedEditionId.Value);
                if (selectedEdition != null)
                {
                    selectedEdition.IsSelected = true;
                }
            }
            else
            {
                editionItems[0].IsSelected = true;
            }

            return editionItems;
        }

        [AbpAuthorize(EditionAppPermissions.Create)]
        protected virtual async Task CreateEditionAsync(CreateOrUpdateEditionDto input)
        {
            var edition = ObjectMapper.Map<SubscribableEdition>(input.Edition);

            if (edition.ExpiringEditionId.HasValue)
            {
                var expiringEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(edition.ExpiringEditionId.Value);
                if (!expiringEdition.IsFree)
                {
                    throw new UserFriendlyException(L("ExpiringEditionMustBeAFreeEdition"));
                }
            }

            await _editionManager.CreateAsync(edition);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.

            await SetFeatureValues(edition, input.FeatureValues);
        }

        [AbpAuthorize(EditionAppPermissions.Edit)]
        protected virtual async Task UpdateEditionAsync(CreateOrUpdateEditionDto input)
        {
            if (input.Edition.Id != null)
            {
                var edition = await _editionManager.GetByIdAsync(input.Edition.Id.Value);
                var existingSubscribableEdition = (SubscribableEdition)edition;

                var updatingSubscribableEdition = ObjectMapper.Map<SubscribableEdition>(input.Edition);
                if (existingSubscribableEdition.IsFree &&
                    !updatingSubscribableEdition.IsFree &&
                    await _subscribableEditionRepository.CountAsync(e => e.ExpiringEditionId == existingSubscribableEdition.Id) > 0)
                {
                    throw new UserFriendlyException(L("ThisEditionIsUsedAsAnExpiringEdition"));
                }

                ObjectMapper.Map(input.Edition, edition);

                await SetFeatureValues(edition, input.FeatureValues);
            }
        }

        private Task SetFeatureValues(Edition edition, List<NameValueDto> featureValues)
        {
            return _editionManager.SetFeatureValuesAsync(edition.Id,
                featureValues.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray());
        }
    }
}
