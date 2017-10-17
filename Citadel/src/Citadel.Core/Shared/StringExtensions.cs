using System.Text;

namespace Citadel.Shared
{
    public static class StringExtensions
    {
        public static byte[] ToBytes(this string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }

        public static string ToString(this byte[] value, Encoding encoding)
        {
            return encoding.GetString(value);
        }
    }
}
