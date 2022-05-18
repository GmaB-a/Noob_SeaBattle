using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    enum fieldCells
    {
        empty = ' ',
        ship = '*',
        miss = '#',
        brokenShip = 'X'
    }

    public class Game
    {
        public int width = 5;
        public int height = 5;
        public int maxAmountOfShipCells;
        string letters = "abcdefghijklmnopqrstuvwxyz";

        Player player1;
        Player player2;
        Player currentPlayer;
        Player notCurrentPlayer;

        Random rnd = new Random();

        public void Play(int gameMode, Player newPlayer1, Player newPlayer2)
        {
            player1 = newPlayer1;
            player2 = newPlayer2;
            maxAmountOfShipCells = 1 + (width * height) / 5;
            player1.CreatePlayer(width, height, maxAmountOfShipCells);
            Console.WriteLine("Press Enter to Start Game");
            Console.ReadLine();
            Console.Clear();
            player2.CreatePlayer(width, height, maxAmountOfShipCells);
            SetPlayMode(gameMode);
            (currentPlayer, notCurrentPlayer) = (player1, player2);
            while (!IsGameEnd())
            {
                ShowBothFields();
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
                if (!player1.isPlayer && !player2.isPlayer) Console.ReadLine();
                if (haveMissed) (currentPlayer, notCurrentPlayer) = (notCurrentPlayer, currentPlayer);
                Console.Clear();
            }
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

        void ShowBothFields()
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
                player2.wins++;
                return true;
            }
            if (player2.shipCount == 0)
            {
                player1.wins++;
                return true;
            }
            return false;
        }

        public (Player, Player) ReturnPlayers() =>
            (player1, player2);
    }
}