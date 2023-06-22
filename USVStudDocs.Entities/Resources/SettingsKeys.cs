using USVStudDocs.Entities.Constants;

namespace USVStudDocs.Entities.Resources
{
    
    public static partial class SettingsKeys
    {
        public static partial class SemesterSettings
        {
            public static readonly SettingsKeyType EducationalYearStartDate = new SettingsKeyType {Key = "EducationalYearStartDate", Type = SettingsType.DateTime};
            public static readonly SettingsKeyType CertificateReasons = new SettingsKeyType {Key = "CertificateReasons", Type = SettingsType.ArrayStrings};
            public static readonly SettingsKeyType oAuthEmailSenderEmail = new SettingsKeyType {Key = "oAuthEmailSenderEmail", Type = SettingsType.String};
            public static readonly SettingsKeyType oAuthEmailAccessToken = new SettingsKeyType {Key = "oAuthEmailAccessToken", Type = SettingsType.String};
            public static readonly SettingsKeyType oAuthEmailRefreshToken = new SettingsKeyType {Key = "oAuthEmailRefreshToken", Type = SettingsType.String};
        }
    }
}