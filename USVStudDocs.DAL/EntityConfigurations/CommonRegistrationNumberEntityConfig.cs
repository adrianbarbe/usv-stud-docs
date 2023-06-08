using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;

namespace USVStudDocs.DAL.EntityConfigurations;

public class CommonRegistrationNumberEntityConfig : IEntityTypeConfiguration<CommonRegistrationNumberEntity>
{
    public void Configure(EntityTypeBuilder<CommonRegistrationNumberEntity> entity)
    {
        entity.ToTable(TableName.CommonRegistrationNumber);
    }
}