using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    internal class Game
    {
        public const int width = 5;
        public const int height = 5;
        public const int maxAmountOfShipCells = 1 + (width * height) / 5;
        string letters = "abcdefghijklmnopqrstuvwxyz";

        Player player1 = new Player();
        Player player2 = new Player();
        Player currentPlayer;
        Player notCurrentPlayer;

        Random rnd = new Random();

        public enum fieldCells
        {
            empty = ' ',
            ship = '*',
            miss = '#',
            brokenShip = 'X'
        }

        public void Play()
        {
            player1.CreatePlayer();
            Intro();
            bool isOnlyBots = GetPlayMode();
            player2.CreatePlayer();
            (currentPlayer, notCurrentPlayer) = (player1, player2);
            while (!IsGameEnd())
            {
                ShowBothFields(currentPlayer);
                bool haveMissed;
                int x, y;
                if (currentPlayer.isPlayer)
                {
                    (x, y) = GetPlayerShootPosition();
                }
                else
                {
                    (x, y) = GetRandomShootPosition();
                }
                haveMissed = ShootAndCheckMiss(x, y);
                if (isOnlyBots) Console.ReadLine();
                if (haveMissed) (currentPlayer, notCurrentPlayer) = (notCurrentPlayer, currentPlayer);
                Console.Clear();
            }
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


        bool GetPlayMode()
        {
            string playModeInString = Console.ReadLine();
            int.TryParse(playModeInString, out int playMode);
            SetPlayMode(playMode);
            Console.Clear();
            if (playMode == 0) return true;
            return false;
        }

        void SetPlayMode(int playMode)
        {
            switch (playMode)
            {
                case 0: (player1.isPlayer, player2.isPlayer) = (false, false); break;
                case 1:
                    if (rnd.Next(0,100) < 50) 
                        (player1.isPlayer, player2.isPlayer) = (true, false);
                    else 
                        (player1.isPlayer, player2.isPlayer) = (false, true);
                    break;
                case 2: (player1.isPlayer, player2.isPlayer) = (true, true); break;
            }
        }

        /*(bool, bool) SetPlayMode(int playMode) => playMode switch
        {
            0 => (false, false),
            1 => (true, false),
            _ => (true, true)
        }; */

        void ShowBothFields(Player currentPlayer)
        {
            Console.Write("   ");
            WriteThisLetter(width);
            Console.Write(" | ");
            WriteThisLetter(width);
            Console.WriteLine();
            for (int y = 0; y < height; y++)
            {
                if (y < 9) Console.Write(" ");
                Console.Write(y + 1 + " ");
                WriteThisChar(player1, y);
                Console.Write(" | ");
                WriteThisChar(player2, y);
                Console.WriteLine();
            }
        }

        void WriteThisChar(Player player, int y)
        {
            bool needToShowTile = needToShowThisTile(player);
            for (int x = 0; x < width; x++)
            {
                Console.Write(WriteTrueChar(player.field[x, y], needToShowTile));
            }
        }
        bool needToShowThisTile(Player player)
        {
            if(!player1.isPlayer && !player2.isPlayer) return true;
            if (player != currentPlayer) return false;
            return true;
        }

        char WriteTrueChar(char ThisTile, bool needToShowTile)
        {
            if (ThisTile == (char)fieldCells.ship && !needToShowTile) 
                return ((char)fieldCells.empty);

            return ThisTile;
        }


        void WriteThisLetter(int width)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(letters[x]);
            }
        }

        (int, int) GetPlayerShootPosition()
        {
            int answerX = width;
            int answerY;
            do
            {
                Console.WriteLine("Write a number");
                string answerYinString = Console.ReadLine();
                int.TryParse(answerYinString, out answerY);
                answerY -= 1;

                Console.WriteLine("Write a letter");
                char answerXinChar = Console.ReadKey().KeyChar;
                for (int i = 0; i < letters.Length; i++)
                {
                    if (answerXinChar == letters[i])
                    {
                        answerX = i;
                        break;
                    }
                }
            } while (!CheckIfShootablePosition(answerX, answerY));
            return (answerX, answerY);
        }

        bool CheckIfShootablePosition(int x, int y)
        {
            if (y < 0 || y >= height) return false;
            if (x >= width) return false;
            if (notCurrentPlayer.field[x, y] == (char)fieldCells.brokenShip) return false;
            if (notCurrentPlayer.field[x, y] == (char)fieldCells.miss) return false;
            return true;
        }

        bool ShootAndCheckMiss(int x, int y)
        {
            if (notCurrentPlayer.field[x, y] == (char)fieldCells.ship)
            {
                notCurrentPlayer.field[x, y] = (char)fieldCells.brokenShip;
                notCurrentPlayer.shipCount -= 1;
                return false;
            }
            else if (notCurrentPlayer.field[x, y] == (char)fieldCells.empty) notCurrentPlayer.field[x, y] = (char)fieldCells.miss;
            return true;
        }

        (int, int) GetRandomShootPosition()
        {
            int x;
            int y;
            do{
                x = rnd.Next(0, width);
                y = rnd.Next(0, height);
            } while (!CheckIfShootablePosition(x, y));
            return (x, y);
        }

        bool IsGameEnd()
        {
            if (player1.shipCount == 0)
            {
                Console.WriteLine("Player2 won!");
                return true;
            }
            if (player2.shipCount == 0)
            {
                Console.WriteLine("Player1 won!");
                return true;
            }
            return false;
        }
    }
}