using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External
{
    [Serializable]
    public class TwoFactorCodeCacheItem
    {
        public const string CacheName = "YoYoAppTwoFactorCodeCache";

        public string Code { get; set; }

        public TwoFactorCodeCacheItem()
        {

        }

        public TwoFactorCodeCacheItem(string code)
        {
            Code = code;
        }
    }
}
