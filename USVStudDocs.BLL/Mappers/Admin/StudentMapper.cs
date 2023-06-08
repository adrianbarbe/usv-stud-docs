using USVStudDocs.Entities;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;

namespace USVStudDocs.BLL.Mappers.Admin
{
    public class StudentMapper : IMapper<StudentEntity, Models.Admin.Student>
    {
        private readonly IMapper<YearSemesterEntity, Semester> _semesterMapper;

        public StudentMapper(IMapper<YearSemesterEntity, Semester> semesterMapper)
        {
            _semesterMapper = semesterMapper;
        }
        public StudentEntity Map(Models.Admin.Student source)
        {
            return new StudentEntity
            {
                Id = source.Id,
                Surname = source.Surname.Trim(),
                Name = source.Name.Trim(),
                Patronymic = source.Patronymic.Trim(),
                Email = source.Email.Trim(),
                FacultyId = source.Faculty.Id,
                YearSemesterId = source.YearSemester.Id,
                ProgramOfStudyId = source.ProgramStudy.Id,
            };
        }

        public Models.Admin.Student Map(StudentEntity source)
        {
            FinancialStatus financialStatus = FinancialStatus.Budget;

            switch (source.FinancialStatus)
            {
                case Entities.Constants.FinancialStatus.Budget:
                    financialStatus = FinancialStatus.Budget;
                    break;
                case Entities.Constants.FinancialStatus.TuitionFee:
                    financialStatus = FinancialStatus.TuitionFee;
                    break;
            }
            
            return new Models.Admin.Student
            {
                Id = source.Id,
                Surname = source.Surname,
                Name = source.Name,
                Patronymic = source.Patronymic,
                Email = source.Email,
                FinancialStatus = financialStatus,
                ProgramStudy = source.ProgramOfStudy != null ? new ProgramStudy
                {
                    Id = source.ProgramOfStudy.Id,
                    Name = source.ProgramOfStudy.Name,
                    NameShort = source.ProgramOfStudy.NameShort,
                } : null,
                YearSemester = source.YearSemester != null ? _semesterMapper.Map(source.YearSemester) : null,
                Faculty = source.Faculty != null ? new Faculty
                {
                    Id = source.Faculty.Id,
                    Name = source.Faculty.Name,
                    NameShort = source.Faculty.NameShort,
                } : null,
            };
        }
    }
}