using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using YoYo.ABP.Common.Exceptions;

namespace YoYo.ABP.Common.Filter
{
    /// <summary>
    ///     <see cref="IQueryable{T}" />类型字符串排序操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class QueryablePropertySorter<T>
    {
        private static readonly ConcurrentDictionary<string, LambdaExpression> Cache =
            new ConcurrentDictionary<string, LambdaExpression>();

  
        private static LambdaExpression GetKeySelector(string keyName)
        {
            var type = typeof (T);
            var key = type.FullName + "." + keyName;
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }
            var param = Expression.Parameter(type);
            var propertyNames = keyName.Split('.');
            Expression propertyAccess = param;
            foreach (var propertyName in propertyNames)
            {
                var property = type.GetProperty(propertyName);
                if (property == null)
                {
                    throw new YoYoABPException(string.Format(Resources.ObjectExtensions_PropertyNameNotExistsInType,
                        propertyName));
                }
                type = property.PropertyType;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
            }
            var keySelector = Expression.Lambda(propertyAccess, param);
            Cache[key] = keySelector;
            return keySelector;
        }
    }
}