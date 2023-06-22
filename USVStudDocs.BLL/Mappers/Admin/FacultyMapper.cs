using USVStudDocs.Entities;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Mappers.Admin
{
    public class FacultyMapper : IMapper<FacultyEntity, Faculty>
    {
        public FacultyEntity Map(Faculty source)
        {
            return new FacultyEntity
            {
                Id = source.Id,
                Name = source.Name.Trim(),
                NameShort = source.NameShort.Trim(),
                OrderBy = source.OrderBy ?? 0,
                DeanId = source.Dean.Id,
                SecretaryHeadId = source.SecretaryHead.Id
            };
        }

        public Faculty Map(FacultyEntity source)
        {
            return new Faculty
            {
                Id = source.Id,
                Name = source.Name,
                NameShort = source.NameShort,
                OrderBy = source.OrderBy,
                Dean = source.Dean != null ? new FacultyPerson
                {
                    Id = source.Dean.Id,
                    Surname = source.Dean.Surname,
                    Name = source.Dean.Name,
                    Patronymic = source.Dean.Patronymic,
                } : null,
                SecretaryHead = source.SecretaryHead != null ? new FacultyPerson
                {
                    Id = source.SecretaryHead.Id,
                    Surname = source.SecretaryHead.Surname,
                    Name = source.SecretaryHead.Name,
                    Patronymic = source.SecretaryHead.Patronymic,
                } : null,
            };
        }
    }
}