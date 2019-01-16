using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Extensions;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Debugging;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Security;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using System.Web;
using System;
using Abp.Authorization;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Impersonation;
using Abp.Runtime.Session;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts
{
    public class AccountAppService : YoyoCmsTemplateAppServiceBase, IAccountAppService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly ICacheManager _cacheManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager,
            IPasswordHasher<User> passwordHasher,
            ICacheManager cacheManager,
            IImpersonationManager impersonationManager,
            IUserLinkManager userLinkManager
            )
        {
            _userRegistrationManager = userRegistrationManager;
            _passwordHasher = passwordHasher;
            _cacheManager = cacheManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null) return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);

            if (!tenant.IsActive) return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public Task<int?> ResolveTenantId(ResolveTenantIdInput input)
        {
            if (string.IsNullOrEmpty(input.c))
            {
                return Task.FromResult(AbpSession.TenantId);
            }

            var parameters = SimpleStringCipher.Instance.Decrypt(input.c);
            var query = HttpUtility.ParseQueryString(parameters);

            if (query["tenantId"] == null)
            {
                return Task.FromResult<int?>(null);
            }

            var tenantId = Convert.ToInt32(query["tenantId"]) as int?;
            return Task.FromResult(tenantId);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            // 检查验证码
            await CaptchaHelper.CheckVierificationCode(
                this._cacheManager,
                this.SettingManager,
                CaptchaType.TenantUserRegister,
                input.UserName,
                input.VerificationCode,
                AbpSession.TenantId);


            var user = await _userRegistrationManager.RegisterAsync(
                input.UserName,
                input.UserName,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );

            var isEmailConfirmationRequiredForLogin =
                await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                    .IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                // 是否可以登陆 1、用户已激活 并且 2、用户邮箱已确认或未启用邮箱校验 并且 3、没有启用登陆验证码
                CanLogin = user.IsActive
                && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
                && !await UseCaptchaOnLogin()
            };
        }


        public async Task SendPasswordResetCode(SendPasswordResetCodeInput input)
        {
            var user = await UserManager.FindByEmailAsync(input.EmailAddress);
            if (user == null) throw new UserFriendlyException(L("InvalidEmailAddress"));

            user.SetNewPasswordResetCode();
            throw new UserFriendlyException("等待完善的短信验证码功能");

            // TODO: 发送短信验证码
        }

        /// <summary>
        ///  重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResetPasswordOutput> ResetPasswordAsync(ResetPasswordInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != input.ResetCode)
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));

            user.Password = _passwordHasher.HashPassword(user, input.Password);
            user.PasswordResetCode = null;
            user.IsEmailConfirmed = true;
            user.NeedToChangeThePassword = false;

            await UserManager.UpdateAsync(user);

            return new ResetPasswordOutput
            {
                CanLogin = user.IsActive && !await UseCaptchaOnLogin(),
                UserName = user.UserName
            };
        }


        /// <summary>
        /// 登陆启用验证码
        /// </summary>
        /// <returns></returns>
        private async Task<bool> UseCaptchaOnLogin()
        {
            var captchaConfig = await SettingManager.GetCaptchaConfig(AbpSession.TenantId.HasValue ? CaptchaType.TenantUserLogin : CaptchaType.HostUserLogin, AbpSession.TenantId);

            return captchaConfig.Enabled;
        }

        public async Task SendEmailActivationLink(SendEmailActivationLinkInput input)
        {
            throw new NotImplementedException("SendEmailActivationLink 暂未实现");
        }

        public async Task ActivateEmail(ActivateEmailInput input)
        {
            throw new NotImplementedException("ActivateEmail 暂未实现");
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Impersonation)]
        public async Task<ImpersonateOutput> Impersonate(ImpersonateInput input)
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(input.UserId, input.TenantId),
                TenancyName = await GetTenancyNameOrNull(input.TenantId)
            };
        }

        public async Task<ImpersonateOutput> BackToImpersonator()
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetBackToImpersonatorToken(),
                TenancyName = await GetTenancyNameOrNull(AbpSession.ImpersonatorTenantId)
            };
        }

        public async Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input)
        {
            if (!await _userLinkManager.AreUsersLinked(AbpSession.ToUserIdentifier(), input.ToUserIdentifier()))
            {
                throw new Exception(L("This account is not linked to your account"));
            }

            return new SwitchToLinkedAccountOutput
            {
                SwitchAccountToken = await _userLinkManager.GetAccountSwitchToken(input.TargetUserId, input.TargetTenantId),
                TenancyName = await GetTenancyNameOrNull(input.TargetTenantId)
            };
        }





        #region 私有函数

        private async Task<Tenant> GetActiveTenant(int tenantId)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        private async Task<string> GetTenancyNameOrNull(int? tenantId)
        {
            return tenantId.HasValue ? (await GetActiveTenant(tenantId.Value)).TenancyName : null;
        }

        private async Task<User> GetUserByChecking(string inputEmailAddress)
        {
            var user = await UserManager.FindByEmailAsync(inputEmailAddress);
            if (user == null)
            {
                throw new UserFriendlyException(L("InvalidEmailAddress"));
            }

            return user;
        }

        #endregion
    }
}