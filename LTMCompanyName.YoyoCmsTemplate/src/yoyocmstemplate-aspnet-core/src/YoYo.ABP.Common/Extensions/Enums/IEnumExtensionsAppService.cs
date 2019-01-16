using System.Collections.Generic;
using Abp.Dependency;

namespace YoYo.ABP.Common.Extensions.Enums
{

    /// <summary>
    /// 枚举的扩展服务方法
    /// </summary>
    public interface IEnumExtensionsAppService : ISingletonDependency
    {
        /// <summary>
        /// 获取string,string
        /// </summary>
        /// <returns></returns>
        List<KeyValuePair<string, string>> GetEntityDoubleStringKeyValueList<TModel>();
        /// <summary>
        /// 获取string,int
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        List<KeyValuePair<string, int>> GetEntityStringIntKeyValueList<TModel>();
        /// <summary>
        /// 获取 int,int
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        List<KeyValuePair<int, int>> GetEntityDoubleIntKeyValueList<TModel>();





    }
}