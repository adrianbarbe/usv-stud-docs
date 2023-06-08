using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;

namespace USVStudDocs.DAL.EntityConfigurations;

public class YearProgramStudyEntityConfig : IEntityTypeConfiguration<YearProgramStudyEntity>
{
    public void Configure(EntityTypeBuilder<YearProgramStudyEntity> entity)
    {
        entity.ToTable(TableName.YearProgramStudy);
        
        entity.HasKey(k => new {k.ProgramStudyId, k.YearSemesterId});

        entity.HasIndex(a => a.ProgramStudyId);
        entity.HasIndex(a => a.YearSemesterId);
        
        entity.HasData(
            new YearProgramStudyEntity { ProgramStudyId = 1, YearSemesterId = 5 },
            new YearProgramStudyEntity { ProgramStudyId = 1, YearSemesterId = 6 }
        );
    }
}