using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Contants;
using USVStudDocs.Entities.Storage;

namespace USVStudDocs.DAL.EntityConfigurations;

public class FileEntityConfig : IEntityTypeConfiguration<FileEntity>
{
    public void Configure(EntityTypeBuilder<FileEntity> entity)
    {
        entity.ToTable(TableName.File);
    }
}