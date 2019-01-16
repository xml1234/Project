using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
