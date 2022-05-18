using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    class GameManager
    {
        PlayerDataBaseSerializer PDBS = new PlayerDataBaseSerializer();

        PlayerInfo player1Info;

        public void PlayGame()
        {
            player1Info = Intro();
            Menu();
        }


        public PlayerInfo Intro()
        {
            PlayerInfo playerInfo;
            do
            {
                Console.WriteLine("Hi! Would you like to login or register? log/reg");
                string answer = Console.ReadLine();
                Console.Clear();
                switch (answer)
                {
                    case "log":
                        playerInfo = Login();
                        break;
                    default:
                        playerInfo = Register();
                        break;
                }
            } while (playerInfo == null);
            Console.Clear();
            return playerInfo;
        }

        PlayerInfo Login()
        {
            PlayerInfo playerData = null;
            while(playerData == null)
            {
                Console.WriteLine("Write your login");
                string playerLogin = Console.ReadLine();
                if (File.Exists($"{playerLogin}.xml")) playerData = PDBS?.GetProfileInfo(playerLogin);
                if (playerData == null)
                {
                    Console.WriteLine("There is no account with that login. Would you like to register or try once more?");
                    Console.WriteLine("Type 'reg' to register or press Enter to try more");
                    string answer = Console.ReadLine();
                    if (answer == "reg") Register();
                    else return null;
                }
                break;
            }

            string password = "";
            while (password != playerData.playerPassword)
            {
                Console.WriteLine("Write your password");
                password = Console.ReadLine();
            }
            return playerData;
        }

        PlayerInfo Register()
        {
            string newLogin;
            do
            {
                Console.WriteLine("Write new login");
                newLogin = Console.ReadLine();
            } while (newLogin == "");

            Console.WriteLine("Write your password");
            string newPassword = Console.ReadLine();
            PlayerInfo newPlayer = new PlayerInfo(newLogin, newPassword);
            PDBS.SaveProfile(newPlayer);
            return newPlayer;
        }

        void Menu()
        {
            string answer = "";
            while (answer != "play" || answer != "stats") {
                Console.WriteLine("if you wanna play game, type 'play'");
                Console.WriteLine("if you wanna check your stats, type 'stats'");
                answer = Console.ReadLine();
                Console.Clear();
                if (answer == "play") StartLobby();
                else if (answer == "stats") ShowStats();
            }
        }

        private void StartLobby()
        {
            Lobby lobby = new Lobby();
            GameIntro();
            playMode = GetPlayMode();
            if (playMode == 0)
            {
                lobby.PlayLobby(playMode, null, null);
            }
            else if (playMode == 2)
            {
                PlayerInfo player2Info = Intro();
                lobby.PlayLobby(playMode, player1Info, player2Info);
            }
            else
            {
                lobby.PlayLobby(playMode, player1Info, null);
            }
        }

        int playMode;

        void GameIntro()
        {
            Console.WriteLine("Ships: " + fieldCells.ship);
            Console.WriteLine("Broken ship: " + fieldCells.brokenShip);
            Console.WriteLine("Place, where you have already shot, but missed: " + fieldCells.miss);
            Console.WriteLine("Answer: first write a number representing height or y; after that write a letter, representing width or x");
            Console.WriteLine("Both numbers and letters you can see when the game has started");
            Console.WriteLine("Press either 0 or 1 or 2 for different modes; 0 - bot vs bot; 1 - you vs bot; 2 - you vs another player");
        }

        int GetPlayMode()
        {
            string playModeInString = Console.ReadLine();
            int.TryParse(playModeInString, out int currentPlayMode);
            Console.Clear();
            return currentPlayMode;
        }

        private void ShowStats()
        {
            player1Info = PDBS.GetProfileInfo(player1Info.playerLogin);
            Console.WriteLine("MMR: " + player1Info.mmr);
            Console.WriteLine("Wins: " + player1Info.wins);
            Console.WriteLine("Loses: " + player1Info.wins);
            Console.ReadLine();
            Console.Clear();
            Menu();
        }
    }
}