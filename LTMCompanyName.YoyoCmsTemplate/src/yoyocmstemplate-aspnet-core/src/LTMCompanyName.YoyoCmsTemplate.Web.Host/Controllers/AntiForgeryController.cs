using Microsoft.AspNetCore.Antiforgery;
using LTMCompanyName.YoyoCmsTemplate.Controllers;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Controllers
{
    public class AntiForgeryController : YoyoCmsTemplateControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
