using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.NavigationService;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.Web.Controllers.admin
{
    [Route("api/[controller]")]
    [Authorize]

    public class NavigationController : Controller
    {
        private readonly INavigationService _navigationService;

        public NavigationController(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [HttpGet]
        [Route("")]
        public List<Faculty> GetNavigation()
        {
            return _navigationService.GetNavigation();
        }
    }
}