using USVStudDocs.Entities.Constants;

namespace USVStudDocs.Entities.Resources
{
    
    public static partial class SettingsKeys
    {
        public static partial class SemesterSettings
        {
            public static readonly SettingsKeyType EducationalYearStartDate = new SettingsKeyType {Key = "EducationalYearStartDate", Type = SettingsType.DateTime};
            public static readonly SettingsKeyType CertificateReasons = new SettingsKeyType {Key = "CertificateReasons", Type = SettingsType.ArrayStrings};
        }
    }
}