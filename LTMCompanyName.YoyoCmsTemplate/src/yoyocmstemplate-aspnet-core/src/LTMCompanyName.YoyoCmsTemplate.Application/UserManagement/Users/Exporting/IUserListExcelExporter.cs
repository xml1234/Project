using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToExcel(List<UserListDto> usertListDtos);
    }
}