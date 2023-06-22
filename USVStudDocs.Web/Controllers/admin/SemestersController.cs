using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.SemesterService;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.Web.Controllers.admin
{
    [Route("api/[controller]")]
    [Authorize]
    public class SemestersController : Controller
    {
        private readonly ISemesterService _semesterService;

        public SemestersController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        [HttpGet]
        [Route("")]
        public List<Semester> GetSemesters()
        {
            return _semesterService.GetSemesters();
        }
    }
}