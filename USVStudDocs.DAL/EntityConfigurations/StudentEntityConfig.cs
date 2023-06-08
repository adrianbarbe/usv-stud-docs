using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;

namespace USVStudDocs.DAL.EntityConfigurations;

public class StudentEntityConfig : IEntityTypeConfiguration<StudentEntity>
{
    public void Configure(EntityTypeBuilder<StudentEntity> entity)
    {
        entity.HasIndex(s => s.Email);
        
        entity.ToTable(TableName.Student);
    }
}