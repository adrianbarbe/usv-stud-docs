using USVStudDocs.Entities.Authentication;
using USVStudDocs.Entities.Constants;

namespace USVStudDocs.Entities;

public class StudentEntity : BaseSoftDelete
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Surname { get; set; }
    public string? Patronymic { get; set; }
    public string Name { get; set; }
    
    public FieldOfStudy FieldOfStudy { get; set; } // Domeniu de studii
    
    public ProgramStudyEntity ProgramOfStudy { get; set; }
    public int ProgramOfStudyId { get; set; }
    
    public FacultyEntity Faculty { get; set; }
    public int FacultyId { get; set; }
    
    public YearSemesterEntity YearSemester { get; set; }
    public int YearSemesterId { get; set; }
    
    public FinancialStatus FinancialStatus { get; set; }

    public UserEntity User { get; set; }
    public int UserId { get; set; }
}