using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace DiveCRM.Localization
{
    public static class DiveCRMLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(DiveCRMConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(DiveCRMLocalizationConfigurer).GetAssembly(),
                        "DiveCRM.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
