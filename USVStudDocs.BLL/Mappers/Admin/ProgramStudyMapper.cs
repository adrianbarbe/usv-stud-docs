using USVStudDocs.Entities;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Mappers.Admin
{
    public class ProgramStudyMapper : IMapper<ProgramStudyEntity, ProgramStudy>
    {
        public ProgramStudyEntity Map(ProgramStudy source)
        {
            return new ProgramStudyEntity
            {
                Id = source.Id,
                Name = source.Name.Trim(),
                NameShort = source.NameShort.Trim(),
                OrderBy = source.OrderBy ?? 0,
                FacultyId = source.Faculty.Id,
                SecretaryId = source.Secretary.Id,
            };
        }

        public ProgramStudy Map(ProgramStudyEntity source)
        {
            var speciality = new ProgramStudy
            {
                Id = source.Id,
                Name = source.Name,
                NameShort = source.NameShort,
                OrderBy = source.OrderBy,
                Faculty = new Faculty
                {
                    Id = source.Faculty.Id,
                    Name = source.Name,
                    OrderBy = source.OrderBy
                },
                Secretary = new FacultyPerson
                {
                    Id = source.Secretary.Id,
                    Name = source.Secretary.Name,
                    Surname = source.Secretary.Surname,
                }
            };

            if (source.YearProgramStudy != null)
            {
                speciality.YearSemesters = source.YearProgramStudy?.Select(s => new YearSemester
                {
                    Id = s.YearSemester.Id,
                    YearNumber = s.YearSemester.YearNumber,
                }).ToList();
            }
            
            return speciality;
        }
    }
}