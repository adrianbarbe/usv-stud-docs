using System.Text;
using ePlato.CoreApp.Models.Shared.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.SecretaryCertificateService;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Secretary;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.Web.Controllers.secretary
{
    [Route("api/certificateSecretary")]
    public class CertificateSecretaryController : Controller
    {
        private readonly ISecretaryCertificateService _secretaryCertificateService;

        public CertificateSecretaryController(ISecretaryCertificateService secretaryCertificateService)
        {
            _secretaryCertificateService = secretaryCertificateService;
        }
        
        [HttpGet]
        [Route("getPrint/{id}")]
        [Authorize(Policy = Policies.Secretary)]
        public SecretaryCertificatePrint GetPrint(int id)
        {
            return _secretaryCertificateService.GetPrint(id);
        }

        [HttpGet]
        [Route("{status}")]
        [Authorize(Policy = Policies.Secretary)]
        public DataGridModel<SecretaryCertificateListItem> GetList([FromQuery] RequestQueryModel requestQueryModel, CertificateStatus status)
        {
            return _secretaryCertificateService.GetList(requestQueryModel, status);
        }

        [HttpGet]
        [Route("count")]
        [Authorize(Policy = Policies.Secretary)]
        public SecretaryCertificateSummary GetCount()
        {
            return _secretaryCertificateService.GetCount();
        }

        [HttpPut]
        [Route("confirm/{id}")]
        [Authorize(Policy = Policies.Secretary)]
        public void ConfirmItem(int id)
        {
            _secretaryCertificateService.ConfirmItem(id);
        }
        
        [HttpPut]
        [Route("confirmPrint/{id}")]
        [Authorize(Policy = Policies.Secretary)]
        public void ConfirmPrint(int id)
        {
            _secretaryCertificateService.ConfirmPrint(id);
        }
        
        [HttpPut]
        [Route("confirmSignature/{id}")]
        [Authorize(Policy = Policies.Secretary)]
        public void ConfirmSignature(int id)
        {
            _secretaryCertificateService.ConfirmSignature(id);
        }

        [HttpPut]
        [Route("reject/{id}")]
        [Authorize(Policy = Policies.Secretary)]
        public void RejectItem([FromBody] SecretaryCertificateUpdateItem secretaryCertificateUpdateItem, int id)
        {
            _secretaryCertificateService.RejectItem(id, secretaryCertificateUpdateItem);
        }
        
        [HttpGet]
        [Route("archive/getAll")]
        [Authorize(Policy = Policies.Secretary)]
        public IActionResult GetArchive()
        {
            var data = _secretaryCertificateService.GetArchive();
            
            var csvContent = ConvertToCsv(data);
            var csvBytes = Encoding.UTF8.GetBytes(csvContent);

            var memoryStream = new MemoryStream(csvBytes);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return File(memoryStream, "text/csv", "data.csv");
        }
        
        private string ConvertToCsv(IEnumerable<SecretaryCertificateListItem> data)
        {
            var csv = new StringBuilder();

            csv.AppendLine("Nr. înregistrare,Data înregistrării,Nume student," +
                           "Prenume student,Domeniu/program de studii,An de studiu," +
                           "Finanțare,Motiv adeverință");

            foreach (var person in data)
            {
                csv.AppendLine($"{person.RegistrationNumber},{person.RegistrationDate}," +
                               $"{person.Student.Surname},{person.Student.Name},{MapFieldOfStudyToStr(person.Student.YearSemester.FieldOfStudy)}/{person.Student.ProgramStudy.Name}," +
                               $"{person.Student.YearSemester.YearNumber},{MapFinancialStatusToStr(person.Student.FinancialStatus)}," +
                               $"{person.CertificateReason}");
            }

            return csv.ToString();
        }

        private string MapFieldOfStudyToStr(FieldOfStudy fieldOfStudy)
        {
            switch (fieldOfStudy)
            {
                case FieldOfStudy.Bachelor:
                    return "Licenta";
                case FieldOfStudy.Master:
                    return "Master";
                case FieldOfStudy.ProfessionalConversion:
                    return "Conversie profesionala";
                default:
                    return "-";
;            }
        }
        private string MapFinancialStatusToStr(FinancialStatus status)
        {
            if (status == FinancialStatus.Budget)
            {
                return "Buget";
            }
            else if (status == FinancialStatus.TuitionFee)
            {
                return "Cu taxa";
            }
            else
            {
                return "-";
            }
        }
    }
}