namespace LTMCompanyName.YoyoCmsTemplate.Security
{
    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum CaptchaType
    {
        /// <summary>
        /// 默认,非任何类型
        /// </summary>
        Defulat = 0,
        /// <summary>
        /// 宿主租户注册
        /// </summary>
        HostTenantRegister = 1,
        /// <summary>
        /// 宿主用户登陆
        /// </summary>
        HostUserLogin = 2,
        /// <summary>
        /// 租户用户注册
        /// </summary>
        TenantUserRegister = 3,
        /// <summary>
        /// 租户用户登陆
        /// </summary>
        TenantUserLogin = 4
    }
}