using USVStudDocs.Entities;
using USVStudDocs.Entities.Constants;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Mappers.Admin
{
    public class SemesterMapper : IMapper<YearSemesterEntity, Semester>
    {
        public YearSemesterEntity Map(Semester source)
        {
            FieldOfStudy fieldOfStudy = FieldOfStudy.Bachelor;

            switch (source.FieldOfStudy)
            {
                case Models.Constants.FieldOfStudy.Bachelor:
                    fieldOfStudy = FieldOfStudy.Bachelor;
                    break;
                case Models.Constants.FieldOfStudy.Master:
                    fieldOfStudy = FieldOfStudy.Master;
                    break;
                case Models.Constants.FieldOfStudy.ProfessionalConversion:
                    fieldOfStudy = FieldOfStudy.ProfessionalConversion;
                    break;
            }
            
            return new YearSemesterEntity
            {
                Id = source.Id,
                YearNumber = source.YearNumber,
                FieldOfStudy = fieldOfStudy,
            };
        }

        public Semester Map(YearSemesterEntity source)
        {
            Models.Constants.FieldOfStudy fieldOfStudy = Models.Constants.FieldOfStudy.Bachelor;

            switch (source.FieldOfStudy)
            {
                case FieldOfStudy.Bachelor:
                    fieldOfStudy = Models.Constants.FieldOfStudy.Bachelor;
                    break;
                case FieldOfStudy.Master:
                    fieldOfStudy = Models.Constants.FieldOfStudy.Master;
                    break;
                case FieldOfStudy.ProfessionalConversion:
                    fieldOfStudy = Models.Constants.FieldOfStudy.ProfessionalConversion;
                    break;
            }
            
            return new Semester
            {
                Id = source.Id,
                YearNumber = source.YearNumber,
                FieldOfStudy = fieldOfStudy
            };
        }
    }
}