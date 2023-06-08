namespace USVStudDocs.Entities;

public class FacultyEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string NameShort { get; set; }
    public int OrderBy { get; set; }
    
    public ICollection<ProgramStudyEntity> ProgramStudies { get; set; }

    public FacultyPersonEntity Dean { get; set; }
    public int DeanId { get; set; }
    
    public FacultyPersonEntity SecretaryHead { get; set; }
    public int SecretaryHeadId { get; set; }
}