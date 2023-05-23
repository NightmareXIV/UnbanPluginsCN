using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnbanPluginsCN
{
    internal static class Utils
    {
         static Random random = new Random();
        internal static int RandomNumber => Utils.random.Next(2147483647 - 1) + 1;

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static bool ContainsAny(this string obj, StringComparison comp, params string[] values)
        {
            foreach (var x in values)
            {
                if (obj.IndexOf(x, comp) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
