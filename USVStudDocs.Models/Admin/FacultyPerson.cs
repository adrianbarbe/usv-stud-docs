using USVStudDocs.Models.Constants;

namespace USVStudDocs.Models.Admin;

public class FacultyPerson
{
    public int Id { get; set; }
    public string? Prefix { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    
    public FacultyPersonType PersonType { get; set; }

    public User User { get; set; }
}