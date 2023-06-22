using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.EmailService;
using USVStudDocs.BLL.Services.SettingsService;
using USVStudDocs.Models;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;

namespace USVStudDocs.Web.Controllers.admin
{
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;
        private readonly IEmailService _emailService;

        public SettingsController(ISettingsService settingsService, IEmailService emailService)
        {
            _settingsService = settingsService;
            _emailService = emailService;
        }
        
        [HttpGet]
        [Authorize(Policy = Policies.Admin)]
        public AdminConfiguration Get()
        {
            return _settingsService.Get();
        }
        
        [HttpPut]
        [Authorize(Policy = Policies.Admin)]
        public AdminConfiguration Update([FromBody] AdminConfiguration settings)
        {
            return _settingsService.Update(settings);
        }
        
        [HttpGet]
        [Authorize(Policy = Policies.Student)]
        [Route("certificateReasons")]
        public List<string> GetCertificateReasons()
        {
            return _settingsService.GetCertificateReasons();
        }
        
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        [Route("testEmail")]
        public void TestEmail()
        {
            _emailService.SendEmail(null, null, null);
        }

        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        [Route("authorizeEmailAuth")]
        public void AuthorizeEmailAuth([FromBody] AuthCode authCode)
        {
            _settingsService.AuthorizeEmailAuth(authCode.Code);
        }
    }
}