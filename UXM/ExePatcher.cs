using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace UXM
{
    class ExePatcher
    {
        private static readonly Encoding UTF16 = Encoding.Unicode;

        public static string Patch(string exePath, IProgress<(double value, string status)> progress, CancellationToken ct)
        {
            progress.Report((0, "Preparing to patch..."));
            string gameDir = Path.GetDirectoryName(exePath);
            string exeName = Path.GetFileName(exePath);

            Util.Game game;
            try
            {
                game = Util.GetExeVersion(exePath);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            if (!File.Exists(gameDir + "\\_backup\\" + exeName))
            {
                try
                {
                    Directory.CreateDirectory(gameDir + "\\_backup");
                    File.Copy(exePath, gameDir + "\\_backup\\" + exeName);
                }
                catch (Exception ex)
                {
                    return $"Failed to backup file:\r\n{exePath}\r\n\r\n{ex}";
                }
            }

            byte[] bytes;
            try
            {
                bytes = File.ReadAllBytes(exePath);
            }
            catch (Exception ex)
            {
                return $"Failed to read file:\r\n{exePath}\r\n\r\n{ex}";
            }

            Dictionary<string, string> replacements = null;
            if (game == Util.Game.DarkSouls2)
                replacements = darkSouls2Replacements;
            else if (game == Util.Game.Scholar)
                replacements = scholarReplacements;
            else if (game == Util.Game.DarkSouls3)
                replacements = darkSouls3Replacements;

            try
            {
                var keys = new List<string>(replacements.Keys);
                for (int i = 0; i < keys.Count; i++)
                {
                    if (ct.IsCancellationRequested)
                        return null;

                    string key = keys[i];

                    // Add 1.0 for preparation step
                    progress.Report(((i + 1.0) / (keys.Count + 1.0), $"Patching alias \"{key}\" ({i + 1}/{keys.Count})..."));

                    replace(bytes, key, replacements[key]);
                }
            }
            catch (Exception ex)
            {
                return $"Failed to patch file:\r\n{exePath}\r\n\r\n{ex}";
            }

            try
            {
                File.WriteAllBytes(exePath, bytes);
            }
            catch (Exception ex)
            {
                return $"Failed to write file:\r\n{exePath}\r\n\r\n{ex}";
            }

            progress.Report((1, "Patching complete!"));
            return null;
        }

        private static void replace(byte[] bytes, string target, string replacement)
        {
            byte[] targetBytes = UTF16.GetBytes(target);
            byte[] replacementBytes = UTF16.GetBytes(replacement);
            if (targetBytes.Length != replacementBytes.Length)
                throw new ArgumentException($"Target length: {targetBytes.Length} | Replacement length: {replacementBytes.Length}");

            List<int> offsets = findBytes(bytes, targetBytes);
            foreach (int offset in offsets)
                Array.Copy(replacementBytes, 0, bytes, offset, replacementBytes.Length);
        }

        private static List<int> findBytes(byte[] bytes, byte[] find)
        {
            List<int> offsets = new List<int>();
            for (int i = 0; i < bytes.Length - find.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < find.Length; j++)
                {
                    if (find[j] != bytes[i + j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                    offsets.Add(i);
            }
            return offsets;
        }

        private static Dictionary<string, string> darkSouls2Replacements = new Dictionary<string, string>
        {
            ["gamedata:/"] = "dlc_root:/",
            ["chrhq:/"] = "title:/",
            ["maphq:/"] = "title:/",
            ["objhq:/"] = "title:/",
            ["partshq:/"] = "title:/./",
        };

        private static Dictionary<string, string> scholarReplacements = new Dictionary<string, string>
        {
            ["gamedata:/"] = "dlc_root:/",
            ["chrlq:/"] = "title:/",
            ["maplq:/"] = "title:/",
            ["objlq:/"] = "title:/",
            ["partslq:/"] = "title:/./",
        };

        private static Dictionary<string, string> darkSouls3Replacements = new Dictionary<string, string>
        {
            ["data1:/"] = "debug:/",
            ["data2:/"] = "debug:/",
            ["data3:/"] = "debug:/",
            ["data4:/"] = "debug:/",
            ["data5:/"] = "debug:/",
            ["game_dlc1:/"] = "interroot:/",
            ["game_dlc2:/"] = "interroot:/",
        };
    }
}
