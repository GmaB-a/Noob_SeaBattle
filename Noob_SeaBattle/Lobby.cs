using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    internal class Lobby
    {
        int AmountOfWinsRequired = 3;

        public Player player1 = new Player();
        public Player player2 = new Player();

        Game game;
        int playMode;

        public void PlayLobby(int currentPlayMode, PlayerInfo player1, PlayerInfo player2)
        {
            playMode = currentPlayMode;
            do
            {
                PlayRound();
                PrintScore();
            } while (!DoesPlayerHave3Wins());

            if (player1.wins == 3) AddWinAndChangeMMR(player1, player2);
            else AddWinAndChangeMMR(player2, player1);

            SavePlayerInfos(player1, player2);
        }

        void PlayRound()
        {
            game = new Game();
            game.Play(playMode, player1, player2);
            (player1, player2) = game.ReturnPlayers();
        }


        void PrintScore()
        {
            Console.WriteLine("Player1 Wins : Player2 Wins");
            Console.WriteLine("      " + player1.wins + "      :     " + player2.wins + "      ");
            Console.ReadLine();
            Console.Clear();
        }

        bool DoesPlayerHave3Wins()
        {
            if (player1.wins == AmountOfWinsRequired || player2.wins == AmountOfWinsRequired) return true;
            return false;
        }

        void AddWinAndChangeMMR(PlayerInfo winner, PlayerInfo loser)
        {
            winner.wins++;
            loser.loses++;
            winner.mmr += 10;
            loser.mmr -= 10;
        }

        private void SavePlayerInfos(PlayerInfo player1, PlayerInfo player2)
        {
            PlayerDataBaseSerializer PDBS = new PlayerDataBaseSerializer();
            PDBS.SaveProfile(player1);
            PDBS.SaveProfile(player2);
        }
    }
}