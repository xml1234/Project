using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using Abp.Zero.Configuration;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings
{
    /// <summary>
    /// Defines settings for the application.
    /// See <see cref="AppSettingNames"/> for setting names.
    /// </summary>
    public class AppSettingProvider : SettingProvider
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AppSettingProvider(IAppConfigurationAccessor configurationAccessor)
        {
            //  YoyoCmsTemplateConsts.MultiTenancyEnabled
            _appConfiguration = configurationAccessor.Configuration;
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            //Disable TwoFactorLogin by default (can be enabled by UI)
            context.Manager.
                GetSettingDefinition(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled)
                .DefaultValue = false.ToString().ToLowerInvariant();

            if (YoyoCmsTemplateConsts.MultiTenancyEnabled)
            {
                return GetHostSettings().Union(GetTenantSettings()).Union(GetApplicationAndTenantSettings());
            }
            else
            {
                return GetTenantSettings().Union(GetApplicationAndTenantSettings());
            }
        }




        /// <summary>
        /// setting scopes Application
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettingDefinition> GetHostSettings()
        {
            return new[]
            {
                // �����⻧ע�� ����
                new SettingDefinition(AppSettingNames.Host.AllowSelfRegistration, GetFromAppSettings(AppSettingNames.Host.AllowSelfRegistration, "true"), isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.Host.IsNewRegisteredTenantActiveByDefault, GetFromAppSettings(AppSettingNames.Host.IsNewRegisteredTenantActiveByDefault, "false")),
                // �����⻧ע��Ĭ�ϰ汾����
                new SettingDefinition(AppSettingNames.Host.DefaultEdition, GetFromAppSettings(AppSettingNames.Host.DefaultEdition, "")),
                // �����⻧ע����֤�� ����
                new SettingDefinition(AppSettingNames.Host.UseCaptchaOnTenantRegistration, GetFromAppSettings(AppSettingNames.Host.UseCaptchaOnTenantRegistration, "false"), isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.Host.CaptchaOnTenantRegistrationType, GetFromAppSettings(AppSettingNames.Host.CaptchaOnTenantRegistrationType, "1"), isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.Host.CaptchaOnTenantRegistrationLength, GetFromAppSettings(AppSettingNames.Host.CaptchaOnTenantRegistrationLength, "4"), isVisibleToClients: true),
                // �������⻧���Ĺ�����������
                new SettingDefinition(AppSettingNames.Host.SubscriptionExpireNotifyDayCount, GetFromAppSettings(AppSettingNames.Host.SubscriptionExpireNotifyDayCount, "7"), isVisibleToClients: true),
                // ������Ʊ����
                new SettingDefinition(AppSettingNames.Host.BillingLegalName, GetFromAppSettings(AppSettingNames.Host.BillingLegalName, "")),
                new SettingDefinition(AppSettingNames.Host.BillingAddress, GetFromAppSettings(AppSettingNames.Host.BillingAddress, "")),
            };
        }

        /// <summary>
        /// setting scopes Tenant
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettingDefinition> GetTenantSettings()
        {
            var scopes = SettingScopes.Tenant;
            return new[]
            {
                // �⻧��Ʊ ����
                new SettingDefinition(AppSettingNames.Tenant.BillingLegalName, GetFromAppSettings(AppSettingNames.Tenant.BillingLegalName, ""), scopes: scopes),
                new SettingDefinition(AppSettingNames.Tenant.BillingAddress, GetFromAppSettings(AppSettingNames.Tenant.BillingAddress, ""), scopes: scopes),
                new SettingDefinition(AppSettingNames.Tenant.BillingTaxVatNo, GetFromAppSettings(AppSettingNames.Tenant.BillingTaxVatNo, ""), scopes: scopes)
            };
        }

        /// <summary>
        /// setting scopes Application/Tenant
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettingDefinition> GetApplicationAndTenantSettings()
        {
            var scopes = SettingScopes.Application | SettingScopes.Tenant;
            return new[]
            {
                // �û�ע�� ����
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.AllowSelfRegistrationUser,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.AllowSelfRegistrationUser, "true"),
                    scopes: scopes,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.IsNewRegisteredUserActiveByDefault,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.IsNewRegisteredUserActiveByDefault, "false"),
                    scopes: scopes
                ),
                // �û�ע����֤�� ����
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserRegistration,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserRegistration, "false"),
                    scopes: scopes,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationType,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationType, "1"),
                    scopes: scopes,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationLength,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationLength, "4"),
                    scopes: scopes,
                    isVisibleToClients: true
                ),
                // �û���½��֤�� ����
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserLogin,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserLogin, "false"),
                    scopes: scopes, isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginType,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginType, "1"),
                    scopes: scopes,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginLength,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginLength, "4"),
                    scopes:scopes,
                    isVisibleToClients: true
                ),
            };
        }

        /// <summary>
        /// setting scopes All
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettingDefinition> GetSharedSettings()
        {
            var scopes = SettingScopes.All;
            return new[]
            {
                // ��������
                new SettingDefinition(
                    AppSettingNames.Shared.SmsVerificationEnabled,
                    GetFromAppSettings(AppSettingNames.Shared.SmsVerificationEnabled, "false"),
                    scopes: scopes,
                    isVisibleToClients: false
                )
            };
        }



        private string GetFromAppSettings(string name, string defaultValue = null)
        {
            return GetFromSettings("App:" + name, defaultValue);
        }

        private string GetFromSettings(string name, string defaultValue = null)
        {
            return _appConfiguration[name] ?? defaultValue;
        }

    }
}
