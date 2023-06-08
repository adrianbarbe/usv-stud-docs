using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using USVStudDocs.DAL.Constants;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Constants;

namespace USVStudDocs.DAL.EntityConfigurations;

public class FacultyPersonEntityConfig : IEntityTypeConfiguration<FacultyPersonEntity>
{
    public void Configure(EntityTypeBuilder<FacultyPersonEntity> entity)
    {
        entity.ToTable(TableName.FacultyPerson);
        
        entity.HasData(
            new FacultyPersonEntity { Id = 1, Name = "Dan", Surname = "Milici", Patronymic =  "", PersonType = FacultyPersonType.Dean},
            new FacultyPersonEntity { Id = 2, Name = "Elena", Surname = "Curelaru", Patronymic =  "", PersonType = FacultyPersonType.SecretaryPrincipal},
            new FacultyPersonEntity { Id = 3, Name = "Laura", Surname = "Dospinescu", Patronymic =  "", PersonType = FacultyPersonType.Secretary}
        );
    }
}