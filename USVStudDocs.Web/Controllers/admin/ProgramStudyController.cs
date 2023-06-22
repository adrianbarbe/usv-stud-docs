using ePlato.CoreApp.Models.Shared.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.ProgramStudyService;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.Web.Controllers.admin
{
    [Route("api/[controller]")]
    [Authorize]

    public class ProgramStudyController : Controller
    {
        private readonly IProgramStudyService _programStudyService;

        public ProgramStudyController(IProgramStudyService programStudyService)
        {
            _programStudyService = programStudyService;
        }
        
        [HttpGet]
        public DataGridModel<ProgramStudy> GetAll([FromQuery] RequestQueryModel requestQueryModel)
        {
            return _programStudyService.GetAll(requestQueryModel);
        }

        [HttpGet]
        [Route("getAll")]
        public List<ProgramStudy> GetAll()
        {
            return _programStudyService.GetAll();
        }

        [HttpGet]
        [Route("getAllByFaculty/{facultyId}")]
        public List<ProgramStudy> GetAllByFaculty(int facultyId)
        {
            return _programStudyService.GetAllByFaculty(facultyId);
        }
        
        [HttpGet]
        [Route("{id}")]
        public ProgramStudy Get(int id)
        {
            return _programStudyService.Get(id);
        }
        
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        public ProgramStudy Save([FromBody] ProgramStudy programStudy)
        {
            return _programStudyService.Update(programStudy);
        }
        
        [HttpPut]
        [Authorize(Policy = Policies.Admin)]
        public ProgramStudy Update([FromBody] ProgramStudy programStudy)
        {
            return _programStudyService.Update(programStudy);
        }

        [HttpDelete]
        [Authorize(Policy = Policies.Admin)]
        [Route("{id}")]
        public void Delete(int id)
        {
            _programStudyService.Delete(id);
        }
    }
}