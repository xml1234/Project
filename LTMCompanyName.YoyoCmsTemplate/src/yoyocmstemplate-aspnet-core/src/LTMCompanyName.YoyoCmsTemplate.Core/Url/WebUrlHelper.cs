using System.Text.RegularExpressions;

namespace LTMCompanyName.YoyoCmsTemplate.Url
{
    /// <summary>
    /// 自己扩展的urlhelper信息
    /// </summary>
    public class WebUrlHelper
    {
        private static readonly Regex UrlWithProtocolRegex = new Regex("^.{1,10}://.*$");

        /// <summary>
        /// 判断是否为根目录路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsRooted(string url)
        {
            return url.StartsWith("/") || WebUrlHelper.UrlWithProtocolRegex.IsMatch(url);
        }

    }



 
}