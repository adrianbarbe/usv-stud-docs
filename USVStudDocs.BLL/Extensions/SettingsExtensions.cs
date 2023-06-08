using System.Globalization;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Constants;

namespace USVStudDocs.BLL.Extensions
{
    public static class SettingsExtensions
    {
        public static int ParseToInt(this SettingsEntity entity)
        {
            if (entity.Type != SettingsType.Int)
            {
                throw new ValidationException("Error while parsing settings. Cannot parse non-int to int");
            }
            
            return Convert.ToInt32(entity.Value);
        }
        
        public static float ParseToFloat(this SettingsEntity entity)
        {
            if (entity.Type != SettingsType.Float)
            {
                throw new ValidationException("Error while parsing settings. Cannot parse non-float to float");
            }
            
            return float.Parse(entity.Value, CultureInfo.InvariantCulture.NumberFormat);
        }
        
        public static DateTime ParseToDateTime(this SettingsEntity entity)
        {
            if (entity.Type != SettingsType.DateTime)
            {
                throw new ValidationException("Error while parsing settings. Cannot parse non-DateTime to DateTime");
            }
            
            return DateTime.ParseExact(entity.Value, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
        }
        
        public static string ParseToString(this SettingsEntity entity)
        {
            if (entity.Type != SettingsType.String)
            {
                throw new ValidationException("Error while parsing settings. Cannot parse non-string to string");
            }
            
            return Convert.ToString(entity.Value);
        }
        
        public static int[] ParseToArrayInts(this SettingsEntity entity)
        {
            if (entity.Type != SettingsType.ArrayInts)
            {
                throw new ValidationException("Error while parsing settings. Cannot parse string to array of ints");
            }

            if (string.IsNullOrEmpty(entity.Value))
            {
                return new int[]{};
            }
            
            var arrayOfStrings = entity.Value.Split(',');

            if (arrayOfStrings.Length == 0)
            {
                return new int[]{};                
            }

            return Array.ConvertAll<string, int>(arrayOfStrings, int.Parse);
        }
        
        public static string[] ParseToArrayStrings(this SettingsEntity entity)
        {
            if (entity.Type != SettingsType.ArrayStrings)
            {
                throw new ValidationException("Error while parsing settings. Cannot parse string to array of strings");
            }

            if (string.IsNullOrEmpty(entity.Value))
            {
                return new string[]{};
            }
            
            var arrayOfStrings = entity.Value.Split(',');

            if (arrayOfStrings.Length == 0)
            {
                return new string[]{};                
            }

            return arrayOfStrings;
        }
        
        public static bool ParseToBool(this SettingsEntity entity)
        {
            if (entity.Type != SettingsType.Boolean)
            {
                throw new ValidationException("Error while parsing settings. Cannot parse non-bool string to bool");
            }
            
            return entity.Value.Trim().ToLower() == "true";
        }
    }
}