using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    public class YoyoCmsTemplateAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public YoyoCmsTemplateAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public YoyoCmsTemplateAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }



        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //Host permissions
            //只有当宿主登陆后才能管理的权限
            //var tenants = pages.CreateChildPermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);


            // 顶级公共权限
            var pages = context.CreatePermission(PermissionNames.Pages, L("Pages"));

            // 组织机构
            var organizationUnits = pages.CreateChildPermission(PermissionNames.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(PermissionNames.Pages_Administration_OrganizationUnits_ManageUsers, L("ManagingMembers"));


            // 角色
            var roles = pages.CreateChildPermission(PermissionNames.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Delete, L("DeletingRole"));


            // 用户
            var users = pages.CreateChildPermission(PermissionNames.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_DeleteProfilePicture, L("DeleteProfilePicture"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_ExportToExcel, L("ExportToExcel"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Unlock, L("UserLockOut"));


            // 语言
            var languages = pages.CreateChildPermission(PermissionNames.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(PermissionNames.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(PermissionNames.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(PermissionNames.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(PermissionNames.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));


            // 审计日志
            var auditLogs = pages.CreateChildPermission(PermissionNames.Pages_AdminiStration_AuditLogs, L("AuditLogs"));

            // Host独有
            var tenants = pages.CreateChildPermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_BatchDelete, L("BatchDelete"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);
            pages.CreateChildPermission(PermissionNames.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            pages.CreateChildPermission(PermissionNames.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);

            pages.CreateChildPermission(PermissionNames.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);

            pages.CreateChildPermission(PermissionNames.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);


            // Tenant 独有
            pages.CreateChildPermission(PermissionNames.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, YoyoCmsTemplateConsts.LocalizationSourceName);
        }
    }
}
