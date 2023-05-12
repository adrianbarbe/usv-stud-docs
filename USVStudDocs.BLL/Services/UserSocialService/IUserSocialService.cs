using USVStudDocs.Models;

namespace USVStudDocs.BLL.Services.UserSocialService;

public interface IUserSocialService
{
    List<UserSocial> GetAll();
    
    
    UserSocial Create(UserSocial userSocial);

}