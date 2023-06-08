using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.StudentService;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.Web.Controllers.admin
{
    [Route("api/[controller]")]
    [Authorize(Policy = Policies.Admin)]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        
        [HttpGet]
        [Route("{facultyId}/{specialityId}/{semesterId}")]
        public DataGridModel<Student> GetAllGrid(int facultyId, int specialityId, int semesterId)
        {
            return _studentService.GetAllGrid(facultyId, specialityId, semesterId);
        }
        
      
        [HttpGet]
        [Route("getAll/{facultyId}/{specialityId}/{semesterId}")]
        public List<Student> GetAllByFacultySpecialitySemester(int facultyId, int specialityId, int semesterId)
        {
            return _studentService.GetAll(facultyId, specialityId, semesterId);
        }

        [HttpGet]
        [Route("{id}")]
        public Student Get(int id)
        {
            return _studentService.Get(id);
        }
        
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        public Student Save([FromBody] Student academicGroup)
        {
            return _studentService.Update(academicGroup);
        }
        
        [HttpPut]
        [Authorize(Policy = Policies.Admin)]
        public Student Update([FromBody] Student student)
        {
            return _studentService.Update(student);
        }
        
        [HttpDelete]
        [Authorize(Policy = Policies.Admin)]
        [Route("{id}")]
        public void Delete(int id)
        {
            _studentService.Delete(id);
        }
        
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        [Route("deletes")]
        public void DeleteMany([FromBody] List<Student> academicGroups)
        {
            _studentService.DeleteMany(academicGroups);
        }
    }
}