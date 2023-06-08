using TinyCsvParser.Mapping;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Services.StudentsImportService
{
    public class StudentsConcatenatedImportCsvMapping : CsvMapping<StudentImportConcatenated>
    {
        public StudentsConcatenatedImportCsvMapping() : base()
        {
            MapProperty(0, x => x.SurnameNamePatronymic);
            MapProperty(1, x => x.Email);
            MapProperty(2, x => x.FieldOfStudy);
            MapProperty(3, x => x.ProgramStudy);
            MapProperty(4, x => x.Year);
            MapProperty(5, x => x.FinancialStatus);
        }
    }
}