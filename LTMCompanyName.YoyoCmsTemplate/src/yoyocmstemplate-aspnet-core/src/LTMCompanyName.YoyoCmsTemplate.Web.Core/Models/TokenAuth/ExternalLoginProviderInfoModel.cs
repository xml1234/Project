using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External;

namespace LTMCompanyName.YoyoCmsTemplate.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
