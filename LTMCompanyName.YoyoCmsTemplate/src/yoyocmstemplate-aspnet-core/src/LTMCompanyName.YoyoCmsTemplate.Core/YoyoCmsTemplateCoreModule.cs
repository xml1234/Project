using System;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Configuration.Startup;
using Abp.MailKit;
using Abp.Net.Mail.Smtp;
using Abp.Zero;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using Castle.MicroKernel.Registration;

using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Localization;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Timing;

using LTMCompanyName.YoyoCmsTemplate.Features;
using LTMCompanyName.YoyoCmsTemplate.Debugging;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Payments.Cache;

namespace LTMCompanyName.YoyoCmsTemplate
{
    [DependsOn(
        typeof(AbpZeroCoreModule),
        typeof(AbpZeroLdapModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpMailKitModule)
        )]
    public class YoyoCmsTemplateCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 多租户
            Configuration.MultiTenancy.IsEnabled = YoyoCmsTemplateConsts.MultiTenancyEnabled;
            // 使用审计日志
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;


            // 声明类型
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

          
            // 功能
            Configuration.Features.Providers.Add<AppFeatureProvider>();
            // 设置
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            // 本地化
            YoyoCmsTemplateLocalizationConfigurer.Configure(Configuration.Localization);


            // 启用LDAP身份验证(只有禁用多租户才能启用)
            //Configuration.Modules.ZeroLdap().Enable(typeof(AppLdapAuthenticationSource));

            // 配置角色
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);


            // 如果是Debug模式
            if (DebugHelper.IsDebug)
            {
                // 禁用邮件发送
                Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            }


            // 全局缓存配置默认过期时间
            Configuration.Caching.Configure(PaymentCacheItem.CacheName, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(YoyoCmsTemplateConsts.PaymentCacheDurationInMinutes);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
