using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace LTMCompanyName.YoyoCmsTemplate.Localization
{
    public static class YoyoCmsTemplateLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(YoyoCmsTemplateConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(YoyoCmsTemplateLocalizationConfigurer).GetAssembly(),
                        "LTMCompanyName.YoyoCmsTemplate.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
