using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.OAuth2Service;
using USVStudDocs.Models;

namespace USVStudDocs.Web.Controllers
{
    [Route("oauth2")]
    public class OAuth2Controller : Controller
    {
        private readonly IOAuth2Service _ioAuth2Service;

        public OAuth2Controller(IOAuth2Service ioAuth2Service)
        {
            _ioAuth2Service = ioAuth2Service;
        }

        [HttpPost]
        [Route("authorize")]
        public AuthTokenResponse? AuthorizeCode([FromBody] AuthCode authCode)
        {
            return _ioAuth2Service.AuthorizeCode(authCode.Code);
        }
    }
}