using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsGlitchHarvester
{
    public static class WGH_Extensions
    {
        public static T[] SubArray<T>(this T[] data, long index, long length)
        {
            T[] result = new T[length];

			if (data == null)
				return null;

            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static string ToBase64(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64(this string base64)
        {
            var data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }

        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
