using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task Validate(string captchaResponse);
    }
}