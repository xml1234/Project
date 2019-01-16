using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;

namespace LTMCompanyName.YoyoCmsTemplate.PhoneBooks.Persons.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="PersonAppPermissions" /> for all permission names. Person
    ///</summary>
    public class PersonAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了Person 的权限。
    var pages = context.GetPermissionOrNull(AppLtmPermissions.Pages) ?? context.CreatePermission(AppLtmPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppLtmPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppLtmPermissions.Pages_Administration, L("Administration"));

    var person = administration.CreateChildPermission(PersonAppPermissions.Person , L("Persons"));
person.CreateChildPermission(PersonAppPermissions.Person_Create, L("Create"));
person.CreateChildPermission(PersonAppPermissions.Person_Edit, L("Edit"));
person.CreateChildPermission(PersonAppPermissions.Person_Delete, L("Delete"));
person.CreateChildPermission(PersonAppPermissions.Person_BatchDelete , L("BatchDelete"));
person.CreateChildPermission(PersonAppPermissions.Person_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, YoyoCmsTemplateConsts.LocalizationSourceName);
    }
    }
    }