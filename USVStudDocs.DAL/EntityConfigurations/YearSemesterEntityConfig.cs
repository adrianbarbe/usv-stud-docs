using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Constants;

namespace USVStudDocs.DAL.EntityConfigurations;

public class YearSemesterEntityConfig : IEntityTypeConfiguration<YearSemesterEntity>
{
    public void Configure(EntityTypeBuilder<YearSemesterEntity> entity)
    {
        entity.ToTable(TableName.YearSemester);
        
        entity.HasData(
            new YearSemesterEntity { Id = 1, YearNumber = 1, FieldOfStudy = FieldOfStudy.Bachelor},
            new YearSemesterEntity { Id = 2, YearNumber = 2, FieldOfStudy = FieldOfStudy.Bachelor},
            new YearSemesterEntity { Id = 3, YearNumber = 3, FieldOfStudy = FieldOfStudy.Bachelor},
            new YearSemesterEntity { Id = 4, YearNumber = 4, FieldOfStudy = FieldOfStudy.Bachelor},
            new YearSemesterEntity { Id = 5, YearNumber = 1, FieldOfStudy = FieldOfStudy.Master},
            new YearSemesterEntity { Id = 6, YearNumber = 2, FieldOfStudy = FieldOfStudy.Master},
            new YearSemesterEntity { Id = 7, YearNumber = 1, FieldOfStudy = FieldOfStudy.ProfessionalConversion},
            new YearSemesterEntity { Id = 8, YearNumber = 2, FieldOfStudy = FieldOfStudy.ProfessionalConversion}
        );
    }
}