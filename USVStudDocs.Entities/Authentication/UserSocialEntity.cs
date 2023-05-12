using USVStudDocs.Entities.Constants;
using USVStudDocs.Entities.Storage;

namespace USVStudDocs.Entities.Authentication;

public class UserSocialEntity
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string UserPicture { get; set; }
    public string ProviderUserId { get; set; }
    
    public OAuthProviderTypes OAuthProvider { get; set; }
    
    public virtual ICollection<FileEntity> Files { get; set; }
}