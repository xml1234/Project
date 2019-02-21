using System.IO;
using Abp.Auditing;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Controllers
{
    public class FileController : YoyoCmsTemplateControllerBase
    {
        private readonly IAppFolder _appFolders;
        private readonly IDataTempFileCacheManager _dataTempFileCacheManager;

        public FileController(IAppFolder appFolders, IDataTempFileCacheManager dataTempFileCacheManager)
        {
            _appFolders = appFolders;
            _dataTempFileCacheManager = dataTempFileCacheManager;
        }

        [DisableAuditing]//取消审计日志
        public ActionResult DownloadTempFilePath(FileDto file)
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

        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _dataTempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes==null)
            {
                return NotFound("当前文件信息不存在！");
            }

            return File(fileBytes, file.FileType, file.FileName);

        }


    }
}


