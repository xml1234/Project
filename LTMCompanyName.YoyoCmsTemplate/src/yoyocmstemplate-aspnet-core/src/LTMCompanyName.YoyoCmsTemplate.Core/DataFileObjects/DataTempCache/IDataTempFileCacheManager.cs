using Abp.Dependency;

namespace LTMCompanyName.YoyoCmsTemplate.DataFileObjects
{
    public interface IDataTempFileCacheManager:ITransientDependency
    {
        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="content"></param>
        void SetFile(string token, byte[] content);

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        byte[] GetFile(string token);
    }
}