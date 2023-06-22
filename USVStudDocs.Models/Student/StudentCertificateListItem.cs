using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;

namespace USVStudDocs.Models.Student;

public class StudentCertificateListItem
{
    public int Id { get; set; }
    
    public DateTime? RegistrationDate { get; set; }
    
    public string? RegistrationNumber { get; set; }

    public CertificateStatus CertificateStatus { get; set; }
    
    public string CertificateReason { get; set; }
    
    public string? DenyReason { get; set; }
    
    public FacultyPerson Secretary { get; set; }
}