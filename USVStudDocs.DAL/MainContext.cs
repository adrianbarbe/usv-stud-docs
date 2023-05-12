using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using USVStudDocs.DAL.EntityConfigurations;
using USVStudDocs.DAL.Helpers;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Authentication;
using USVStudDocs.Entities.Storage;

namespace USVStudDocs.DAL;

public class MainContext : DbContext
{
    private readonly IAuthorizationDAHelper _authorizationDaHelper;
    public MainContext(DbContextOptions<MainContext> options, IAuthorizationDAHelper authorizationDaHelper) : base(options)
    {
        _authorizationDaHelper = authorizationDaHelper;
    }
    
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    
    public virtual DbSet<UserSocialEntity> UserSocial { get; set; }
    public virtual DbSet<UserAdminEntity> UserAdmin { get; set; }
    public virtual DbSet<FileEntity> File { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseSoftDelete).IsAssignableFrom(entity.ClrType))
            {
                modelBuilder
                    .Entity(entity.ClrType)
                    .HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
            }
        }
        
        modelBuilder.ApplyConfiguration(new UserSocialEntityConfig());
        modelBuilder.ApplyConfiguration(new UserAdminEntityConfig());
        modelBuilder.ApplyConfiguration(new FileEntityConfig());
    }
    
    // Methods for soft delete
    private static readonly MethodInfo PropertyMethod = typeof(EF)
        .GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)
        .MakeGenericMethod(typeof(bool));
    private static LambdaExpression GetIsDeletedRestriction(Type type)
    {
        var parm = Expression.Parameter(type, "it");
        var prop = Expression.Call(PropertyMethod, parm, Expression.Constant(nameof(BaseSoftDelete.IsDeleted)));
        var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));
        var lambda = Expression.Lambda(condition, parm);

        return lambda;
    }
    private void OnBeforeSaving()
    {
        foreach (var entry in ChangeTracker.Entries<BaseSoftDelete>())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.DeletedBy = _authorizationDaHelper.GetUserName();
                    entry.CurrentValues[nameof(BaseSoftDelete.IsDeleted)] = true;
                    entry.State = EntityState.Modified;
                    break;
            }
        }
    }
}