using YoYo.ABP.Common.VierificationCode;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos
{
    public class TenantManagementSettingsEditDto
    {
        /// <summary>
        /// ����ע��
        /// </summary>
        public bool AllowSelfRegistration { get; set; }

        /// <summary>
        /// ע���⻧Ĭ�ϼ���
        /// </summary>
        public bool IsNewRegisteredTenantActiveByDefault { get; set; }

        /// <summary>
        /// �����⻧ע��ʹ����֤��
        /// </summary>
        public bool UseCaptchaOnTenantRegistration { get; set; }

        /// <summary>
        /// �����⻧ע����֤������
        /// </summary>
        public ValidateCodeType CaptchaOnTenantRegistrationType { get; set; }

        /// <summary>
        /// �����⻧ע����֤�볤��
        /// </summary>
        public int CaptchaOnTenantRegistrationLength { get; set; }

        /// <summary>
        /// Ĭ�ϰ汾id
        /// </summary>
        public int? DefaultEditionId { get; set; }
    }
}