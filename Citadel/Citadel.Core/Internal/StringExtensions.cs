using System.Text;

namespace Citadel.Internal
{
    internal static class StringExtensions
    {
        internal static byte[] ToBytes(this string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }

        internal static string ToString(this byte[] value, Encoding encoding)
        {
            return encoding.GetString(value);
        }
    }
}
