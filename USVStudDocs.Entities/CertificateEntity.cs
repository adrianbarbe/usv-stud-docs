using USVStudDocs.Entities.Constants;

namespace USVStudDocs.Entities;

public class CertificateEntity : BaseSoftDelete
{
    public int Id { get; set; }
    public string? RegistrationNumber { get; set; }
    
    public DateTime? RegistrationDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    
    public StudentEntity Student { get; set; }
    public int StudentId { get; set; }
    
    public string CertificateReason { get; set; }

    public CertificateStatus Status { get; set; }

    public string? DenyReason { get; set; }

    public FacultyPersonEntity Secretary { get; set; }
    public int SecretaryId { get; set; }
}