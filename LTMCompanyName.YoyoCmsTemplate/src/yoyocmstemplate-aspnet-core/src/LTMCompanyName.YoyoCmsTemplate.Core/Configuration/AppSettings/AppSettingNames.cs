namespace LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings
{
    /// <summary>
    /// 定义用于在应用程序中设置名称的字符串常量。
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
            /// 发票抬头
            /// </summary>
            public const string BillingLegalName = "App.Host.BillingLegalName";
            /// <summary>
            /// 发票地址
            /// </summary>
            public const string BillingAddress = "App.Host.BillingAddress";


            /// <summary>
            /// 启用注册租户
            /// </summary>
            public const string AllowSelfRegistration = "App.Host.AllowSelfRegistration";

            /// <summary>
            /// 启用新租户默认激活
            /// </summary>
            public const string IsNewRegisteredTenantActiveByDefault = "App.Host.IsNewRegisteredTenantActiveByDefault";

            /// <summary>
            /// 新租户默认版本
            /// </summary>
            public const string DefaultEdition = "App.Host.DefaultEdition";

            /// <summary>
            /// 启用租户注册验证码
            /// </summary>
            public const string UseCaptchaOnTenantRegistration = "App.Host.UseCaptchaOnTenantRegistration";
            /// <summary>
            /// 租户注册验证码类型 0:纯数字 1:纯字母 2:数字+字母  3:纯汉字
            /// </summary>
            public const string CaptchaOnTenantRegistrationType = "App.Host.CaptchaOnTenantRegistrationType";
            /// <summary>
            /// 租户注册验证码验证码长度
            /// </summary>
            public const string CaptchaOnTenantRegistrationLength = "App.Host.CaptchaOnTenantRegistrationLength";


            /// <summary>
            /// 订阅过期通知日计数
            /// </summary>
            public const string SubscriptionExpireNotifyDayCount = "App.Host.SubscriptionExpireNotifyDayCount";
        }


        /// <summary>
        /// setting scopes Tenant
        /// </summary>
        public static class Tenant
        {
            /// <summary>
            /// 发票抬头
            /// </summary>
            public const string BillingLegalName = "App.Tenant.BillingLegalName";
            /// <summary>
            /// 发票地址
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
            /// 启用用户注册
            /// </summary>
            public const string AllowSelfRegistrationUser = "App.AllowSelfRegistrationUser";
            /// <summary>
            /// 启用新用户默认激活
            /// </summary>
            public const string IsNewRegisteredUserActiveByDefault = "App.IsNewRegisteredUserActiveByDefault";

            /// <summary>
            /// 启用用户注册验证码
            /// </summary>
            public const string UseCaptchaOnUserRegistration = "App.UseCaptchaOnUserRegistration";
            /// <summary>
            /// 用户注册验证码类型 0:纯数字 1:纯字母 2:数字+字母  3:纯汉字
            /// </summary>
            public const string CaptchaOnUserRegistrationType = "App.CaptchaOnUserRegistrationType";
            /// <summary>
            /// 用户注册验证码长度
            /// </summary>
            public const string CaptchaOnUserRegistrationLength = "App.CaptchaOnUserRegistrationLength";

            /// <summary>
            /// 启用用户登陆验证码
            /// </summary>
            public const string UseCaptchaOnUserLogin = "App.UseCaptchaOnUserLogin";
            /// <summary>
            /// 用户登陆验证码类型 0:纯数字 1:纯字母 2:数字+字母  3:纯汉字
            /// </summary>
            public const string CaptchaOnUserLoginType = "App.CaptchaOnUserLoginType";
            /// <summary>
            /// 用户登陆验证码长度
            /// </summary>
            public const string CaptchaOnUserLoginLength = "App.CaptchaOnUserLoginLength";

        }

        /// <summary>
        /// setting scopes all
        /// </summary>
        public static class Shared
        {

          
            /// <summary>
            /// 启用短信校验
            /// </summary>
            public const string SmsVerificationEnabled = "App.UserManagement.SmsVerificationEnabled";
        }
    }

    /// <summary>
    /// 宿主缓存Key
    /// </summary>
    public static class HostCacheKeys
    {
        /// <summary>
        /// 宿主租户注册验证码缓存Key
        /// </summary>
        public const string TenantRegistrationCaptchaCache = "TenantRegistrationCaptchaCache";
        /// <summary>
        /// 宿主用户注册验证码缓存Key
        /// </summary>
        public const string HostUserRegistrationCaptchaCache = "HostUserRegistrationCaptchaCache";
        /// <summary>
        /// 宿主用户登陆验证码缓存Key
        /// </summary>
        public const string HostUserLoginCaptchaCache = "HostUserLoginCaptchaCache";
    }

    /// <summary>
    /// 租户缓存Key
    /// </summary>
    public static class TenantCacheKeys
    {
        /// <summary>
        /// 租户用户注册缓存验证码Key
        /// </summary>
        public const string UserRegistrationCaptchaCache = "UserRegistrationCaptchaCache";
        /// <summary>
        /// 租户用户登陆缓存验证码Key
        /// </summary>
        public const string UserLoginCaptchaCache = "UserLoginCaptchaCache";
    }
}