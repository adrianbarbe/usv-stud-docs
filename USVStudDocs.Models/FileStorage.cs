using System;

namespace USVStudDocs.Models;

public class FileStorage
{
    public FileStorage()
    {
        CreatedAt = DateTime.UtcNow;
    }
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public decimal FileSize { get; set; }
    
    public string UserSocial { get; set; }
    public int UserSocialId { get; set; }
    
    public DateTime CreatedAt { get; set; }
}