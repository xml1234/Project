using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate
{
    /// <summary>
    ///    项目应用程序服务的基类。
    /// </summary>
    public abstract class YoyoCmsTemplateAppServiceBase : ApplicationService
    {
        protected YoyoCmsTemplateAppServiceBase()
        {
            LocalizationSourceName = YoyoCmsTemplateConsts.LocalizationSourceName;
        }

        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        /// <summary>
        ///     返回当前用户信息
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null) throw new Exception("There is no current user!");

            return user;
        }

        /// <summary>
        ///     返回当前租户信息
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<Tenant> GetCurrentTenantAsync()
        {
            return await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}