namespace LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings
{
    /// <summary>
    /// ����������Ӧ�ó������������Ƶ��ַ���������
    /// See <see cref="AppSettingProvider"/> for setting definitions.
    /// </summary>
    public static class AppSettingNames
    {
        /// <summary>
        /// setting scopes Application
        /// </summary>
        public static class Application
        {

        }

        /// <summary>
        /// setting scopes Application
        /// </summary>
        public static class Host
        {
            /// <summary>
            /// ��Ʊ̧ͷ
            /// </summary>
            public const string BillingLegalName = "App.Host.BillingLegalName";
            /// <summary>
            /// ��Ʊ��ַ
            /// </summary>
            public const string BillingAddress = "App.Host.BillingAddress";


            /// <summary>
            /// ����ע���⻧
            /// </summary>
            public const string AllowSelfRegistration = "App.Host.AllowSelfRegistration";

            /// <summary>
            /// �������⻧Ĭ�ϼ���
            /// </summary>
            public const string IsNewRegisteredTenantActiveByDefault = "App.Host.IsNewRegisteredTenantActiveByDefault";

            /// <summary>
            /// ���⻧Ĭ�ϰ汾
            /// </summary>
            public const string DefaultEdition = "App.Host.DefaultEdition";

            /// <summary>
            /// �����⻧ע����֤��
            /// </summary>
            public const string UseCaptchaOnTenantRegistration = "App.Host.UseCaptchaOnTenantRegistration";
            /// <summary>
            /// �⻧ע����֤������ 0:������ 1:����ĸ 2:����+��ĸ  3:������
            /// </summary>
            public const string CaptchaOnTenantRegistrationType = "App.Host.CaptchaOnTenantRegistrationType";
            /// <summary>
            /// �⻧ע����֤����֤�볤��
            /// </summary>
            public const string CaptchaOnTenantRegistrationLength = "App.Host.CaptchaOnTenantRegistrationLength";


            /// <summary>
            /// ���Ĺ���֪ͨ�ռ���
            /// </summary>
            public const string SubscriptionExpireNotifyDayCount = "App.Host.SubscriptionExpireNotifyDayCount";
        }


        /// <summary>
        /// setting scopes Tenant
        /// </summary>
        public static class Tenant
        {
            /// <summary>
            /// ��Ʊ̧ͷ
            /// </summary>
            public const string BillingLegalName = "App.Tenant.BillingLegalName";
            /// <summary>
            /// ��Ʊ��ַ
            /// </summary>
            public const string BillingAddress = "App.Tenant.BillingAddress";
            /// <summary>
            /// 
            /// </summary>
            public const string BillingTaxVatNo = "App.Tenant.BillingTaxVatNo";



        }

        /// <summary>
        ///  setting scopes User
        /// </summary>
        public static class User
        {


        }

        //public static class Host
        //{

        //}

        /// <summary>
        /// setting scopes Application/Tenant
        /// </summary>
        public static class ApplicationAndTenant
        {
            /// <summary>
            /// �����û�ע��
            /// </summary>
            public const string AllowSelfRegistrationUser = "App.AllowSelfRegistrationUser";
            /// <summary>
            /// �������û�Ĭ�ϼ���
            /// </summary>
            public const string IsNewRegisteredUserActiveByDefault = "App.IsNewRegisteredUserActiveByDefault";

            /// <summary>
            /// �����û�ע����֤��
            /// </summary>
            public const string UseCaptchaOnUserRegistration = "App.UseCaptchaOnUserRegistration";
            /// <summary>
            /// �û�ע����֤������ 0:������ 1:����ĸ 2:����+��ĸ  3:������
            /// </summary>
            public const string CaptchaOnUserRegistrationType = "App.CaptchaOnUserRegistrationType";
            /// <summary>
            /// �û�ע����֤�볤��
            /// </summary>
            public const string CaptchaOnUserRegistrationLength = "App.CaptchaOnUserRegistrationLength";

            /// <summary>
            /// �����û���½��֤��
            /// </summary>
            public const string UseCaptchaOnUserLogin = "App.UseCaptchaOnUserLogin";
            /// <summary>
            /// �û���½��֤������ 0:������ 1:����ĸ 2:����+��ĸ  3:������
            /// </summary>
            public const string CaptchaOnUserLoginType = "App.CaptchaOnUserLoginType";
            /// <summary>
            /// �û���½��֤�볤��
            /// </summary>
            public const string CaptchaOnUserLoginLength = "App.CaptchaOnUserLoginLength";

        }

        /// <summary>
        /// setting scopes all
        /// </summary>
        public static class Shared
        {

          
            /// <summary>
            /// ���ö���У��
            /// </summary>
            public const string SmsVerificationEnabled = "App.UserManagement.SmsVerificationEnabled";
        }
    }

    /// <summary>
    /// ��������Key
    /// </summary>
    public static class HostCacheKeys
    {
        /// <summary>
        /// �����⻧ע����֤�뻺��Key
        /// </summary>
        public const string TenantRegistrationCaptchaCache = "TenantRegistrationCaptchaCache";
        /// <summary>
        /// �����û�ע����֤�뻺��Key
        /// </summary>
        public const string HostUserRegistrationCaptchaCache = "HostUserRegistrationCaptchaCache";
        /// <summary>
        /// �����û���½��֤�뻺��Key
        /// </summary>
        public const string HostUserLoginCaptchaCache = "HostUserLoginCaptchaCache";
    }

    /// <summary>
    /// �⻧����Key
    /// </summary>
    public static class TenantCacheKeys
    {
        /// <summary>
        /// �⻧�û�ע�Ỻ����֤��Key
        /// </summary>
        public const string UserRegistrationCaptchaCache = "UserRegistrationCaptchaCache";
        /// <summary>
        /// �⻧�û���½������֤��Key
        /// </summary>
        public const string UserLoginCaptchaCache = "UserLoginCaptchaCache";
    }
}