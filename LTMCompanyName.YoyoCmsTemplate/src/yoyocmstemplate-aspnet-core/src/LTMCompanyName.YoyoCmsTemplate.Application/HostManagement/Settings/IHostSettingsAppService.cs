using Abp.Application.Services;
using LTMCompanyName.YoyoCmsTemplate.Configuration.Dtos;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings
{
    public interface IHostSettingsAppService : IApplicationService
    {
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns></returns>
        Task<HostSettingsEditDto> GetAllSettings();

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAllSettings(HostSettingsEditDto input);

        /// <summary>
        /// ���Ͳ����ʼ�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendTestEmail(SendTestEmailInput input);


    }
}
