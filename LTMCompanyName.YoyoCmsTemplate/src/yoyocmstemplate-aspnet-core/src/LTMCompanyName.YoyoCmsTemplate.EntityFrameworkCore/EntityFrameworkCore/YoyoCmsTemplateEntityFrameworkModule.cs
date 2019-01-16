using Abp.Dependency;
using Abp.EntityFrameworkCore.Configuration;
using Abp.IdentityServer4;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore.Seed;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore
{
    [DependsOn(
        typeof(YoyoCmsTemplateCoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule),
        typeof(AbpZeroCoreIdentityServerEntityFrameworkCoreModule)
        )]
    public class YoyoCmsTemplateEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
                Configuration.Modules.AbpEfCore().AddDbContext<YoyoCmsTemplateDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                        YoyoCmsTemplateDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    else
                        YoyoCmsTemplateDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var configurationAccessor = IocManager.Resolve<IAppConfigurationAccessor>();

            using (var scope = IocManager.CreateScope())
            {
                if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:Default"]))
                {
                    SeedHelper.SeedHostDb(IocManager);
                }
            }
        }
    }
}