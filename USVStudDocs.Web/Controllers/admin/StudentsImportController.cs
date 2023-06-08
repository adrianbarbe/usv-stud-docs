using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using USVStudDocs.BLL;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.BLL.Services.StudentsImportService;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;

namespace USVStudDocs.Web.Controllers.admin
{
    [Route("api/[controller]")]
    public class StudentsImportController : Controller
    {
        private readonly IStudentsImportService _studentsImportService;

        public StudentsImportController(IStudentsImportService studentsImportService)
        {
            _studentsImportService = studentsImportService;
        }

        [HttpPost]
        [Route("import/{facultyId}")]
        [Authorize(Policy = Policies.Admin)]
        public List<Student> Import(IFormFile file, bool studentNameConcatenated, int facultyId)
        {
            var fileName = file.FileName;

            var fileExtension = Path.GetExtension(fileName);
            var fileMimeType = MimeTypes.GetMimeType(fileExtension);

            if (fileMimeType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Assuming the first worksheet

                    List<string> xlsxCsv = new List<string>();

                    for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var rowData = new List<string>();

                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Value?.ToString() ?? string.Empty;
                            rowData.Add(cellValue);
                        }

                        xlsxCsv.Add(string.Join(",", rowData));
                    }

                    string xlsxCsvString = string.Join("\n", xlsxCsv);

                    return _studentsImportService.SaveStudents(xlsxCsvString, facultyId, studentNameConcatenated);
                }
            }
            else if (fileMimeType == "text/csv")
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var fileContent = reader.ReadToEnd();

                    return _studentsImportService.SaveStudents(fileContent, facultyId, studentNameConcatenated);
                }
            }
            else
            {
                throw new ValidationException("Invalid file format");
            }
        }
    }
}