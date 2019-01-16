
namespace LTMCompanyName.YoyoCmsTemplate.PhoneBooks.Persons.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="PersonAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class PersonAppPermissions
    {
    /// <summary>
        /// Person管理权限_自带查询授权
        ///</summary>
    public const string Person = "Pages.Person";

    /// <summary>
        /// Person创建权限
        ///</summary>
    public const string Person_Create = "Pages.Person.Create";

    /// <summary>
        /// Person修改权限
        ///</summary>
    public const string Person_Edit = "Pages.Person.Edit";

    /// <summary>
        /// Person删除权限
        ///</summary>
    public const string Person_Delete = "Pages.Person.Delete";

    /// <summary>
        /// Person批量删除权限
        ///</summary>
    public const string Person_BatchDelete  = "Pages.Person.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string Person_ExportToExcel = "Pages.Person.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

