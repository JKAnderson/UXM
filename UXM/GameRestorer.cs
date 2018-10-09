using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace UXM
{
    static class GameRestorer
    {
        public static string Restore(string exePath, IProgress<(double value, string status)> progress, CancellationToken ct)
        {
            progress.Report((0, "Restoring executable..."));
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

            if (File.Exists(gameDir + "\\_backup\\" + exeName))
            {
                try
                {
                    File.Delete(exePath);
                    File.Move(gameDir + "\\_backup\\" + exeName, exePath);
                }
                catch (Exception ex)
                {
                    return $"Failed to restore executable.\r\n\r\n{ex}";
                }
            }

            if (ct.IsCancellationRequested)
                return null;

            List<string> restoreDirs = null;
            List<string> deleteDirs = null;

            if (game == Util.Game.DarkSouls2)
            {
                restoreDirs = darkSouls2RestoreDirs;
                deleteDirs = darkSouls2DeleteDirs;
            }
            else if (game == Util.Game.Scholar)
            {
                restoreDirs = scholarRestoreDirs;
                deleteDirs = scholarDeleteDirs;
            }
            else if (game == Util.Game.DarkSouls3)
            {
                restoreDirs = darkSouls3RestoreDirs;
                deleteDirs = darkSouls3DeleteDirs;
            }

            double totalSteps = restoreDirs.Count + deleteDirs.Count + 1;

            for (int i = 0; i < restoreDirs.Count; i++)
            {
                string restore = restoreDirs[i];
                progress.Report(((i + 1.0) / totalSteps, $"Restoring directory \"{restore}\" ({i + 1}/{restoreDirs.Count})..."));

                string restoreSource = gameDir + "\\_backup\\" + restore;
                string restoreTarget = gameDir + "\\" + restore;

                if (Directory.Exists(restoreSource))
                {
                    try
                    {
                        if (Directory.Exists(restoreTarget))
                            Directory.Delete(restoreTarget, true);
                        Directory.Move(restoreSource, restoreTarget);
                    }
                    catch (Exception ex)
                    {
                        return $"Failed to restore sounds.\r\n\r\n{ex}";
                    }
                }
            }

            try
            {
                for (int i = 0; i < deleteDirs.Count; i++)
                {
                    string dir = deleteDirs[i];

                    progress.Report(((i + 1.0 + restoreDirs.Count) / totalSteps, $"Deleting directory \"{dir}\" ({i + 1}/{deleteDirs.Count})..."));

                    if (ct.IsCancellationRequested)
                        return null;

                    if (Directory.Exists(gameDir + "\\" + dir))
                        Directory.Delete(gameDir + "\\" + dir, true);
                }
            }
            catch (Exception ex)
            {
                return $"Failed to delete directory.\r\n\r\n{ex}";
            }

            try
            {
                if (Directory.Exists(gameDir + "\\_backup") && Directory.GetFiles(gameDir + "\\_backup").Length == 0)
                    Directory.Delete(gameDir + "\\_backup");
            }
            catch (Exception ex)
            {
                return $"Failed to delete backup directory.\r\n\r\n{ex}";
            }

            progress.Report((1, "Restoration complete!"));
            return null;
        }

        private static List<string> darkSouls2RestoreDirs = new List<string>
        {
            "param",
            "sfx",
            "sfx_hq",
            "sound",
        };

        private static List<string> darkSouls2DeleteDirs = new List<string>
        {
            "_unknown",
            "breakobj",
            "decal",
            "eventmakerex",
            "ezstate",
            "filter",
            "map",
            "material",
            "menu",
            "model",
            "model_hq",
            "morpheme4",
            "prefabeditor",
            "timeact",
        };

        private static List<string> scholarRestoreDirs = new List<string>
        {
            "param",
            "sfx",
            "sfx_lq",
            "sound",
        };

        private static List<string> scholarDeleteDirs = new List<string>
        {
            "_unknown",
            "breakobj",
            "decal",
            "eventmakerex",
            "ezstate",
            "filter",
            "map",
            "material",
            "menu",
            "model",
            "model_lq",
            "morpheme4",
            "prefabeditor",
            "timeact",
        };

        private static List<string> darkSouls3RestoreDirs = new List<string>
        {
            "sound",
        };

        private static List<string> darkSouls3DeleteDirs = new List<string>
        {
            "_unknown",
            "action",
            "adhoc",
            "chr",
            "event",
            "facegen",
            "font",
            "map",
            "menu",
            "msg",
            "mtd",
            "obj",
            "other",
            "param",
            "parts",
            "remo",
            "script",
            "sfx",
            "shader",
            "testdata",
        };
    }
}
