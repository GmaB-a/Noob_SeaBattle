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

        public int player1Wins;
        public int player2Wins;

        Game game;
        int playMode;

        public void PlayLobby()
        {
            Intro();
            GetPlayMode();

            do
            {
                StartRound();
                EndRound();
                PrintScore();
            } while (!DoesPlayerHave3Wins());

            if (player1Wins == 3) Console.WriteLine("Player1 Won! Congrats");
            else Console.WriteLine("Player2 Won! Congrats");
        }

        void StartRound()
        {
            game = new Game();
            game.Play(playMode);
        }

        void Intro()
        {
            Console.WriteLine("Ships: " + fieldCells.ship);
            Console.WriteLine("Broken ship: " + fieldCells.brokenShip);
            Console.WriteLine("Place, where you have already shot, but missed: " + fieldCells.miss);
            Console.WriteLine("Answer: first write a number representing height or y; after that write a letter, representing width or x");
            Console.WriteLine("Both numbers and letters you can see when the game has started");
            Console.WriteLine("Press either 0 or 1 or 2 for different modes; 0 - bot vs bot; 1 - you vs bot; 2 - you vs another player");
        }

        void GetPlayMode()
        {
            string playModeInString = Console.ReadLine();
            int.TryParse(playModeInString, out playMode);
            Console.Clear();
        }

        void EndRound()
        {
            int playerWonNumber = game.ReturnPlayerWon();
            switch (playerWonNumber)
            {
                case 1:
                    player1Wins++;
                    break;
                case 2:
                    player2Wins++;
                    break;
            }
        }

        void PrintScore()
        {
            Console.WriteLine("Player1 Wins : Player2 Wins");
            Console.WriteLine("      " + player1Wins + "      :     " + player2Wins + "      ");
            Console.ReadLine();
            Console.Clear();
        }

        bool DoesPlayerHave3Wins()
        {
            if (player1Wins == AmountOfWinsRequired || player2Wins == AmountOfWinsRequired) return true;
            return false;
        }
    }
}