namespace USVStudDocs.BLL.Services.EmailService;

public interface IEmailService
{
    void SendEmail(string recipientEmail, string subject, string body);
}