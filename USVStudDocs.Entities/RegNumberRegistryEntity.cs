namespace USVStudDocs.Entities;

public class RegNumberRegistryEntity
{
    public int Id { get; set; }
    public string DailyRegistrationNumber { get; set; }
    public int LastCertificateNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}