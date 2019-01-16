using Abp.Extensions;
using Abp.Runtime.Caching;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Net.MimeTypes;
using LTMCompanyName.YoyoCmsTemplate.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YoYo.ABP.Common.VierificationCode;

namespace LTMCompanyName.YoyoCmsTemplate.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VerificationController : YoyoCmsTemplateControllerBase
    {
        private readonly IVierificationCodeService _vierificationCodeService;
        private readonly ICacheManager _cacheManager;

        public VerificationController(
            IVierificationCodeService vierificationCodeService,
            ICacheManager cacheManager
            )
        {
            _vierificationCodeService = vierificationCodeService;
            _cacheManager = cacheManager;
        }




        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="name">验证码key</param>
        /// <param name="t">验证码类型</param>
        /// <param name="tid">租户Id(可空)</param>
        /// <returns>验证码图片</returns>
        [HttpGet]
        public async Task<FileContentResult> GenerateCaptcha(string name, CaptchaType? t, int? tid)
        {
            if (!t.HasValue
                || t.Value == CaptchaType.Defulat
                || name.IsNullOrWhiteSpace())
            {
                return null;
            }

            var captchaConfig = await SettingManager.GetCaptchaConfig(t.Value, tid);
            if (!captchaConfig.Enabled)
            {
                return null;
            }

            var imgStream = _vierificationCodeService.Create(out string code, (ValidateCodeType)captchaConfig.Type, captchaConfig.Length);

            // 分租户获取缓存
            var cacheKey = CaptchaHelper.CreateCacheKey(t.Value, tid);
            var cache = _cacheManager.GetCache(cacheKey);

            // 存值,过期时间3分钟
            await cache.SetAsync(name, code.ToLower(), null, TimeSpan.FromMinutes(3));

            //
            Response.Body.Dispose();
            return File(imgStream.ToArray(), MimeTypeNames.ImagePng);
        }


    }
}
