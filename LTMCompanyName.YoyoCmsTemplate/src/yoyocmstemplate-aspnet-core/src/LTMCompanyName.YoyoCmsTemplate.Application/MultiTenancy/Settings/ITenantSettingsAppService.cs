using Abp.Application.Services;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings.Dtos;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);


        Task ClearLogo();

        Task ClearCustomCss();
    }
}
