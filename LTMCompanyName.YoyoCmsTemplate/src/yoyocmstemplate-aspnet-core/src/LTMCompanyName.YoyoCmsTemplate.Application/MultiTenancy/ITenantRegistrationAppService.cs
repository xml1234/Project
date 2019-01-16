using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy
{
    public interface ITenantRegistrationAppService
    {
        /// <summary>
        /// 注册租户信息
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        Task<RegisterTenantResultDto> RegisterTenantAsync(CreateTenantDto input);
    }
}
