namespace USVStudDocs.Entities;

public class YearProgramStudyEntity
{
    public YearSemesterEntity YearSemester { get; set; }
    public int YearSemesterId { get; set; }

    public ProgramStudyEntity ProgramStudy { get; set; }
    public int ProgramStudyId { get; set; }
}