using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.BLL.Services.StudentService;

public interface IStudentService
{
    DataGridModel<Student> GetAllGrid(int facultyId, int specialityId, int semesterId);
    
    List<Student> GetAll();
    
    List<Student> GetAll(int facultyId, int specialityId, int semesterId);

    Student Get(int id);
        
    Student Update(Student model);
        
    void Delete(int id);
        
    void DeleteMany(List<Student> students);
}