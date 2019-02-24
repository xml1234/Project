using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        private IAddCustomerUserCalims _addCustomerUserCalims;

        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor, IAddCustomerUserCalims addCustomerUserCalims)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor)
        {
            _addCustomerUserCalims = addCustomerUserCalims;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);
            var identity = principal.Identities.First();

            //添加信息到claim中
            await _addCustomerUserCalims.AddCustomerCalims(identity, user);

            return principal;
        }

    }
}
