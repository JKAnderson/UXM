using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UXM
{
    class ArchiveDictionary
    {
        private const uint PRIME = 37;

        private Dictionary<uint, string> hashes;

        public ArchiveDictionary(string dictionary)
        {
            hashes = new Dictionary<uint, string>();
            foreach (string line in Regex.Split(dictionary, "[\r\n]+"))
            {
                string trimmed = line.Trim();
                if (trimmed.Length > 0)
                {
                    uint hash = ComputeHash(trimmed);
                    hashes[hash] = trimmed;
                }
            }
        }

        private static uint ComputeHash(string path)
        {
            string hashable = path.Trim().Replace('\\', '/').ToLowerInvariant();
            if (!hashable.StartsWith("/"))
                hashable = '/' + hashable;
            return hashable.Aggregate(0u, (i, c) => i * PRIME + c);
        }

        public bool GetPath(uint hash, out string path)
        {
            return hashes.TryGetValue(hash, out path);
        }
    }
}
