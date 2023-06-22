using ePlato.CoreApp.Models.Shared.DataGrid;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Shared.DataGrid;
using USVStudDocs.Models.Student;

namespace USVStudDocs.BLL.Services.StudentCertificateService;

public interface IStudentCertificateService
{
    DataGridModel<StudentCertificateListItem> GetList(RequestQueryModel requestQueryModel);
    void CreateCertificate(StudentCertificateCreateItem studentCertificateCreateItem);

    Student GetCurrentStudentInfo();
}