using System;
using Abp.Runtime.Caching;

namespace LTMCompanyName.YoyoCmsTemplate.DataFileObjects
{
    public class DataTempFileCacheManager: IDataTempFileCacheManager
    {

        public const string TempFileCacheName = "TempFileCacheName";

        private readonly ICacheManager _cacheManager;

        public DataTempFileCacheManager(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void SetFile(string token, byte[] content)
        {
            _cacheManager.GetCache(TempFileCacheName).Set(token, content, new TimeSpan(0, 0, 1));//默认过期时间为1分钟
        }

        public byte[] GetFile(string token)
        {
            var file = _cacheManager.GetCache(TempFileCacheName).Get(token, ep => ep) as byte[];
            return file;
        }
    }
}