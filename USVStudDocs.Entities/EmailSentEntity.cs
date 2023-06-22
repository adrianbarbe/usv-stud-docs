namespace USVStudDocs.Entities;

public class EmailSentEntity
{
    public int Id { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTime SentAt { get; set; }
    public string Status { get; set; }
}