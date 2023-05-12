using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Contants;
using USVStudDocs.Entities.Authentication;

namespace USVStudDocs.DAL.EntityConfigurations;

public class UserAdminEntityConfig : IEntityTypeConfiguration<UserAdminEntity>
{
    public void Configure(EntityTypeBuilder<UserAdminEntity> entity)
    {
        entity.ToTable(TableName.UserAdmin);
    }
}