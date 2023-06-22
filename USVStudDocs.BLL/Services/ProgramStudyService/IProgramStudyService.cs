using ePlato.CoreApp.Models.Shared.DataGrid;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.BLL.Services.ProgramStudyService
{
    public interface IProgramStudyService
    {
        DataGridModel<ProgramStudy> GetAll(RequestQueryModel requestQueryModel);
        
        List<ProgramStudy> GetAll();
        
        List<ProgramStudy> GetAllByFaculty(int facultyId);

        ProgramStudy Get(int id);

        ProgramStudy Update(ProgramStudy model);

        void Delete(int id);

    }
}