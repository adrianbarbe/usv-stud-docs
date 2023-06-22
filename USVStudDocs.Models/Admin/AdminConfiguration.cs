namespace USVStudDocs.Models.Admin;

public class AdminConfiguration
{
    public DateTime? EducationYearStart { get; set; }
    public List<string> CertificateReasons { get; set; }
    
    public String oAuthEmailSenderEmail { get; set; }
}