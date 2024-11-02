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

            Game checkDs2Version()
            {
                using FileStream fs = File.OpenRead(exePath);
                using var br = new BinaryReader(fs);
                fs.Position = 0x3C;
                uint peOffset = br.ReadUInt32();
                fs.Position = peOffset + 4;
                ushort architecture = br.ReadUInt16();

                return architecture switch
                {
                    0x014C => Game.DarkSouls2,
                    0x8664 => Game.Scholar,
                    _ => throw new InvalidDataException("Could not determine version of DarkSoulsII.exe.\r\n"
                        + $"Unknown architecture found: 0x{architecture:X4}")
                };
            }

            string filename = Path.GetFileName(exePath);
            return filename switch
            {
                "DarkSoulsII.exe" => checkDs2Version(),
                "DarkSoulsIII.exe" => Game.DarkSouls3,
                "sekiro.exe" => Game.Sekiro,
                "DigitalArtwork_MiniSoundtrack.exe" => Game.SekiroBonus,
                "eldenring.exe" => Game.EldenRing,
                "armoredcore6.exe" => Game.ArmoredCore6,
                _ => throw new ArgumentException($"Invalid executable name given: {filename}\r\n"
                    + "Executable file name is expected to be DarkSoulsII.exe, DarkSoulsIII.exe, sekiro.exe, or DigitalArtwork_MiniSoundtrack.exe."),
            };
        }

        public enum Game
        {
            DarkSouls2,
            Scholar,
            DarkSouls3,
            Sekiro,
            SekiroBonus,
            EldenRing,
            ArmoredCore6,
        }
    }
}
