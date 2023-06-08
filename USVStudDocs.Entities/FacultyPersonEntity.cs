using USVStudDocs.Entities.Authentication;
using USVStudDocs.Entities.Constants;

namespace USVStudDocs.Entities;

public class FacultyPersonEntity : BaseSoftDelete
{
    public int Id { get; set; }
    public string? Prefix { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    
    public FacultyPersonType PersonType { get; set; }

    public UserEntity User { get; set; }
    public int? UserId { get; set; }
    
    public ICollection<FacultyEntity> FacultiesDean { get; set; }
    public ICollection<FacultyEntity> FacultiesSecretaryHead { get; set; }
    
    public ICollection<ProgramStudyEntity> ProgramStudies { get; set; }
}