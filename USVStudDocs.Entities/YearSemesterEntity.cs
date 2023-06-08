using USVStudDocs.Entities.Constants;

namespace USVStudDocs.Entities;

public class YearSemesterEntity
{
    public int Id { get; set; }
    public int YearNumber { get; set; }

    public FieldOfStudy FieldOfStudy { get; set; }

    public ICollection<YearProgramStudyEntity> YearProgramStudy { get; set; }
}