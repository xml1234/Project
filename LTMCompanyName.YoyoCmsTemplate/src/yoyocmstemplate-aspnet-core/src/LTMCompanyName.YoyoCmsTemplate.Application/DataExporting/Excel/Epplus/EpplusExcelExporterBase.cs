using System;
using System.Collections.Generic;
using System.Text;
using Abp.Dependency;
using Castle.Core.Internal;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Net.MimeTypes;
using OfficeOpenXml;

namespace LTMCompanyName.YoyoCmsTemplate.DataExporting.Excel.Epplus
{
    public class EpplusExcelExporterBase:YoyoCmsTemplateAppServiceBase,ITransientDependency
    {
        private readonly IDataTempFileCacheManager _dataTempFileCacheManager;

        public EpplusExcelExporterBase(IDataTempFileCacheManager dataTempFileCacheManager)
        {
            _dataTempFileCacheManager = dataTempFileCacheManager;
        }

        protected FileDto CreateExcelPackage(string fileName, Action<ExcelPackage> createor)
        {
            var file = new FileDto(fileName,
                MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            using (var excelPackage = new ExcelPackage())
            {
                createor(excelPackage);
                Save(excelPackage, file);
            }

            return file;
        }

        protected void AddHeader(ExcelWorksheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            for (int i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, headerTexts[i]);
            }
        }

        protected void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
        {
            sheet.Cells[1, columnIndex].Value = headerText;
            sheet.Cells[1, columnIndex].Style.Font.Bold = true;
        }

        protected void AddObject<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items,
            params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
                }
            }

        }

        protected void Save(ExcelPackage excelPackage, FileDto file)
        {
            _dataTempFileCacheManager.SetFile(file.FileToken,excelPackage.GetAsByteArray());
        }


    }
}
