using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Authentication;

namespace USVStudDocs.DAL.EntityConfigurations;

public class UserEntityConfig : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> entity)
    {
        entity
            .HasOne<StudentEntity>(a => a.Student)
            .WithOne(b => b.User)
            .HasForeignKey<StudentEntity>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        entity.ToTable(TableName.User);
    }
}