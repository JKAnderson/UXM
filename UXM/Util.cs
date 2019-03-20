using System;
using System.IO;

namespace UXM
{
    static class Util
    {
        public static Game GetExeVersion(string exePath)
        {
            if (!File.Exists(exePath))
            {
                throw new FileNotFoundException($"Executable not found at path: {exePath}\r\n"
                    + "Please browse to an existing executable.");
            }

            string filename = Path.GetFileName(exePath);
            if (filename == "sekiro.exe")
            {
                return Game.Sekiro;
            }
            else if (filename == "DarkSoulsIII.exe")
            {
                return Game.DarkSouls3;
            }
            else if (filename == "DarkSoulsII.exe")
            {
                using (FileStream fs = File.OpenRead(exePath))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    fs.Position = 0x3C;
                    uint peOffset = br.ReadUInt32();
                    fs.Position = peOffset + 4;
                    ushort architecture = br.ReadUInt16();

                    if (architecture == 0x014C)
                    {
                        return Game.DarkSouls2;
                    }
                    else if (architecture == 0x8664)
                    {
                        return Game.Scholar;
                    }
                    else
                    {
                        throw new InvalidDataException("Could not determine version of DarkSoulsII.exe.\r\n"
                            + $"Unknown architecture found: 0x{architecture:X4}");
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Invalid executable name given: {filename}\r\n"
                    + "Executable file name is expected to be either DarkSoulsII.exe or DarkSoulsIII.exe.");
            }
        }

        public enum Game
        {
            DarkSouls2,
            Scholar,
            DarkSouls3,
            Sekiro,
        }
    }
}
