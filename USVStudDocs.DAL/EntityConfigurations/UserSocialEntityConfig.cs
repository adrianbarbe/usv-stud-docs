using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Contants;
using USVStudDocs.Entities.Authentication;

namespace USVStudDocs.DAL.EntityConfigurations;

public class UserSocialEntityConfig : IEntityTypeConfiguration<UserSocialEntity>
{
    public void Configure(EntityTypeBuilder<UserSocialEntity> entity)
    {
        entity.ToTable(TableName.UserSocial);
    }
}