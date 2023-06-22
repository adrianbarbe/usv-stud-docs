using ePlato.CoreApp.Models.Shared.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.FacultyPersonService;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.Web.Controllers.admin
{
    [Route("api/[controller]")]
    [Authorize]

    public class FacultyPersonController : Controller
    {
        private readonly IFacultyPersonService _facultyPersonService;

        public FacultyPersonController(IFacultyPersonService facultyPersonService)
        {
            _facultyPersonService = facultyPersonService;
        }
        
        [HttpGet]
        public DataGridModel<FacultyPerson> GetAll([FromQuery] RequestQueryModel requestQueryModel)
        {
            return _facultyPersonService.GetAll(requestQueryModel);
        }

        [HttpGet]
        [Route("getAll")]
        public List<FacultyPerson> GetAll()
        {
            return _facultyPersonService.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public FacultyPerson Get(int id)
        {
            return _facultyPersonService.Get(id);
        }
        
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        public FacultyPerson Save([FromBody] FacultyPerson faculty)
        {
            return _facultyPersonService.Update(faculty);
        }
        
        [HttpPut]
        [Authorize(Policy = Policies.Admin)]
        public FacultyPerson Update([FromBody] FacultyPerson faculty)
        {
            return _facultyPersonService.Update(faculty);
        }

        [HttpDelete]
        [Authorize(Policy = Policies.Admin)]
        [Route("{id}")]
        public void Delete(int id)
        {
            _facultyPersonService.Delete(id);
        }
    }
}