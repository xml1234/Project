using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Dependency;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users
{
    public interface IAddCustomerUserCalims : ISingletonDependency
    {

        /// <summary>
        /// 添加自定义的Calims
        /// </summary>
        /// <param name="claimsIdentity">Calims的管理器</param>
        /// <param name="user">当前登录用户的信息</param>
        /// <returns></returns>
        Task AddCustomerCalims(ClaimsIdentity claimsIdentity, User user);

    }
}