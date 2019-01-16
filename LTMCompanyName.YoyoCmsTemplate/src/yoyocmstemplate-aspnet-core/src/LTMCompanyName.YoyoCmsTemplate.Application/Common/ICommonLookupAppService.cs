using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Common.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        /// <summary>
        /// 获取所有验证码支持的类型
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<ComboboxItemDtoT<int>>> GetValidateCodeTypesForCombobox();

        Task<PagedResultDto<NameValueDto>> FindUsers(CommonLookupFindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}