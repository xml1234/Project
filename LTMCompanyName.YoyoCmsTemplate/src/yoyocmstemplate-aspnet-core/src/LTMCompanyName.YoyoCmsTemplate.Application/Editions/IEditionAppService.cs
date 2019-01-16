using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Editions
{
    public interface IEditionAppService : IApplicationService
    {

        Task<ListResultDto<EditionListDto>> GetEditions();

        Task<GetEditionEditOutput> GetEditionForEdit(NullableIdDto input);

        Task CreateOrUpdateEdition(CreateOrUpdateEditionDto input);

        Task DeleteEdition(EntityDto input);

        Task<List<SubscribableEditionComboboxItemDto>> GetEditionComboboxItems(int? selectedEditionId = null, bool addAllItem = false, bool onlyFree = false);
    }
}
