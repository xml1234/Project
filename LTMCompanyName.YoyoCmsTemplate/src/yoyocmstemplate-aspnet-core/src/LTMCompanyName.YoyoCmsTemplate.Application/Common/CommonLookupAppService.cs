using System.Collections.Generic;
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
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.Common.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using YoYo.ABP.Common.VierificationCode;

namespace LTMCompanyName.YoyoCmsTemplate.Common
{

    [AbpAuthorize]
    public class CommonLookupAppService : YoyoCmsTemplateAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;

        public CommonLookupAppService(EditionManager editionManager)
        {
            _editionManager = editionManager;
        }

        public async Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false)
        {
            var subscribableEditions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
              .WhereIf(onlyFreeItems, e => e.IsFree)
              .OrderBy(e => e.MonthlyPrice);

            return new ListResultDto<SubscribableEditionComboboxItemDto>(
                subscribableEditions.Select(e => new SubscribableEditionComboboxItemDto(e.Id, e.DisplayName, e.IsFree)).ToList()
            );
        }

        public async Task<PagedResultDto<NameValueDto>> FindUsers(CommonLookupFindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.FilterText.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.FilterText) ||
                            u.Surname.Contains(input.FilterText) ||
                            u.UserName.Contains(input.FilterText) ||
                            u.EmailAddress.Contains(input.FilterText)
                    );

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );
            }
        }

        public GetDefaultEditionNameOutput GetDefaultEditionName()
        {
            return new GetDefaultEditionNameOutput
            {
                Name = EditionManager.DefaultEditionName
            };
        }

        public async Task<ListResultDto<ComboboxItemDtoT<int>>> GetValidateCodeTypesForCombobox()
        {
            await Task.Yield();

            var resList = new List<ComboboxItemDtoT<int>>();
            resList.Add(new ComboboxItemDtoT<int>((int)ValidateCodeType.Number, "数字"));
            resList.Add(new ComboboxItemDtoT<int>((int)ValidateCodeType.English, "英文"));
            resList.Add(new ComboboxItemDtoT<int>((int)ValidateCodeType.NumberAndLetter, "数字 + 英文"));
            resList.Add(new ComboboxItemDtoT<int>((int)ValidateCodeType.Hanzi, "汉字"));

            return new ListResultDto<ComboboxItemDtoT<int>>(resList);
        }
    }
}
