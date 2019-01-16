using System.IO;
using Abp.Auditing;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Controllers
{
    public class FileController : YoyoCmsTemplateControllerBase
    {
        private readonly IAppFolder _appFolders;

        public FileController(IAppFolder appFolders)
        {
            _appFolders = appFolders;
        }

        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var filePath = Path.Combine(_appFolders.TempFileDownloadFolder, file.FileToken);
            if (!System.IO.File.Exists(filePath))
            {
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);
            return File(fileBytes, file.FileType, file.FileName);
        }
    }
}


