using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Security.Recaptcha
{
    public class YoYoRecaptchaValidator : IRecaptchaValidator
    {
        private readonly ICacheManager _cacheManager;

        public YoYoRecaptchaValidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }


        public Task Validate(string captchaResponse)
        {
            return null;
        }
    }
}
