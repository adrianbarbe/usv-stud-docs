using ePlato.CoreApp.Models.Shared.DataGrid;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.BLL.Services.FacultyService
{
    public interface IFacultiesService
    {
        
        DataGridModel<Faculty> GetAll(RequestQueryModel requestQueryModel);

        List<Faculty> GetAll();

        Faculty Get(int id);

        Faculty Update(Faculty model);

        void Delete(int id);
    }
}