using System.Linq;
using Abp.Dependency;
using Abp.Runtime.Session;

namespace LTMCompanyName.YoyoCmsTemplate.Extension
{

    /// <summary>
    /// 用于扩展ABPSession的方法
    /// </summary>
    public static class CustomerABPSessionExtension
    {

        /// <summary>
        /// abpSession的扩展方法获取用户名
        /// </summary>
        /// <param name="abpSession"></param>
        /// <returns></returns>
        public static string GetUserName(this IAbpSession abpSession)
        {
            return GetClaimValue(AppClaimExtensionTypes.UserName);
        }

        /// <summary>
        /// abpSession的扩展方法获取邮箱
        /// </summary>
        /// <param name="abpSession"></param>
        /// <returns></returns>
        public static string GerUserEmail(this IAbpSession abpSession)
        {
            return GetClaimValue(AppClaimExtensionTypes.Email);
        }

        /// <summary>
        /// 从身份信息中获取指定的类型值
        /// </summary>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public static string GetClaimValue(string claimType)
        {
            var pricipalAccessor = IocManager.Instance.Resolve<IPrincipalAccessor>();//使用ioc容器进行获取当前用户身份信息

            var claimPrincipal = pricipalAccessor.Principal;

            var claim = claimPrincipal?.Claims.FirstOrDefault(a => a.Type == claimType);

            return string.IsNullOrEmpty(claim?.Value) ? null : claim.Value;
        }

    }
}