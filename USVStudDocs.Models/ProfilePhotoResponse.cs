using System.Text.Json.Serialization;

namespace USVStudDocs.Models;

public class ProfilePhoto
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public class ProfilePhotoResponse
{
    [JsonPropertyName("photos")]
    public ProfilePhoto[] Photos { get; set; }
}