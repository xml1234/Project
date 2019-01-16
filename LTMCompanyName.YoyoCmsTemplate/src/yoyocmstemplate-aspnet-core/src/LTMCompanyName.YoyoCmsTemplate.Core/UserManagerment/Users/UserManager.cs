using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserManager(
            RoleManager roleManager,
            UserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ISettingManager settingManager)
            : base(
                roleManager,
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger,
                permissionManager,
                unitOfWorkManager,
                cacheManager,
                organizationUnitRepository,
                userOrganizationUnitRepository,
                organizationUnitSettings,
                settingManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(long? expectedUserId,
            string userName, string emailAddress)
        {
            var user = await Users
                .Where(o => o.UserName == userName || o.EmailAddress == emailAddress)
                .FirstOrDefaultAsync();

            if (user != null && user.UserName == userName && user.Id != expectedUserId)
            {
                throw new UserFriendlyException(L("UserAlreadyExists"), string.Format(L("UserAlreadyExists_Msg"), userName));
            }

            if (user != null && user.EmailAddress == emailAddress && user.Id != expectedUserId)
            {
                throw new UserFriendlyException(L("EmailAlreadyExists"), string.Format(L("EmailAlreadyExists_Msg"), user.EmailAddress));
            }
            return IdentityResult.Success;
        }

        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);
            if (user == null)
            {
                throw new Exception("There is no user: " + userIdentifier);
            }

            return user;
        }

        private new string L(string name)
        {
            return LocalizationManager.GetString(YoyoCmsTemplateConsts.LocalizationSourceName, name);
        }

        public override async Task<IdentityResult> CreateAsync(User user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            // TODO:不清空邮箱
            //user.EmailAddress = string.Empty;

            var tenantId = GetCurrentTenantId();
            if (tenantId.HasValue && !user.TenantId.HasValue)
            {
                user.TenantId = tenantId.Value;
            }

            try
            {
                return await base.CreateAsync(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [UnitOfWork]
        public virtual async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                return await FindByIdAsync(userIdentifier.UserId.ToString());
            }
        }

        private int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }

        public User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }
    }
}