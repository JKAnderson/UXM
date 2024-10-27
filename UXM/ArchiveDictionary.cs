using SoulsFormats;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UXM
{
    internal class ArchiveDictionary : Dictionary<ulong, string>
    {
        private const uint PRIME32 = 37;
        private const ulong PRIME64 = 133;

        public ArchiveDictionary(string dictionary, BHD5.Game game)
        {
            foreach (string line in Regex.Split(dictionary, "[\r\n]+"))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string path = Normalize(line);
                    ulong hash = FromHash(path, game);
                    this[hash] = path;
                }
            }
        }

        public static string Normalize(string path)
        {
            if (path.Contains(':'))
                path = path[(path.IndexOf(':') + 1)..];

            path = path.ToLowerInvariant().Replace('\\', '/').Trim();

            if (!path.StartsWith('/'))
                path = '/' + path;

            return path;
        }

        public static ulong FromHash(string path, BHD5.Game game)
        {
            if (game == BHD5.Game.EldenRing)
                return path.Aggregate(0ul, (a, c) => a * PRIME64 + c);
            else
                return path.Aggregate(0u, (a, c) => a * PRIME32 + c);
        }
    }
}
