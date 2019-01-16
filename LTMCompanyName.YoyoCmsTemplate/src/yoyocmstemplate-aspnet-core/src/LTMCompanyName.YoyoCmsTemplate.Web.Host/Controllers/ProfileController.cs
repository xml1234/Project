using Abp.AspNetCore.Mvc.Authorization;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : UploadFilesControllerBase
    {
        public ProfileController(IAppFolder appFolder, IDataFileObjectManager dataFileObjectManager) : base(appFolder, dataFileObjectManager)
        {
        }
    }
}
