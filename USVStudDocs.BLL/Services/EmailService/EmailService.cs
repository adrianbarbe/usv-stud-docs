using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using MimeKit;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.DAL;
using USVStudDocs.Entities.Resources;

namespace USVStudDocs.BLL.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly MainContext _context;

    public EmailService(MainContext context)
    {
        _context = context;
    }

    public void SendEmail(string? recipientEmail, string? subject, string? body)
    {
        if (string.IsNullOrEmpty(subject))
        {
            subject = "Test USV Docs";
        }
        
        if (string.IsNullOrEmpty(body))
        {
            body = "This is test message sent from USV Docs";
        }

        var settingsKeys = new[]
        {
            SettingsKeys.SemesterSettings.oAuthEmailSenderEmail.Key,
            SettingsKeys.SemesterSettings.oAuthEmailAccessToken.Key,
            SettingsKeys.SemesterSettings.oAuthEmailRefreshToken.Key,
        };
        
        var clientId = Environment.GetEnvironmentVariable("GoogleClientId") ?? "";
        var clientSecret = Environment.GetEnvironmentVariable("GoogleClientSecret") ?? "";

        var settings = _context.Settings.Where(s => settingsKeys.Contains(s.Key)).ToList();
        
        var oAuthEmailSenderEmail =
            settings.FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.oAuthEmailSenderEmail.Key);
        
        var oAuthEmailAccessToken =
            settings.FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.oAuthEmailAccessToken.Key);
        
        var oAuthEmailRefreshToken =
            settings.FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.oAuthEmailRefreshToken.Key);

        if (oAuthEmailSenderEmail?.Value == null || oAuthEmailAccessToken?.Value == null || oAuthEmailRefreshToken?.Value == null)
        {
            throw new ValidationException("oAuthEmailRefreshToken or oAuthEmailAccessToken or oAuthEmailRefreshToken not found");
        }
        
        string accessToken = oAuthEmailAccessToken.Value;
        string refreshToken = oAuthEmailRefreshToken.Value;
        string senderEmail = oAuthEmailSenderEmail.Value;
        
        if (String.IsNullOrEmpty(recipientEmail))
        {
            recipientEmail = oAuthEmailSenderEmail.Value;
        }


        var clientSecrets = new ClientSecrets
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };


        var token = new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        var credential = CreateUserCredential(clientSecrets, token, oAuthEmailSenderEmail.Value);

        // Check if the access token has expired
        if (credential.Token.IsExpired(credential.Flow.Clock))
        {
            // Refresh the access token
            if (credential.RefreshTokenAsync(CancellationToken.None).Result)
            {
                accessToken = credential.Token.AccessToken;
                oAuthEmailAccessToken.Value = accessToken;
                _context.Settings.Update(oAuthEmailAccessToken);
                
                refreshToken = credential.Token.RefreshToken;
                oAuthEmailRefreshToken.Value = refreshToken;
                _context.Settings.Update(oAuthEmailRefreshToken);

                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Failed to refresh access token.");
            }
        }

        var service = new GmailService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential
        });

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using (var stream = new MemoryStream())
        {
            message.WriteTo(stream);
            var rawMessage = Convert.ToBase64String(stream.ToArray());

            var gmailMessage = new Message
            {
                Raw = rawMessage
            };
            service.Users.Messages.Send(gmailMessage, "me").Execute();
        }
    }

    private UserCredential CreateUserCredential(ClientSecrets clientSecrets, TokenResponse token, string oAuthEmailSenderEmail)
    {
        return new UserCredential(new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets,
                    Scopes = new[] { GmailService.Scope.GmailSend }
                }),
            oAuthEmailSenderEmail,
            token);
    }
}