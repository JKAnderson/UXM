using SoulsFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace UXM
{
    class GameInfo
    {
        public long RequiredGB;
        public BHD5.Game BHD5Game;
        public List<string> Archives;
        public ArchiveDictionary Dictionary;
        public List<string> BackupDirs;
        public List<string> DeleteDirs;
        public List<string> Replacements;

        public GameInfo(string xmlStr, string dictionaryStr)
        {
            XDocument xml = XDocument.Parse(xmlStr);
            RequiredGB = long.Parse(xml.Root.Element("required_gb").Value);
            BHD5Game = (BHD5.Game)Enum.Parse(typeof(BHD5.Game), xml.Root.Element("bhd5_game").Value);
            Archives = xml.Root.Element("archives").Elements().Select(element => element.Value).ToList();
            BackupDirs = xml.Root.Element("backup_dirs").Elements().Select(element => element.Value).ToList();
            DeleteDirs = xml.Root.Element("delete_dirs").Elements().Select(element => element.Value).ToList();
            Replacements = xml.Root.Element("replacements").Elements().Select(element => element.Value).ToList();

            Dictionary = new ArchiveDictionary(dictionaryStr, BHD5Game);
        }

        public static GameInfo GetGameInfo(Util.Game game)
        {
            string prefix = game switch
            {
                Util.Game.DarkSouls2 => "DarkSouls2",
                Util.Game.Scholar => "Scholar",
                Util.Game.DarkSouls3 => "DarkSouls3",
                Util.Game.Sekiro => "Sekiro",
                Util.Game.SekiroBonus => "SekiroBonus",
                Util.Game.EldenRing => "EldenRing",
                Util.Game.ArmoredCore6 => "ArmoredCore6",
                _ => throw new ArgumentException("Invalid game type."),
            };

#if DEBUG
            string gameInfo = File.ReadAllText($@"..\..\..\dist\res\{prefix}GameInfo.xml");
            string dictionary = File.ReadAllText($@"..\..\..\dist\res\{prefix}Dictionary.txt");
#else
            string gameInfo = File.ReadAllText($@"res\{prefix}GameInfo.xml");
            string dictionary = File.ReadAllText($@"res\{prefix}Dictionary.txt");
#endif
            return new GameInfo(gameInfo, dictionary);
        }
    }
}
