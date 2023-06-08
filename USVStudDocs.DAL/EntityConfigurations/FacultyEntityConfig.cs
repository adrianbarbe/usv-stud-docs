using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;

namespace USVStudDocs.DAL.EntityConfigurations;

public class FacultyEntityConfig : IEntityTypeConfiguration<FacultyEntity>
{
    public void Configure(EntityTypeBuilder<FacultyEntity> entity)
    {
        entity.ToTable(TableName.Faculty);
        
        entity.HasOne(fs => fs.Dean)
            .WithMany(fs => fs.FacultiesDean)
            .HasForeignKey(fs => fs.DeanId)
            .OnDelete(DeleteBehavior.Restrict);;
        
        entity.HasOne(fs => fs.SecretaryHead)
            .WithMany(fs => fs.FacultiesSecretaryHead)
            .HasForeignKey(fs => fs.SecretaryHeadId)
            .OnDelete(DeleteBehavior.Restrict);;

        entity.HasData(
            new FacultyEntity { Id = 1, Name = "Facultatea de inginerie electrica si stiinta calculatoarelor", NameShort = "FIESC", OrderBy = 1, DeanId = 1, SecretaryHeadId = 2}
        );
    }
}