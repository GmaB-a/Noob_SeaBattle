using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    class GameManager
    {
        PlayerDataBaseSerializer PDBS = new PlayerDataBaseSerializer();
        Dictionary<string, PlayerInfo> players;
        public void Intro()
        {
            Console.WriteLine("Hi! Would you like to login or register? log/reg");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "log": 
                    Login(); 
                    break;
                default: 
                    Register(); 
                    break;
            }
            players = PDBS.DeserializeDictionary();
        }

        void Login()
        {
            PlayerInfo playerData;
            do
            {
                Console.WriteLine("Write your login");
                string playerLogin = Console.ReadLine();
                playerData = players[playerLogin];
                if (playerData == null)
                {
                    Console.WriteLine("There is no account with that login. Would you like to register or try once more?");
                    Console.WriteLine("Type 'reg' to register or press Enter to try more");
                    string answer = Console.ReadLine();
                    if (answer == "reg") Register();
                    else return;
                }
                Console.Clear();
            } while (playerData == null);
        }



        void Register()
        {

        }
    }
}