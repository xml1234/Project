using System;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.Helpers;
using LTMCompanyName.YoyoCmsTemplate.IO;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.UploadFiles.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Controllers
{
    /// <summary>
    /// 上传文件的控制器基类
    /// </summary>
    public abstract class UploadFilesControllerBase : YoyoCmsTemplateControllerBase
    {
        private readonly IAppFolder _appFolder;
        private const int MaxProfilePictureSize = 5242880; //5MB

        private readonly IDataFileObjectManager _dataFileObjectManager;
        protected UploadFilesControllerBase(IAppFolder appFolder, IDataFileObjectManager dataFileObjectManager)
        {
            _appFolder = appFolder;
            _dataFileObjectManager = dataFileObjectManager;
            LocalizationSourceName = YoyoCmsTemplateConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 上传头像的输出
        /// </summary>
        /// <returns></returns>
        public UploadProfilePictureOutputDto UploadProfilePicture()
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (profilePictureFile.Length > MaxProfilePictureSize)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit", AppConsts.MaxProfilePictureBytesUserFriendlyValue));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new Exception("Uploaded file is not an accepted image file !");
                }

                //Delete old temp profile pictures
                AppFileHelper.DeleteFilesInFolderIfExists(_appFolder.TempFileDownloadFolder, "userProfileImage_" + AbpSession.GetUserId());

                //Save new picture
                var fileInfo = new FileInfo(profilePictureFile.FileName);
                var tempFileName = "userProfileImage_" + AbpSession.GetUserId() + fileInfo.Extension;
                var tempFilePath = Path.Combine(_appFolder.TempFileDownloadFolder, tempFileName);
                System.IO.File.WriteAllBytes(tempFilePath, fileBytes);

                using (var bmpImage = new Bitmap(tempFilePath))
                {
                    return new UploadProfilePictureOutputDto
                    {
                        FileName = tempFileName,
                        Width = bmpImage.Width,
                        Height = bmpImage.Height
                    };
                }
            }
            catch (UserFriendlyException ex)
            {
                return new UploadProfilePictureOutputDto(new ErrorInfo(ex.Message));
            }
        }

        /// <summary>
        /// 上传头像返回图片Id
        /// </summary>
        public async Task<UploadProfilePictureOutputDto> UploadProfilePictureReturnFileId()
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (profilePictureFile.Length > MaxProfilePictureSize)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit", AppConsts.MaxProfilePictureBytesUserFriendlyValue));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Info", AppConsts.MaxProfilePictureBytesUserFriendlyValue));
                }

                var storedFile = new DataFileObject(AbpSession.TenantId, fileBytes.ToArray());
                await _dataFileObjectManager.SaveAsync(storedFile);

                return new UploadProfilePictureOutputDto
                {
                    FileName = profilePictureFile.FileName,
                    ProfilePictureId = storedFile.Id
                };
            }
            catch (UserFriendlyException ex)
            {
                return new UploadProfilePictureOutputDto(new ErrorInfo(ex.Message));
            }
        }
    }
}