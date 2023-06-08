using TinyCsvParser;
using TinyCsvParser.Mapping;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Authentication;
using USVStudDocs.Entities.Constants;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Services.StudentsImportService;

public class StudentsImportService : IStudentsImportService
{
    private readonly MainContext _context;

    public StudentsImportService(MainContext context)
    {
        _context = context;
    }

    public List<Student> SaveStudents(string csvContent, int facultyId, bool studentNameConcatenated)
    {
        var roleStudent = _context.Role.FirstOrDefault(r => r.Name == Roles.Student);

        if (roleStudent == null)
        {
            throw new ValidationException("Role not found");
        }

        var allStudents = _context.Student.ToList();

        var faculty =
            _context.Faculty.FirstOrDefault(f => f.Id == facultyId);

        if (faculty == null)
        {
            throw new ValidationException("Faculty not found");
        }

        CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');

        if (studentNameConcatenated)
        {
            StudentsConcatenatedImportCsvMapping csvMapper = new StudentsConcatenatedImportCsvMapping();
            CsvParser<StudentImportConcatenated> csvParser =
                new CsvParser<StudentImportConcatenated>(csvParserOptions, csvMapper);

            var resultConcatenated = csvParser
                .ReadFromString(new CsvReaderOptions(new[] { Environment.NewLine }), csvContent)
                .ToList();

            return ParseCsvStudentsConcatenated(resultConcatenated, allStudents, roleStudent, faculty);
        }
        else
        {
            StudentsImportCsvMapping csvMapper = new StudentsImportCsvMapping();
            CsvParser<StudentImport> csvParser = new CsvParser<StudentImport>(csvParserOptions, csvMapper);

            var result = csvParser
                .ReadFromString(new CsvReaderOptions(new[] { Environment.NewLine }), csvContent)
                .ToList();

            return ParseCsvStudents(result, allStudents, roleStudent, faculty);
        }
    }

    private List<Student> ParseCsvStudentsConcatenated(List<CsvMappingResult<StudentImportConcatenated>> result,
        List<StudentEntity> allUsers, RoleEntity roleStudent, FacultyEntity faculty)
    {
        var studentErrors = new List<Student>();
        var studentsSkipped = 0;

        var studentEntitiesAll = new List<StudentEntity>();

        foreach (var res in result)
        {
            if (res.IsValid)
            {
                var foundUser = allUsers.FirstOrDefault(u => u.Email == res.Result.Email);

                if (foundUser == null)
                {
                    var programStudy =
                        _context.ProgramStudy.FirstOrDefault(s =>
                            res.Result.ProgramStudy.Trim().ToLower().Equals(s.NameShort.Trim().ToLower())
                        );

                    if (programStudy == null)
                    {
                        throw new ValidationException("ProgramStudy not found");
                    }

                    var fieldOfStudy = ParseFieldOfStudy(res.Result.FieldOfStudy);

                    var financialStatus = ParseFinancialStatus(res.Result.FinancialStatus);

                    var yearSemester = _context.YearSemester
                        .Where(a => a.YearNumber == res.Result.Year && a.FieldOfStudy == fieldOfStudy)
                        .OrderBy(a => a.YearNumber).FirstOrDefault();

                    if (yearSemester == null)
                    {
                        throw new ValidationException("yearSemester not found");
                    }

                    var studentSurnameNamePatronymic = res.Result.SurnameNamePatronymic.Trim()
                        .Replace("`", "'").Replace("'", "'")
                        .Replace("’", "'").Replace("  ", " ")
                        .Split(" ");

                    List<string> surname = new List<string>();
                    List<string> name = new List<string>();
                    List<string> patronymic = new List<string>();

                    foreach (var studentString in studentSurnameNamePatronymic)
                    {
                        if (!studentString.Contains('.') && patronymic.Count == 0)
                        {
                            surname.Add(studentString);
                        }
                        else if (studentString.Contains('.') && patronymic.Count == 0)
                        {
                            patronymic.Add(studentString);
                        }
                        else if (!studentString.Contains('.') && patronymic.Count > 0)
                        {
                            name.Add(studentString);
                        }
                    }

                    var studentEntity = new StudentEntity
                    {
                        Surname = string.Join(" ", surname),
                        Name = string.Join(" ", name),
                        Patronymic = string.Join(" ", patronymic),
                        Email = res.Result.Email.Trim(),
                        FacultyId = faculty.Id,
                        FieldOfStudy = fieldOfStudy,
                        YearSemesterId = yearSemester.Id,
                        ProgramOfStudyId = programStudy.Id,
                        FinancialStatus = financialStatus,
                        User = new UserEntity
                        {
                            Email = res.Result.Email.Trim(),
                            Username = res.Result.Email.Trim(),
                            RoleId = roleStudent.Id,
                        },
                    };

                    studentEntitiesAll.Add(studentEntity);
                }
                else
                {
                    studentsSkipped++;
                }
            }
            else
            {
                studentErrors.Add(new Student
                {
                    Surname = res.Error.Value,
                    Name = res.Error.UnmappedRow,
                });
            }
        }

        _context.Student.UpdateRange(studentEntitiesAll);
        _context.SaveChanges();

        Console.Write($"studentsSkipped: {studentsSkipped}");

        return studentErrors;
    }

