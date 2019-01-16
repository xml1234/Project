using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Caching;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External
{
    public static class TwoFactorCodeCacheExtensions
    {
        public static ITypedCache<string, TwoFactorCodeCacheItem> GetTwoFactorCodeCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, TwoFactorCodeCacheItem>(TwoFactorCodeCacheItem.CacheName);
        }
    }
}