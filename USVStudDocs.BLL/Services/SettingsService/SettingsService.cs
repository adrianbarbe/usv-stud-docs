using System.IdentityModel.Tokens.Jwt;
using RestSharp;
using Serilog;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.BLL.Extensions;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Resources;
using USVStudDocs.Models;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Services.SettingsService;

public class SettingsService : ISettingsService
{
    private readonly MainContext _context;
    private readonly RestClient _googleRestClient;

    public SettingsService(MainContext context)
    {
        _context = context;
        _googleRestClient = new RestClient("https://oauth2.googleapis.com/token");
    }

    public AdminConfiguration Get()
    {
        var settingsKeys = new[]
        {
            SettingsKeys.SemesterSettings.EducationalYearStartDate.Key,
            SettingsKeys.SemesterSettings.CertificateReasons.Key,
            SettingsKeys.SemesterSettings.oAuthEmailSenderEmail.Key,
        };

        var settings = _context.Settings.Where(s => settingsKeys.Contains(s.Key)).ToList();
        if (settings.Count < settingsKeys.Length)
        {
            return new AdminConfiguration
            {
                EducationYearStart = null,
                CertificateReasons = new List<string>(),
                oAuthEmailSenderEmail = null,
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
        
        var oAuthEmailSenderEmail =
            settings.FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.oAuthEmailSenderEmail.Key);

        return new AdminConfiguration
        {
            EducationYearStart = educationalYearStartDate,
            CertificateReasons = certificateReasons.ToList(),
            oAuthEmailSenderEmail = oAuthEmailSenderEmail?.Value ?? String.Empty,
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

    public void AuthorizeEmailAuth(string code)
    {
        var token = AuthorizeCodeRequest(code);

        if (token?.IdToken == null)
        {
            throw new ValidationException("Cannot validate token");
        }
        
        var stream = token.IdToken;  
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(stream);
        var tokenS = jsonToken as JwtSecurityToken;

        if (tokenS == null)
        {
            throw new ValidationException("Cannot read token");
        }

        var email = tokenS.Claims.FirstOrDefault(c => c.Type == "email");
        
        var settingsKeys = new[]
        {
            SettingsKeys.SemesterSettings.oAuthEmailSenderEmail.Key,
            SettingsKeys.SemesterSettings.oAuthEmailAccessToken.Key,
            SettingsKeys.SemesterSettings.oAuthEmailRefreshToken.Key,
        };

        var settings = _context.Settings.Where(s => settingsKeys.Contains(s.Key)).ToList();

        var oAuthEmailSenderEmail =
            settings.FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.oAuthEmailSenderEmail.Key);
        
        var oAuthEmailAccessToken =
            settings.FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.oAuthEmailAccessToken.Key);
        
        var oAuthEmailRefreshToken =
            settings.FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.oAuthEmailRefreshToken.Key);

        if (oAuthEmailSenderEmail == null)
        {
            var settingsEntity = new SettingsEntity
            {
                Key = SettingsKeys.SemesterSettings.oAuthEmailSenderEmail.Key,
                Type = SettingsKeys.SemesterSettings.oAuthEmailSenderEmail.Type,
                Value = email?.Value ?? "",
            };

            _context.Settings.Add(settingsEntity);
        }
        else
        {
            oAuthEmailSenderEmail.Value = email?.Value ?? "";
            _context.Settings.Update(oAuthEmailSenderEmail);
        }
        
        if (oAuthEmailAccessToken == null)
        {
            var settingsEntity = new SettingsEntity
            {
                Key = SettingsKeys.SemesterSettings.oAuthEmailAccessToken.Key,
                Type = SettingsKeys.SemesterSettings.oAuthEmailAccessToken.Type,
                Value = token.AccessToken,
            };

            _context.Settings.Add(settingsEntity);
        }
        else
        {
            oAuthEmailAccessToken.Value = token.AccessToken;
            _context.Settings.Update(oAuthEmailAccessToken);
        }
        
        if (oAuthEmailRefreshToken == null)
        {
            var settingsEntity = new SettingsEntity
            {
                Key = SettingsKeys.SemesterSettings.oAuthEmailRefreshToken.Key,
                Type = SettingsKeys.SemesterSettings.oAuthEmailRefreshToken.Type,
                Value = token.RefreshToken,
            };

            _context.Settings.Add(settingsEntity);
        }
        else
        {
            oAuthEmailRefreshToken.Value = token.RefreshToken;
            _context.Settings.Update(oAuthEmailRefreshToken);
        }
        _context.SaveChanges();
    }


    private AuthTokenResponse? AuthorizeCodeRequest(string code)
    {
        var clientId = Environment.GetEnvironmentVariable("GoogleClientId") ?? "";
        var clientSecret = Environment.GetEnvironmentVariable("GoogleClientSecret") ?? "";
        var redirectUri = Environment.GetEnvironmentVariable("GoogleEmailRedirectUri") ?? "";

        var codeExchangeRequest = new RestRequest("", Method.Post);

        codeExchangeRequest.AddQueryParameter("code", code);
        codeExchangeRequest.AddQueryParameter("client_id", clientId);
        codeExchangeRequest.AddQueryParameter("client_secret", clientSecret);
        codeExchangeRequest.AddQueryParameter("redirect_uri", redirectUri);
        codeExchangeRequest.AddQueryParameter("grant_type", "authorization_code");

        var token = new AuthTokenResponse();
        try
        {
            var tokenResponse = _googleRestClient.Execute<AuthTokenResponse>(codeExchangeRequest);
            token = tokenResponse.Data;
        }
        catch (Exception e)
        {
            Log.ForContext<SettingsService>().Error("Something went wrong with token authentication on Google");
        }

        return token;
    }
}