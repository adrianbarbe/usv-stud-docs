using USVStudDocs.Entities;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;

namespace USVStudDocs.BLL.Mappers.Admin
{
    public class FacultyPersonMapper : IMapper<FacultyPersonEntity, FacultyPerson>
    {
        public FacultyPersonEntity Map(FacultyPerson source)
        {
            Entities.Constants.FacultyPersonType personType = Entities.Constants.FacultyPersonType.SecretaryPrincipal;

            switch (source.PersonType)
            {
                case FacultyPersonType.Dean:
                    personType = Entities.Constants.FacultyPersonType.Dean;
                    break;
                case FacultyPersonType.SecretaryPrincipal:
                    personType = Entities.Constants.FacultyPersonType.SecretaryPrincipal;
                    break;
                case FacultyPersonType.Secretary:
                    personType = Entities.Constants.FacultyPersonType.Secretary;
                    break;
            }
            
            return new FacultyPersonEntity
            {
                Id = source.Id,
                Prefix = source.Prefix.Trim(),
                Name = source.Name.Trim(),
                Surname = source.Surname.Trim(),
                Patronymic = source.Patronymic.Trim(),
                PersonType = personType,
            };
        }

        public FacultyPerson Map(FacultyPersonEntity source)
        {
            FacultyPersonType personType = FacultyPersonType.SecretaryPrincipal;

            switch (source.PersonType)
            {
                case Entities.Constants.FacultyPersonType.Dean:
                    personType = FacultyPersonType.Dean;
                    break;
                case Entities.Constants.FacultyPersonType.SecretaryPrincipal:
                    personType = FacultyPersonType.SecretaryPrincipal;
                    break;
                case Entities.Constants.FacultyPersonType.Secretary:
                    personType = FacultyPersonType.Secretary;
                    break;
            }
            
            return new FacultyPerson
            {
                Id = source.Id,
                Prefix = source.Prefix,
                Name = source.Name,
                Surname = source.Surname,
                Patronymic = source.Patronymic,
                User = source.User != null ? new User
                {
                    Id = source.User.Id,
                    Username = source.User.Username,
                    Email = source.User.Email,
                } : null,
                PersonType = personType,
            };
        }
    }
}