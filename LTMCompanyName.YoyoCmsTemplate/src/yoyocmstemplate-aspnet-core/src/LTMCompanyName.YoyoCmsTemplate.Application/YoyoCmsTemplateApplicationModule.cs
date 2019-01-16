using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos.CustomMapper;
using LTMCompanyName.YoyoCmsTemplate.Editions.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Editions.Mapper;
using LTMCompanyName.YoyoCmsTemplate.Languages.Dtos.CustomMapper;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Mapper;
using LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos.CustomMapper;

namespace LTMCompanyName.YoyoCmsTemplate
{
    [DependsOn(
        typeof(YoyoCmsTemplateCoreModule),
        typeof(AbpAutoMapperModule))]
    public class YoyoCmsTemplateApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 权限认证提供者
            Configuration.Authorization.Providers.Add<YoyoCmsTemplateAuthorizationProvider>();
            Configuration.Authorization.Providers.Add<EditionAuthorizationProvider>();

            // 类型映射
            Configuration.Modules.AbpAutoMapper().Configurators.Add(EditionMapper.CreateMappings);

            // 自定义类型映射
            Configuration.Modules.AbpAutoMapper().Configurators.Add(configuration =>
            {
                // Add your custom AutoMapper mappings here...
                CustomerLanguageMapper.CreateMappings(configuration);

                // Permission
                CustomerPermissionsMapper.CreateMappings(configuration);

                // OrganizationUnit
                CustomerOranizationMapper.CreateMappings(configuration);

                //
                TenantMapper.CreateMappings(configuration);

            });
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(YoyoCmsTemplateApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }

        public override void PostInitialize()
        {
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //base.PostInitialize();
        }
    }
}