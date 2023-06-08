using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Authentication;

namespace USVStudDocs.DAL.EntityConfigurations;

public class RegNumberRegistryEntityConfig : IEntityTypeConfiguration<RegNumberRegistryEntity>
{
    public void Configure(EntityTypeBuilder<RegNumberRegistryEntity> entity)
    {
        entity.ToTable(TableName.RegNumberRegistry);

        entity.Property(x => x.DailyRegistrationNumber)
            .IsRequired();
        
        entity.Property(x => x.LastCertificateNumber)
            .IsRequired();
            
        entity.Property(x => x.CreatedAt)
            .HasColumnType("date")
            .IsRequired();
    }
}