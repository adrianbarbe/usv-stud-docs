using USVStudDocs.Models;

namespace USVStudDocs.BLL.Services.OAuth2Service;

public interface IOAuth2Service
{
    AuthTokenResponse? AuthorizeCode(string code);
}