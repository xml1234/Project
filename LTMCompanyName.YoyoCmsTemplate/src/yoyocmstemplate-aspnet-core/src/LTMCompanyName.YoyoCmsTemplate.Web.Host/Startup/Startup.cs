using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using Abp.AspNetCore.SignalR.Hubs;
using YoYo.ABP.Common.Configurations;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using Abp.Hangfire;

using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.IdentityServer;
using LTMCompanyName.YoyoCmsTemplate.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc(
                options => options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName))
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1); ;


            services.AddSignalR(options => { options.EnableDetailedErrors = true; });


            // ����ǰ��˷������
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );


            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);


            // IdentityServer4 ����
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                IdentityServerRegistrar.Register(services, _appConfiguration);
            }


            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "YoyoCmsTemplate API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // ��ʾ��Ҫ��Ȩ����Ȩ��ť�����߼��ھ�̬�ļ� wwwroot/swagger/ui/index.html ��
                options.AddSecurityDefinition("Bearer", new BasicAuthScheme());
            });

            //// ����hangfire
            //services.AddHangfire(config =>
            //{
            //    config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Default"));
            //});

            // ����abp������ע��
            return services.AddAbp<YoyoCmsTemplateWebHostModule>(
                options =>
                {
                    // ����log4net
                    options.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseAbpLog4Net().WithConfig("log4net.config"));
                }
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // ��ʼ��ABP���
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false;
            });

            // ����CORS
            app.UseCors(_defaultCorsPolicyName);

            // ���þ�̬�ļ�
            app.UseStaticFiles();

            // ����У��
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            // ���ʹ��Identity Server4
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                app.UseJwtTokenMiddleware("IdentityBearer");
                app.UseIdentityServer();
            }

            app.UseAbpRequestLocalization();


            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
            });

            ////Hangfire dashboard &server(Enable to use Hangfire instead of default job manager)
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new AbpHangfireAuthorizationFilter(PermissionNames.Pages_Administration_HangfireDashboard) }
            //});
            //app.UseHangfireServer();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // ʹ�м���ܹ���ΪJSON�˵��ṩ���ɵ�Swagger
            app.UseSwagger();
            // ʹ�м���ܹ��ṩswagger-ui(HTML��JS��CSS��)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "YoyoCmsTemplate API V1");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("LTMCompanyName.YoyoCmsTemplate.Web.Host.wwwroot.swagger.ui.index.html");
            }); // URL: /swagger
        }
    }
}
