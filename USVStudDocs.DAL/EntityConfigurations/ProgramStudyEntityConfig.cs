using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Constants;

namespace USVStudDocs.DAL.EntityConfigurations;

public class ProgramStudyEntityConfig : IEntityTypeConfiguration<ProgramStudyEntity>
{
    public void Configure(EntityTypeBuilder<ProgramStudyEntity> entity)
    {
        entity.ToTable(TableName.ProgramStudy);
        
        entity.HasData(
            new ProgramStudyEntity
            {
                Id = 1, 
                Name = "Stiinta si ingineriea calculatoarelor", 
                NameShort = "SIC", 
                FieldOfStudy = FieldOfStudy.Master, 
                FacultyId = 1,
                SecretaryId = 3,
                OrderBy = 1,
            }
        );
    }
}