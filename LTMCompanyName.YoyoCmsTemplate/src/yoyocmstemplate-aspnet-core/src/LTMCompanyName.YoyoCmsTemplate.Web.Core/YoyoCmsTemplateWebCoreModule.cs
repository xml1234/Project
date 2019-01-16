using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Authentication.JwtBearer;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using System.IO;
using Abp.IO;
using Abp.MailKit;
using Abp.Configuration.Startup;
using LTMCompanyName.YoyoCmsTemplate.Notifications;
using YoYo.ABP.Common;
using Abp.Runtime.Caching.Redis;
using Abp.Hangfire;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External;
using Abp.Hangfire.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate
{
    [DependsOn(
        typeof(YoyoCmsTemplateApplicationModule),
        typeof(YoyoCmsTemplateEntityFrameworkModule),
        typeof(AbpAspNetCoreModule),
        typeof(AbpAspNetCoreSignalRModule),
        typeof(YoYoABPCommonModule),
        typeof(AbpRedisCacheModule), // 如果不使用Redis做缓存,可以移除 Abp.RedisCache
        typeof(AbpHangfireAspNetCoreModule) // 如果不使用Hangfire做后台任务,可以移除 Abp.Hangfire 和 Hangfire.Sqlxxxx
     )]
    public class YoyoCmsTemplateWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public YoyoCmsTemplateWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                YoyoCmsTemplateConsts.ConnectionStringName
            );



            // 使用数据库管理语言
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(YoyoCmsTemplateApplicationModule).GetAssembly()
                 );


            Configuration.Caching.Configure(TwoFactorCodeCacheItem.CacheName, cache =>
            {
                cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(2);
            });

            if (_appConfiguration["Authentication:JwtBearer:IsEnabled"] != null 
                && bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"]))
            {
                ConfigureTokenAuth();
            }

            Configuration.ReplaceService<IAppConfigurationAccessor, AppConfigurationAccessor>();



            //// 将默认后台任务替换成 Hangfire,还需要取消Startup中对Hangfire配置的注释
            //Configuration.BackgroundJobs.UseHangfire();

            //// 将默认缓存切换为Redis
            //// 配置信息位于 appsettings.json中
            //Configuration.Caching.UseRedis(options =>
            //{
            //    options.ConnectionString = _appConfiguration["Cache:Redis:ConnectionString"];
            //    options.DatabaseId = _appConfiguration.GetValue<int>("Cache:Redis:DatabaseId");
            //});
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            SetAppFolders();
        }

        /// <summary>
        /// 启动项目的时候设置存档的文件夹信息
        /// </summary>
        private void SetAppFolders()
        {
            var appFolders = IocManager.Resolve<AppFolder>();

            appFolders.SampleProfileImagesFolder = Path.Combine(_env.WebRootPath, $"Common{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}SampleProfilePics");
            appFolders.WebSiteLogsFolder = Path.Combine(_env.ContentRootPath, $"App_Data{Path.DirectorySeparatorChar}Logs");

            appFolders.WebContentRootPath = _env.WebRootPath;
            appFolders.TempFileDownloadFolder = Path.Combine(_env.WebRootPath, $"Temp{Path.DirectorySeparatorChar}Downloads");

            try
            {

                DirectoryHelper.CreateIfNotExists(appFolders.SampleProfileImagesFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder);

            }
            catch { }
        }
    }
}
