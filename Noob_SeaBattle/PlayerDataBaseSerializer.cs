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
        public void SerializeDictionary(Dictionary<string, PlayerInfo> dictionary)
        {
            using (FileStream fs = new FileStream("players.xml", FileMode.OpenOrCreate))
            {
                List<PlayerInfo> items = new List<PlayerInfo>(dictionary.Count);
                foreach (string login in dictionary.Keys)
                {
                    items.Add(new PlayerInfo(login, dictionary[login].playerPassword));
                }
                XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerInfo>));
                /*XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", ""); */
                serializer.Serialize(fs, items);
            }
        }

        public Dictionary<string, PlayerInfo> DeserializeDictionary()
        {
            using (FileStream fs = new FileStream("players.xml", FileMode.OpenOrCreate))
            {
                Dictionary<string, PlayerInfo> dictionary = new Dictionary<string, PlayerInfo>();
                XmlSerializer xs = new XmlSerializer(typeof(List<PlayerInfo>));
                List<PlayerInfo> templist = (List<PlayerInfo>)xs.Deserialize(fs);
                foreach (PlayerInfo PI in templist)
                {
                    dictionary.Add(PI.playerLogin, PI);
                }
                return dictionary;
            }
        }
    }
}