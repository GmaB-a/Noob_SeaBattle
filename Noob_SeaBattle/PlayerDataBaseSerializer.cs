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
        private XmlSerializer Serializer = new XmlSerializer(typeof(PlayerInfo));
        public void SaveProfile(PlayerInfo player)
        {
            using (FileStream stream = new FileStream($"{player.playerLogin}.xml", FileMode.OpenOrCreate))
                Serializer.Serialize(stream, player);
        }

        public PlayerInfo GetProfileInfo(string login)
        {
            using (FileStream fs = new FileStream($"{login}.xml", FileMode.Open))
            {
                PlayerInfo profile = (PlayerInfo)Serializer.Deserialize(fs);
                return profile;
            }
        }
    }
}