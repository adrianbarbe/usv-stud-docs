using ePlato.CoreApp.Models.Shared.DataGrid;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.BLL.Services.FacultyPersonService
{
    public interface IFacultyPersonService
    {
        
        DataGridModel<FacultyPerson> GetAll(RequestQueryModel requestQueryModel);

        List<FacultyPerson> GetAll();

        FacultyPerson Get(int id);

        FacultyPerson Update(FacultyPerson model);

        void Delete(int id);
    }
}