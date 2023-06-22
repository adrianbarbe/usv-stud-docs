using USVStudDocs.Entities.Authentication;
using USVStudDocs.Models;

namespace USVStudDocs.BLL.Mappers.Authentication;

public class UserSocialMapper : IMapper<UserEntity, UserSocial>
{
    public UserEntity Map(UserSocial source)
    {
        return new UserEntity
        {
            Id = source.Id,
            Email = source.Email,
            Username = source.Username,
            FirstName = source.FirstName,
            LastName = source.LastName,
            ProviderUserId = source.ProviderUserId,
            UserPicture = source.UserPicture,
        };
    }

    public UserSocial Map(UserEntity source)
    {
        return new UserSocial
        {
            Id = source.Id,
            Email = source.Email,
            Username = source.Username,
            FirstName = source.FirstName,
            LastName = source.LastName,
            ProviderUserId = source.ProviderUserId,
            UserPicture = source.UserPicture,
        };
    }
}