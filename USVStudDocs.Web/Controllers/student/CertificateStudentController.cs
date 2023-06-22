using ePlato.CoreApp.Models.Shared.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.StudentCertificateService;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Shared.DataGrid;
using USVStudDocs.Models.Student;

namespace USVStudDocs.Web.Controllers.student
{
    [Route("api/certificateStudent")]
    public class CertificateStudentController : Controller
    {
        private readonly IStudentCertificateService _studentCertificateService;

        public CertificateStudentController(IStudentCertificateService studentCertificateService)
        {
            _studentCertificateService = studentCertificateService;
        }
        
        [HttpGet]
        [Route("me")]
        [Authorize(Policy = Policies.Student)]
        public Student GetMe()
        {
            return _studentCertificateService.GetCurrentStudentInfo();
        }

        [HttpGet]
        [Route("")]
        [Authorize(Policy = Policies.Student)]
        public DataGridModel<StudentCertificateListItem> GetList([FromQuery] RequestQueryModel requestQueryModel)
        {
            return _studentCertificateService.GetList(requestQueryModel);
        }
        
        [HttpPost]
        [Route("")]
        [Authorize(Policy = Policies.Student)]
        public void CreateNew([FromBody] StudentCertificateCreateItem studentCertificateCreateItem)
        {
            _studentCertificateService.CreateCertificate(studentCertificateCreateItem);
        }
    }
}