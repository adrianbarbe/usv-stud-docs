using USVStudDocs.Models.Admin;

namespace USVStudDocs.Models.Secretary;

public class SecretaryCertificatePrint
{
    public SecretaryCertificateListItem CertificateItem { get; set; }
    
    public int StudyYearStart { get; set; }

    public FacultyPerson Dean { get; set; }
    public FacultyPerson SecretaryHead { get; set; }
    public FacultyPerson Secretary { get; set; }
}