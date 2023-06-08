using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Authentication;

namespace USVStudDocs.DAL.EntityConfigurations;

public class SettingsEntityConfig : IEntityTypeConfiguration<SettingsEntity>
{
    public void Configure(EntityTypeBuilder<SettingsEntity> entity)
    {
        entity.ToTable(TableName.Settings);
    }
}