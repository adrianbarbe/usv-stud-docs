using USVStudDocs.BLL.Exceptions;
using USVStudDocs.BLL.Extensions;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Resources;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Services.SettingsService;

public class SettingsService : ISettingsService
{
    private readonly MainContext _context;

    public SettingsService(MainContext context)
    {
        _context = context;
    }
    
    public AdminConfiguration Get()
    {
        var settingsKeys = new[]
        {
            SettingsKeys.SemesterSettings.EducationalYearStartDate.Key,
            SettingsKeys.SemesterSettings.CertificateReasons.Key,
        };

        var settings = _context.Settings.Where(s => settingsKeys.Contains(s.Key)).ToList();
        if (settings.Count < settingsKeys.Length)
        {
            return new AdminConfiguration
            {
                EducationYearStart = null,
                CertificateReasons = new List<string>(),
            };
        }
        
        var educationalYearStartDateStr = settings
            .FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.EducationalYearStartDate.Key);

        var educationalYearStartDate = DateTime.Today;
        if (!string.IsNullOrEmpty(educationalYearStartDateStr?.Value))
        {
            educationalYearStartDate = educationalYearStartDateStr.ParseToDateTime();
        }

        var certificateReasons = settings
            .FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.CertificateReasons.Key)
            .ParseToArrayStrings();
        
        return new AdminConfiguration
        {
            EducationYearStart = educationalYearStartDate,
            CertificateReasons = certificateReasons.ToList(),
        };
    }

    public List<string> GetCertificateReasons()
    {
        var settingsKeys = new[]
        {
            SettingsKeys.SemesterSettings.CertificateReasons.Key,
        };

        var settings = _context.Settings.Where(s => settingsKeys.Contains(s.Key)).ToList();
        if (settings.Count >= settingsKeys.Length)
        {
            var certificateReasons = settings
                .FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.CertificateReasons.Key)
                .ParseToArrayStrings();
            
            return certificateReasons.ToList();
        }

        return new List<string>();
    }

    public AdminConfiguration Update(AdminConfiguration model)
    {
        var settingsKeys = new[]
        {
            SettingsKeys.SemesterSettings.EducationalYearStartDate.Key,
            SettingsKeys.SemesterSettings.CertificateReasons.Key,
        };

        var settings = _context.Settings.Where(s => settingsKeys.Contains(s.Key)).ToList();
        
        if (settings.Count < settingsKeys.Length)
        {

            var settingsEntity = new SettingsEntity
            {
                Key = SettingsKeys.SemesterSettings.EducationalYearStartDate.Key,
                Type = SettingsKeys.SemesterSettings.EducationalYearStartDate.Type,
                Value = model.EducationYearStart.ToString(),
            };

            _context.Settings.Add(settingsEntity);
            
            var settingsCertificateEntity = new SettingsEntity
            {
                Key = SettingsKeys.SemesterSettings.CertificateReasons.Key,
                Type = SettingsKeys.SemesterSettings.CertificateReasons.Type,
                Value = string.Join(",", model.CertificateReasons),
            };

            _context.Settings.Add(settingsCertificateEntity);
            
            _context.SaveChanges();

            return new AdminConfiguration
            {
                EducationYearStart = model.EducationYearStart,
                CertificateReasons = model.CertificateReasons,
            };
        }

        var educationalYearStartDate = settings
            .FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.EducationalYearStartDate.Key);

        if (educationalYearStartDate == null)
        {
            throw new ValidationException("Cannot find educationalYearStartDate");
        }

        educationalYearStartDate.Value = model.EducationYearStart.ToString();
        
        var certificateReasons = settings
            .FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.CertificateReasons.Key);

        if (certificateReasons == null)
        {
            throw new ValidationException("Cannot find certificateReasons");
        }

        certificateReasons.Value = string.Join(",", model.CertificateReasons);

        _context.SaveChanges();

        return new AdminConfiguration
        {
            EducationYearStart = model.EducationYearStart,
            CertificateReasons = model.CertificateReasons,
        };
    }
}