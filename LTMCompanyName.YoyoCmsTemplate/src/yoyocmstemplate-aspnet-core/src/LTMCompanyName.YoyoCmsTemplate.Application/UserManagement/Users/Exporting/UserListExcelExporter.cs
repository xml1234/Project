using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LTMCompanyName.YoyoCmsTemplate.DataExporting.Excel.Epplus;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Exporting
{
    public class UserListExcelExporter:EpplusExcelExporterBase, IUserListExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;

        public UserListExcelExporter(IDataTempFileCacheManager dataTempFileCacheManager, ITimeZoneConverter timeZoneConverter) : base(dataTempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToExcel(List<UserListDto> usertListDtos)
        {
            var excel = CreateExcelPackage("UserList.xlsx", excelpackage =>
            {
                var sheet = excelpackage.Workbook.Worksheets.Add(L("Users"));
                sheet.OutLineApplyStyle = true;

                AddHeader(sheet, L("UserName"), L("EmailAddress"), L("CreationTime"));

                AddObject(sheet, 2, usertListDtos, ex => ex.UserName, ex => ex.EmailAddress, ex => _timeZoneConverter.Convert(ex.CreationTime,AbpSession.TenantId,AbpSession.GetUserId()));



                var createTimeColumn = sheet.Column(3);
                createTimeColumn.Style.Numberformat.Format = "yyyy-MM-dd";

            });

            return excel;

        }
    }
}