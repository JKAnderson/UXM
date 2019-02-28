using SoulsFormats;
using System;
using System.Collections.Generic;
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
        public List<(string Target, string Replacement)> Replacements;

        public GameInfo(string xmlStr, string dictionaryStr)
        {
            Dictionary = new ArchiveDictionary(dictionaryStr);

            XDocument xml = XDocument.Parse(xmlStr);
            RequiredGB = long.Parse(xml.Root.Element("required_gb").Value);
            BHD5Game = (BHD5.Game)Enum.Parse(typeof(BHD5.Game), xml.Root.Element("bhd5_game").Value);
            Archives = xml.Root.Element("archives").Elements().Select(element => element.Value).ToList();
            BackupDirs = xml.Root.Element("backup_dirs").Elements().Select(element => element.Value).ToList();
            DeleteDirs = xml.Root.Element("delete_dirs").Elements().Select(element => element.Value).ToList();
            Replacements = (from element in xml.Root.Element("replacements").Elements()
                            let args = element.Value.Split('=')
                            select (args[0], args[1])).ToList();
        }

        public static GameInfo GetGameInfo(Util.Game game)
        {
            if (game == Util.Game.DarkSouls2)
                return new GameInfo(Properties.Resources.DarkSouls2GameInfo, Properties.Resources.DarkSouls2Dictionary);
            else if (game == Util.Game.Scholar)
                return new GameInfo(Properties.Resources.ScholarGameInfo, Properties.Resources.ScholarDictionary);
            else if (game == Util.Game.DarkSouls3)
                return new GameInfo(Properties.Resources.DarkSouls3GameInfo, Properties.Resources.DarkSouls3Dictionary);

            throw new ArgumentException("Invalid game type.");
        }
    }
}
