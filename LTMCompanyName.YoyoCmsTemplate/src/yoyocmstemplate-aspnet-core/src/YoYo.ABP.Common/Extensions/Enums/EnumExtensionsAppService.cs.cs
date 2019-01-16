using System;
using System.Collections.Generic;

namespace YoYo.ABP.Common.Extensions.Enums
{
    /// <summary>
    /// 枚举的扩展服务方法
    /// </summary>
    public class EnumExtensionsAppService : IEnumExtensionsAppService
    {


        #region 封装的方法

        /// <summary>
        ///     通用的获取枚举类型的信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> GetEnumTypeList<T>()
        {
            var items = new List<KeyValuePair<string, string>>();

            typeof(T).Each(
                (name, value, description) => { items.Add(new KeyValuePair<string, string>(description, value)); });

            return items;
        }

       

        #endregion


        public List<KeyValuePair<string, string>> GetEntityDoubleStringKeyValueList<TModel>()
        {
            return GetEnumTypeList<TModel>();
        }

        public List<KeyValuePair<string, int>> GetEntityStringIntKeyValueList<TModel>()
        {
            var dataList= GetEnumTypeList<TModel>();

           var result=new List<KeyValuePair<string,int>>();

            foreach (var item in dataList)
            {
              
                result.Add(new KeyValuePair<string, int>(item.Key, Convert.ToInt32(item.Value)));

            }

            return result;



        }

        public List<KeyValuePair<int, int>> GetEntityDoubleIntKeyValueList<TModel>()
        {
            var dataList = GetEnumTypeList<TModel>();

            var result = new List<KeyValuePair<int, int>>();

            foreach (var item in dataList)
            {

                result.Add(new KeyValuePair<int, int>(Convert.ToInt32(item.Key), Convert.ToInt32(item.Value)));

            }

            return result;
        }
    }
}