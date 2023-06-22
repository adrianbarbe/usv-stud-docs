using ePlato.CoreApp.Models.Shared.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.FacultyService;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.Web.Controllers.admin
{
    [Route("api/[controller]")]
    [Authorize]

    public class FacultyController : Controller
    {
        private readonly IFacultiesService _facultiesService;

        public FacultyController(IFacultiesService facultiesService)
        {
            _facultiesService = facultiesService;
        }
        
        [HttpGet]
        public DataGridModel<Faculty> GetAll([FromQuery] RequestQueryModel requestQueryModel)
        {
            return _facultiesService.GetAll(requestQueryModel);
        }

        [HttpGet]
        [Route("getAll")]
        public List<Faculty> GetAll()
        {
            return _facultiesService.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public Faculty Get(int id)
        {
            return _facultiesService.Get(id);
        }
        
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        public Faculty Save([FromBody] Faculty faculty)
        {
            return _facultiesService.Update(faculty);
        }
        
        [HttpPut]
        [Authorize(Policy = Policies.Admin)]
        public Faculty Update([FromBody] Faculty faculty)
        {
            return _facultiesService.Update(faculty);
        }

        [HttpDelete]
        [Authorize(Policy = Policies.Admin)]
        [Route("{id}")]
        public void Delete(int id)
        {
            _facultiesService.Delete(id);
        }
    }
}