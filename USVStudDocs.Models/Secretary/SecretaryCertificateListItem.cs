using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;

namespace USVStudDocs.Models.Secretary;

public class SecretaryCertificateListItem
{
    public int Id { get; set; }

    public Admin.Student Student { get; set; }
    
    public DateTime? RegistrationDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    
    public string? RegistrationNumber { get; set; }

    public CertificateStatus CertificateStatus { get; set; }
    
    public string CertificateReason { get; set; }
    
    public string? DenyReason { get; set; }
    
    public FacultyPerson Secretary { get; set; }
}