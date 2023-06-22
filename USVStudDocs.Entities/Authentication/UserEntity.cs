using System.Collections.Generic;
using USVStudDocs.Entities.Constants;
using USVStudDocs.Entities.Storage;

namespace USVStudDocs.Entities.Authentication;

public class UserEntity : BaseSoftDelete
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string? UserPicture { get; set; }
    public string? ProviderUserId { get; set; }
    
    
    public RoleEntity Role { get; set; }
    public int RoleId { get; set; }
    
    public OAuthProviderTypes OAuthProvider { get; set; }
    
    public virtual ICollection<FileEntity> Files { get; set; }
    
    public StudentEntity Student { get; set; }
    public FacultyPersonEntity Person { get; set; }
}