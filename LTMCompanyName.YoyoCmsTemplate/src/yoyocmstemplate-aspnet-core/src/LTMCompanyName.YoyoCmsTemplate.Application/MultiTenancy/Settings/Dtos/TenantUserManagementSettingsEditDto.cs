using YoYo.ABP.Common.VierificationCode;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings.Dtos
{
    public class TenantUserManagementSettingsEditDto
    {
        /// <summary>
        /// �Ƿ�����ע��
        /// </summary>
        public bool AllowSelfRegistration { get; set; }

        /// <summary>
        /// �Ƿ���ע���û�Ĭ�ϼ���
        /// </summary>
        public bool IsNewRegisteredUserActiveByDefault { get; set; }

        /// <summary>
        /// �Ƿ����У��������ܵ�½
        /// </summary>
        public bool IsEmailConfirmationRequiredForLogin { get; set; }

        /// <summary>
        /// �Ƿ�ע��ʹ����֤��
        /// </summary>
        public bool UseCaptchaOnUserRegistration { get; set; }

        /// <summary>
        /// ע����֤������
        /// </summary>
        public ValidateCodeType CaptchaOnUserRegistrationType { get; set; }

        /// <summary>
        /// ע����֤�볤��
        /// </summary>
        public int CaptchaOnUserRegistrationLength { get; set; }

        /// <summary>
        /// �Ƿ��½ʹ����֤��
        /// </summary>
        public bool UseCaptchaOnUserLogin { get; set; }


        /// <summary>
        /// ��½��֤������
        /// </summary>
        public ValidateCodeType CaptchaOnUserLoginType { get; set; }

        /// <summary>
        /// ��½��֤�볤��
        /// </summary>
        public int CaptchaOnUserLoginLength { get; set; }
    }
}