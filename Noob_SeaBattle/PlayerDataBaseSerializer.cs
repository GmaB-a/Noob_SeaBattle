using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    class PlayerDataBaseSerializer
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Dictionary<string, PlayerInfo>));

        string saveFileName = "players.xml";
        public Dictionary<string, PlayerInfo> GetData()
        {
            using (FileStream fs = new FileStream(saveFileName, FileMode.OpenOrCreate))
            {
                Dictionary<string, PlayerInfo> playerInfos = xmlSerializer.Deserialize(fs) as Dictionary<string, PlayerInfo>;
                return playerInfos;
            }
            
        }

        public void SerializeData(Dictionary<string, PlayerInfo> playerInfos)
        {
            using (FileStream fs = new FileStream(saveFileName, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, playerInfos);
            }
        }
    }
}