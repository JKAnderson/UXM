using BinderTool.Core;
using BinderTool.Core.Bdt5;
using BinderTool.Core.Bhd5;
using BinderTool.Core.Dcx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UXM
{
    static class ArchiveUnpacker
    {
        private const int WRITE_LIMIT = 1024 * 1024 * 100;
        private static readonly Encoding ASCII = Encoding.ASCII;

        public static string Unpack(string exePath, IProgress<(double value, string status)> progress, CancellationToken ct)
        {
            progress.Report((0, "Preparing to unpack..."));
            string gameDir = Path.GetDirectoryName(exePath);

            Util.Game game;
            GameInfo gameInfo;
            try
            {
                game = Util.GetExeVersion(exePath);
                gameInfo = GameInfo.GetGameInfo(game);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            Dictionary<string, string> keys = null;
            if (game == Util.Game.DarkSouls2 || game == Util.Game.Scholar)
            {
                try
                {
                    keys = new Dictionary<string, string>();
                    foreach (string archive in gameInfo.Archives)
                    {
                        string pemPath = gameDir + "\\" + archive.Replace("Ebl", "KeyCode") + ".pem";
                        keys[archive] = File.ReadAllText(pemPath);
                    }
                }
                catch (Exception ex)
                {
                    return $"Failed to load Dark Souls 2 archive keys.\r\n\r\n{ex}";
                }
            }
            else if (game == Util.Game.DarkSouls3)
            {
                keys = ArchiveKeys.DarkSouls3Keys;
            }

            string drive = Path.GetPathRoot(Path.GetFullPath(gameDir));
            DriveInfo driveInfo = new DriveInfo(drive);

            if (driveInfo.AvailableFreeSpace < gameInfo.RequiredGB * 1024 * 1024 * 1024)
            {
                DialogResult choice = MessageBox.Show(
                    $"{gameInfo.RequiredGB} GB of free space is required to fully unpack this game; " +
                    $"only {driveInfo.AvailableFreeSpace / (1024f * 1024 * 1024):F1} GB available.\r\n" +
                    "If you're only doing a partial unpack to restore some files you may ignore this warning, " +
                    "otherwise it will most likely fail.\r\n\r\n" +
                    "Do you want to continue?",
                    "Space Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (choice == DialogResult.No)
                    return null;
            }

            if (ct.IsCancellationRequested)
                return null;

            try
            {
                for (int i = 0; i < gameInfo.BackupDirs.Count; i++)
                {
                    string backup = gameInfo.BackupDirs[i];
                    progress.Report(((1.0 + (double)i / gameInfo.BackupDirs.Count) / (gameInfo.Archives.Count + 2.0),
                        $"Backing up directory \"{backup}\" ({i + 1}/{gameInfo.BackupDirs.Count})..."));

                    string backupSource = gameDir + "\\" + backup;
                    string backupTarget = gameDir + "\\_backup\\" + backup;

                    if (!Directory.Exists(backupTarget))
                    {
                        foreach (string file in Directory.GetFiles(backupSource, "*", SearchOption.AllDirectories))
                        {
                            string relative = file.Substring(backupSource.Length + 1);
                            string target = backupTarget + "\\" + relative;
                            Directory.CreateDirectory(Path.GetDirectoryName(target));
                            File.Copy(file, target);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Failed to back up directories.\r\n\r\n{ex}";
            }

            for (int i = 0; i < gameInfo.Archives.Count; i++)
            {
                if (ct.IsCancellationRequested)
                    return null;

                string archive = gameInfo.Archives[i];
                string error = UnpackArchive(gameDir, archive, keys[archive], i,
                    gameInfo.Archives.Count, gameInfo.BinderToolVersion, gameInfo.Dictionary, progress, ct).Result;
                if (error != null)
                    return error;
            }

            progress.Report((1, "Unpacking complete!"));
            return null;
        }

        private static async Task<string> UnpackArchive(string gameDir, string archive, string key, int index, int total,
            GameVersion gameVersion, ArchiveDictionary archiveDictionary,
            IProgress<(double value, string status)> progress, CancellationToken ct)
        {
            progress.Report(((index + 2.0) / (total + 2.0), $"Loading {archive}..."));
            string bhdPath = gameDir + "\\" + archive + ".bhd";
            string bdtPath = gameDir + "\\" + archive + ".bdt";

            if (File.Exists(bhdPath) && File.Exists(bdtPath))
            {
                Bhd5File bhd;
                try
                {
                    bool encrypted = true;
                    using (FileStream fs = File.OpenRead(bhdPath))
                    {
                        byte[] magic = new byte[4];
                        fs.Read(magic, 0, 4);
                        encrypted = ASCII.GetString(magic) != "BHD5";
                    }

                    if (encrypted)
                    {
                        using (MemoryStream bhdStream = CryptographyUtility.DecryptRsa(bhdPath, key))
                        {
                            bhd = Bhd5File.Read(bhdStream, gameVersion);
                        }
                    }
                    else
                    {
                        using (FileStream bhdStream = File.OpenRead(bhdPath))
                        {
                            bhd = Bhd5File.Read(bhdStream, gameVersion);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return $"Failed to open BHD:\n{bhdPath}\n\n{ex}";
                }

                int fileCount = 0;
                foreach (Bhd5Bucket bucket in bhd.GetBuckets())
                    fileCount += bucket.GetEntries().Count();

                try
                {
                    var asyncFileWriters = new List<Task<long>>();
                    using (Bdt5FileStream bdt = Bdt5FileStream.OpenFile(bdtPath, FileMode.Open, FileAccess.Read))
                    {
                        int currentFile = -1;
                        long writingSize = 0;

                        foreach (Bhd5Bucket bucket in bhd.GetBuckets())
                        {
                            if (ct.IsCancellationRequested)
                                break;

                            foreach (Bhd5BucketEntry entry in bucket.GetEntries())
                            {
                                if (ct.IsCancellationRequested)
                                    break;

                                currentFile++;

                                string path;
                                if (archiveDictionary.GetPath(entry.FileNameHash, out path))
                                {
                                    path = gameDir + path.Replace('/', '\\');
                                }
                                else
                                {
                                    path = $"{gameDir}\\_unknown\\{archive}_{entry.FileNameHash:D10}";
                                }

                                if (File.Exists(path))
                                    continue;

                                progress.Report(((index + 2.0 + currentFile / (double)fileCount) / (total + 2.0),
                                    $"Unpacking {archive} ({currentFile + 1}/{fileCount})..."));

                                if (entry.FileSize == 0)
                                {
                                    MemoryStream header = bdt.Read(entry.FileOffset, 48);
                                    if (entry.IsEncrypted)
                                    {
                                        MemoryStream disposer = header;
                                        header = CryptographyUtility.DecryptAesEcb(header, entry.AesKey.Key);
                                        disposer.Dispose();
                                    }

                                    byte[] signatureBytes = new byte[4];
                                    header.Read(signatureBytes, 0, 4);
                                    string signature = ASCII.GetString(signatureBytes);
                                    if (signature != DcxFile.DcxSignature)
                                    {
                                        throw new Exception("Zero-length entry is not DCX in BHD:\r\n" + bhdPath);
                                    }

                                    header.Position = 0;
                                    entry.FileSize = DcxFile.DcxSize + DcxFile.ReadCompressedSize(header);
                                    header.Dispose();
                                }

                                while (asyncFileWriters.Count > 0 && writingSize + entry.FileSize > WRITE_LIMIT)
                                {
                                    for (int i = 0; i < asyncFileWriters.Count; i++)
                                    {
                                        if (asyncFileWriters[i].IsCompleted)
                                        {
                                            writingSize -= await asyncFileWriters[i];
                                            asyncFileWriters.RemoveAt(i);
                                        }
                                    }

                                    if (asyncFileWriters.Count > 0 && writingSize + entry.FileSize > WRITE_LIMIT)
                                        Thread.Sleep(10);
                                }

                                MemoryStream data;
                                if (entry.IsEncrypted)
                                {
                                    data = bdt.Read(entry.FileOffset, entry.PaddedFileSize);
                                    CryptographyUtility.DecryptAesEcb(data, entry.AesKey.Key, entry.AesKey.Ranges);
                                    data.Position = 0;
                                    data.SetLength(entry.FileSize);
                                }
                                else
                                {
                                    data = bdt.Read(entry.FileOffset, entry.FileSize);
                                }

                                try
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                                    writingSize += data.Length;
                                    asyncFileWriters.Add(WriteFileAsync(path, data));
                                }
                                catch (Exception ex)
                                {
                                    return $"Failed to write file:\r\n{path}\r\n\r\n{ex}";
                                }
                            }
                        }
                    }

                    foreach (Task<long> task in asyncFileWriters)
                        await task;
                }
                catch (Exception ex)
                {
                    return $"Failed to unpack BDT:\r\n{bdtPath}\r\n\r\n{ex}";
                }
            }

            return null;
        }

        private static async Task<long> WriteFileAsync(string path, MemoryStream ms)
        {
            using (FileStream fs = new FileStream(
                path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                ms.Position = 0;
                while (ms.Position < ms.Length)
                {
                    int size = (int)Math.Min(1024 * 1024, ms.Length - ms.Position);
                    byte[] bytes = new byte[size];
                    ms.Read(bytes, 0, size);
                    await fs.WriteAsync(bytes, 0, size);
                }
            }

            long length = ms.Length;
            ms.Dispose();
            return length;
        }
    }
}
