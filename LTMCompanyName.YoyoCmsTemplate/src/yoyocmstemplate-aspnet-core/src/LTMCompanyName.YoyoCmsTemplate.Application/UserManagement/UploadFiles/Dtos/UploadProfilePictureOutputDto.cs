using System;
using Abp.Web.Models;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.UploadFiles.Dtos
{
    public class UploadProfilePictureOutputDto : ErrorInfo
    {
        public UploadProfilePictureOutputDto()
        {
        }

        public UploadProfilePictureOutputDto(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }

        /// <summary>
        ///   
        /// </summary>
        public Guid? ProfilePictureId { get; set; }

        public string FileName { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}