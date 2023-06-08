using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Authentication;

namespace USVStudDocs.DAL.EntityConfigurations;

public class CertificateEntityConfig : IEntityTypeConfiguration<CertificateEntity>
{
    public void Configure(EntityTypeBuilder<CertificateEntity> entity)
    {
        entity.ToTable(TableName.Certificate);
    }
}