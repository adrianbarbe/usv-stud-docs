using USVStudDocs.BLL.Mappers;
using USVStudDocs.DAL;
using USVStudDocs.Entities.Authentication;
using USVStudDocs.Models;

namespace USVStudDocs.BLL.Services.UserSocialService;

public class UserSocialService : IUserSocialService
{
    private readonly MainContext _context;
    private readonly IMapper<UserSocialEntity, UserSocial> _mapper;

    public UserSocialService(MainContext context, IMapper<UserSocialEntity, UserSocial> mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public List<UserSocial> GetAll()
    {
        return _context.UserSocial.Select(u => _mapper.Map(u)).ToList();
    }

    public UserSocial Create(UserSocial userSocial)
    {
        var userSocialEntity = _mapper.Map(userSocial);
        userSocialEntity.ProviderUserId = "manual";
        
        _context.UserSocial.Add(userSocialEntity);
        _context.SaveChanges();

        return userSocial;
    }
}