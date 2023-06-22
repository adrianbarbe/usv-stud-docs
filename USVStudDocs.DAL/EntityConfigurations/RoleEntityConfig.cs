using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities.Authentication;
using USVStudDocs.Entities.Constants;

namespace USVStudDocs.DAL.EntityConfigurations;

public class RoleEntityConfig : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> entity)
    {
        entity.ToTable(TableName.Role);

        entity.HasData(
            new RoleEntity { Id = 1, Name = Roles.Admin, Description = "Admin role" },
            new RoleEntity { Id = 2, Name = Roles.Secretary, Description = "Secretary role" },
            new RoleEntity { Id = 3, Name = Roles.Student, Description = "Student role" },
            new RoleEntity { Id = 4, Name = Roles.Analytic, Description = "Analytic role" }
        );
    }
}