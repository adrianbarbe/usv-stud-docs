using USVStudDocs.Entities.Constants;

namespace USVStudDocs.Entities;

public class ProgramStudyEntity
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public string NameShort { get; set; }

    public int OrderBy { get; set; }

    public FieldOfStudy FieldOfStudy { get; set; }
    
    public ICollection<YearProgramStudyEntity> YearProgramStudy { get; set; }

    public FacultyEntity Faculty { get; set; }
    public int FacultyId { get; set; }
    
    public FacultyPersonEntity Secretary { get; set; }
    public int SecretaryId { get; set; }
}