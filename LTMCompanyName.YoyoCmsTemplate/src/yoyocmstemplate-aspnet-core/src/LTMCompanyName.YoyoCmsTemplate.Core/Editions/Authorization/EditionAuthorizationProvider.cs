using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Editions.Authorization
{
    public class EditionAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public EditionAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public EditionAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }



        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //在这里配置了 Edition 的权限。
            var pages = context.GetPermissionOrNull(AppLtmPermissions.Pages) ?? context.CreatePermission(AppLtmPermissions.Pages, L("Pages"));


            var node = pages.CreateChildPermission(EditionAppPermissions.Node, L("Edition"), multiTenancySides: MultiTenancySides.Host);
            node.CreateChildPermission(EditionAppPermissions.Query, L("Query"), multiTenancySides: MultiTenancySides.Host);
            node.CreateChildPermission(EditionAppPermissions.Create, L("Create"), multiTenancySides: MultiTenancySides.Host);
            node.CreateChildPermission(EditionAppPermissions.Edit, L("Edit"), multiTenancySides: MultiTenancySides.Host);
            node.CreateChildPermission(EditionAppPermissions.Delete, L("Delete"), multiTenancySides: MultiTenancySides.Host);

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, YoyoCmsTemplateConsts.LocalizationSourceName);
        }
    }
}