    private List<Student> ParseCsvStudents(List<CsvMappingResult<StudentImport>> result, List<StudentEntity> allUsers,
        RoleEntity roleStudent, FacultyEntity faculty)
    {
        var studentErrors = new List<Student>();
        var studentsSkipped = 0;

        var studentEntitiesAll = new List<StudentEntity>();

        foreach (var res in result)
        {
            if (res.IsValid)
            {
                var foundUser = allUsers.FirstOrDefault(u => u.Email == res.Result.Email);

                if (foundUser == null)
                {
                    var programStudy =
                        _context.ProgramStudy.FirstOrDefault(s =>
                            res.Result.ProgramStudy.Trim().ToLower().Equals(s.NameShort.Trim().ToLower())
                        );

                    if (programStudy == null)
                    {
                        throw new ValidationException("ProgramStudy not found");
                    }

                    var fieldOfStudy = ParseFieldOfStudy(res.Result.FieldOfStudy);

                    var financialStatus = ParseFinancialStatus(res.Result.FinancialStatus);

                    var yearSemester = _context.YearSemester
                        .Where(a => a.YearNumber == res.Result.Year && a.FieldOfStudy == fieldOfStudy)
                        .OrderBy(a => a.YearNumber).FirstOrDefault();

                    if (yearSemester == null)
                    {
                        throw new ValidationException("yearSemester not found");
                    }

                    var studentEntity = new StudentEntity
                    {
                        Surname = res.Result.Surname,
                        Name = res.Result.Name,
                        Patronymic = res.Result.Patronymic,
                        Email = res.Result.Email.Trim(),
                        FacultyId = faculty.Id,
                        FieldOfStudy = fieldOfStudy,
                        YearSemesterId = yearSemester.Id,
                        ProgramOfStudyId = programStudy.Id,
                        FinancialStatus = financialStatus,
                        User = new UserEntity
                        {
                            Email = res.Result.Email.Trim(),
                            Username = res.Result.Email.Trim(),
                            RoleId = roleStudent.Id,
                        },
                    };

                    studentEntitiesAll.Add(studentEntity);
                }
                else
                {
                    studentsSkipped++;
                }
            }
            else
            {
                studentErrors.Add(new Student
                {
                    Surname = res.Error.Value,
                    Name = res.Error.UnmappedRow,
                });
            }
        }

        _context.Student.UpdateRange(studentEntitiesAll);
        _context.SaveChanges();

        Console.Write($"studentsSkipped: {studentsSkipped}");

        return studentErrors;
    }

    private FieldOfStudy ParseFieldOfStudy(string result)
    {
        FieldOfStudy fieldOfStudy = FieldOfStudy.Bachelor;

        if (result.Trim().ToLower() == "licență" ||
            result.Trim().ToLower() == "licenta")
        {
            fieldOfStudy = FieldOfStudy.Bachelor;
        }
        else if (result.Trim().ToLower() == "masterat")
        {
            fieldOfStudy = FieldOfStudy.Master;
        }
        else if (result.Trim().ToLower() == "conversie profesională" ||
                 result.Trim().ToLower() == "conversie profesionala")
        {
            fieldOfStudy = FieldOfStudy.ProfessionalConversion;
        }

        return fieldOfStudy;
    }

    private FinancialStatus ParseFinancialStatus(string result)
    {
        FinancialStatus financialStatus = FinancialStatus.Budget;

        if (result.Trim().ToLower() == "buget")
        {
            financialStatus = FinancialStatus.Budget;
        }
        else if (result.Trim().ToLower() == "cu taxă" ||
                 result.Trim().ToLower() == "taxă" ||
                 result.Trim().ToLower() == "cu taxa" ||
                 result.Trim().ToLower() == "taxa")
        {
            financialStatus = FinancialStatus.TuitionFee;
        }

        return financialStatus;
    }
}