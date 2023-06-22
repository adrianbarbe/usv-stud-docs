using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;

namespace USVStudDocs.DAL.EntityConfigurations;

public class EmailSentEntityConfig : IEntityTypeConfiguration<EmailSentEntity>
{
    public void Configure(EntityTypeBuilder<EmailSentEntity> entity)
    {
        entity.ToTable(TableName.EmailSent);
    }
}