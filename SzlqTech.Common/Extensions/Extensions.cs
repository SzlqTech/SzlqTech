
using NLog;
using System.Net;

namespace SzlqTech.Common.Extensions
{
    public static class Extensions
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static T? GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            object[] customAttributes = enumVal.GetType().GetMember(enumVal.ToString())[0].GetCustomAttributes(typeof(T), inherit: false);
            if (customAttributes.Length == 0)
            {
                return null;
            }

            return (T)customAttributes[0];
        }

        public static IEnumerable<KeyValuePair<string, string>> GetHeaders(this WebHeaderCollection webHeaderCollection)
        {
            string[] allKeys = webHeaderCollection.AllKeys;
            string[] array = allKeys;
            foreach (string text in array)
            {
                yield return new KeyValuePair<string, string>(text, webHeaderCollection[text]);
            }
        }

        public static string? GetValueByPropertyName(this object obj, string propertyName, string? format = null)
        {
            try
            {
                object obj2 = obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
                if (string.IsNullOrEmpty(format))
                {
                    return Convert.ToString(obj2);
                }

                if (obj2 is DateTime dateTime)
                {
                    return dateTime.ToString(format);
                }

                logger.Error("Parsing failed");
                return null;
            }
            catch (Exception value)
            {
                logger.Error(value);
                return null;
            }
        }

        public static string NewGuidString()
        {
            return Guid.NewGuid().ToString().Replace("-", "")
                .ToLower();
        }

        public static string ToLowerString(this Guid guid)
        {
            return guid.ToString().Replace("-", "").ToLower();
        }
    }
}
