using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.SettingsService;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;

namespace USVStudDocs.Web.Controllers.admin
{
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
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
    }
}