using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Abp.UI;
using Abp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using LTMCompanyName.YoyoCmsTemplate.Debugging;
using Abp.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Security;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Controllers
{
    public abstract class YoyoCmsTemplateControllerBase : AbpController
    {
        protected YoyoCmsTemplateControllerBase()
        {
            LocalizationSourceName = YoyoCmsTemplateConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        /// <summary>
        /// Session 移除并抛出异常
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="sessionCacheKey"></param>
        protected void ThrowExceptionAndRemoveSession(string msg, string sessionCacheKey)
        {
            // 移除session缓存并抛出异常
            HttpContext.Session.Remove(sessionCacheKey);
            throw new UserFriendlyException(msg);
        }

        /// <summary>
        /// Session 校验验证码,不正确将抛出异常
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        protected void ValuedationCode(CaptchaType type, string code)
        {
            var key = type.ToString();
            var value = HttpContext.Session.GetString(key);

            if (value.IsNullOrWhiteSpace())
            {
                throw new UserFriendlyException("验证码不能为空!");
            }

            if (value != code?.ToLower())
            {
                throw new UserFriendlyException("验证码错误!");
            }

            HttpContext.Session.Remove(key);
        }

    }
}
