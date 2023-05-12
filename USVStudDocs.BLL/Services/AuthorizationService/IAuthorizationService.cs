namespace USVStudDocs.BLL.Services.AuthorizationService
{
    public interface IAuthorizationService
    {
        int GetCurrentUserId();

        string GetUserName();

        string[] GetRoles();
    }
}