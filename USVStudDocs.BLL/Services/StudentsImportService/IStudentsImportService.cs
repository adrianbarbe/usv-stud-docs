using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Services.StudentsImportService;

public interface IStudentsImportService
{
    List<Student> SaveStudents(string csvContent, int facultyId, bool studentNameConcatenated);
}