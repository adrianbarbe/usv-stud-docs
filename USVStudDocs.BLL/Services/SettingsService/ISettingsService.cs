using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Services.SettingsService;

public interface ISettingsService
{
    AdminConfiguration Get();
    List<string> GetCertificateReasons();
    
    AdminConfiguration Update(AdminConfiguration model);

    void AuthorizeEmailAuth(string code);
}