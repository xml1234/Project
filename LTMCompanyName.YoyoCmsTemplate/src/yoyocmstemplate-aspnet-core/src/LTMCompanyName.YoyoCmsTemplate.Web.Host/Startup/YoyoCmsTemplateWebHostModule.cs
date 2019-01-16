using Abp.Configuration.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External;
using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Startup
{
    [DependsOn(
       typeof(YoyoCmsTemplateWebCoreModule))]
    public class YoyoCmsTemplateWebHostModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public YoyoCmsTemplateWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }


        public override void PreInitialize()
        {

#if DEBUG
            // 只有在DEBUG下才会把错误发送到客户端
            Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = true;
#endif
        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateWebHostModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            using (var scope = IocManager.CreateScope())
            {
                if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    return;
                }
            }

            if (IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
                workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
            }

            ConfigureExternalAuthProviders();
        }

        private void ConfigureExternalAuthProviders()
        {
            var externalAuthConfiguration = IocManager.Resolve<ExternalAuthConfiguration>();

            if (bool.Parse(_appConfiguration["Authentication:OpenId:IsEnabled"]))
            {
                externalAuthConfiguration.Providers.Add(
                    new ExternalLoginProviderInfo(
                        OpenIdConnectAuthProviderApi.Name,
                        _appConfiguration["Authentication:OpenId:ClientId"],
                        _appConfiguration["Authentication:OpenId:ClientSecret"],
                        typeof(OpenIdConnectAuthProviderApi),
                        new Dictionary<string, string>
                        {
                            {"Authority", _appConfiguration["Authentication:OpenId:Authority"]},
                            {"LoginUrl",_appConfiguration["Authentication:OpenId:LoginUrl"]}
                        }
                    )
                );
            }
        }
    }
}
