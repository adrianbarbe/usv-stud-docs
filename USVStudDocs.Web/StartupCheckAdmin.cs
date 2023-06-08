using Microsoft.EntityFrameworkCore;
using USVStudDocs.DAL;
using USVStudDocs.Entities.Authentication;
using USVStudDocs.Entities.Constants;

namespace USVStudDocs.Web;

public class StartupCheckAdmin : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    public StartupCheckAdmin(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MainContext>();
        
        var userEntity = dbContext.User
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Role.Name == Roles.Admin);

        if (userEntity == null)
        {
            var adminRoleEntity = dbContext.Role.FirstOrDefault(r => r.Name == Roles.Admin);

            if (adminRoleEntity == null)
            {
                throw new Exception("Cannot find admin role in the DB");
            }

            var systemAdminEmailEnv = Environment.GetEnvironmentVariable("SYSTEM_ADMIN_EMAIL");

            if (string.IsNullOrEmpty(systemAdminEmailEnv))
            {
                throw new Exception("Please set the SYSTEM_ADMIN_EMAIL environment variable");
            }

            var adminUserEntity = new UserEntity
            {
                Email = systemAdminEmailEnv.Trim(),
                Username = systemAdminEmailEnv.Trim(),
                FirstName = "",
                LastName = "",
                RoleId = adminRoleEntity.Id,
                OAuthProvider = OAuthProviderTypes.Google,
            };

            dbContext.User.Add(adminUserEntity);
            dbContext.SaveChanges();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}