namespace USVStudDocs.Entities.Constants
{
    public enum SettingsType
    {
        String = 0,
        DateTime = 1,
        Int = 2,
        ArrayInts = 3,
        Boolean = 4,
        Float = 5,
        ArrayStrings = 6,
    }

    public static class SettingsTypeExtensions
    {
        public static string GetValueType(this SettingsType st)
        {
            switch (st)
            {
                case SettingsType.Int:
                    return "int";

                case SettingsType.String:
                    return "string";

                case SettingsType.DateTime:
                    return "datetime";
                
                case SettingsType.Boolean:
                    return "boolean";
            }

            return "";
        }
    }
}