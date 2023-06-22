namespace USVStudDocs.Entities;

public class CommonRegistrationNumberEntity
{
    public int Id { get; set; }
    
    public string RegistrationNumber { get; set; }
    
    public int? OrderNumber { get; set; }

    public DateTime Date { get; set; }
    
    public FacultyEntity Faculty { get; set; }
    public int FacultyId { get; set; }
}