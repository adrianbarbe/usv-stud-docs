using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Authentication;
using USVStudDocs.Entities.Constants;
using USVStudDocs.Entities.Resources;

namespace USVStudDocs.DAL.EntityConfigurations;

public class SettingsEntityConfig : IEntityTypeConfiguration<SettingsEntity>
{
    public void Configure(EntityTypeBuilder<SettingsEntity> entity)
    {
        entity.ToTable(TableName.Settings);
        
        entity.HasData(
            new SettingsEntity { Id = 1, Key = Roles.Admin, Value = SettingsKeys.SemesterSettings.oAuthEmailSenderEmail.Key, Type = SettingsType.String}
        );
    }
}