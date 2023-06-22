using USVStudDocs.Entities.Constants;

namespace USVStudDocs.Entities
{
    public class SettingsEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public SettingsType Type { get; set; }
    }
}