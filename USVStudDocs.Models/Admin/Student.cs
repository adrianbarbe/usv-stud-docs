using USVStudDocs.Models.Constants;

namespace USVStudDocs.Models.Admin;

public class Student
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Surname { get; set; }
    public string? Patronymic { get; set; }
    public string Name { get; set; }
    
    public Faculty Faculty { get; set; }
    
    public ProgramStudy ProgramStudy { get; set; }
    
    public Semester YearSemester { get; set; }
    
    public FinancialStatus FinancialStatus { get; set; }
}