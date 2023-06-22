using TinyCsvParser.Mapping;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Services.StudentsImportService
{
    public class StudentsImportCsvMapping : CsvMapping<StudentImport>
    {
        public StudentsImportCsvMapping() : base()
        {
            MapProperty(0, x => x.Surname);
            MapProperty(1, x => x.Patronymic);
            MapProperty(2, x => x.Name);
            MapProperty(3, x => x.Email);
            MapProperty(4, x => x.FieldOfStudy);
            MapProperty(5, x => x.ProgramStudy);
            MapProperty(6, x => x.Year);
            MapProperty(7, x => x.FinancialStatus);
        }
    }
}