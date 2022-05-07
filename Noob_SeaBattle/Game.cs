using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    class Player
    {
        public char[,] field;
        public int shipCount;
        public bool isBot;
        public int playerNumber;
    }

    internal class Game
    {
        const int width = 2;
        const int height = 2;
        int maxAmountOfShipCells;
        string letters = "abcdefghijklmnopqrstuvwxyz";
        Player player1;
        Player player2;

        public enum fieldCells
        {
            empty = ' ',
            ship = '*',
            miss = '#',
            brokenShip = 'X'
        }

        Random rnd = new Random();
        public void Play()
        {
            maxAmountOfShipCells = 1 + (width * height) / 5;
            Intro(out player1, out player2);
            Player currentPlayer = player1;
            Player notCurrentPlayer = player2;
            while (!IsGameEnd())
            {
                ShowBothFields(currentPlayer);
                bool haveMissed;
                int x, y;
                if (!currentPlayer.isBot)
                {
                    (x, y) = GetPlayerShootPosition(notCurrentPlayer.field);
                }
                else
                {
                    (x, y) = GetRandomShootPosition(notCurrentPlayer.field);
                }
                haveMissed = ShootAndCheckMiss(x, y, notCurrentPlayer, out notCurrentPlayer.shipCount);
                if (haveMissed) (currentPlayer, notCurrentPlayer) = ChangeCurrentPlayer(currentPlayer, notCurrentPlayer);
                Console.Clear();
            }
        }

        void Intro(out Player player1, out Player player2)
        {
            player1 = CreatePlayer(1);
            Console.WriteLine("Ships: " + fieldCells.ship);
            Console.WriteLine("Broken ship: " + fieldCells.brokenShip);
            Console.WriteLine("Place, where you have already shot, but missed: " + fieldCells.miss);
            Console.WriteLine("Answer: first write a number representing height or y; after that write a letter, representing width or x");
            Console.WriteLine("Both numbers and letters you can see when the game has started");
            Console.WriteLine("Press either 1 or 2 for different modes; 1 - you vs bot; 2 - you vs another player");
            string playModeInString = Console.ReadLine();
            player2 = CreatePlayer(2);
            GetPlayMode(playModeInString, out player1, out player2);
            Console.Clear();
        }

        Player CreatePlayer(int playerNumber)
        {
            Player player = new Player();
            player.field = CreateOneField();
            player.shipCount = maxAmountOfShipCells;
            player.playerNumber = playerNumber;
            return player;
        }

        void GetPlayMode(string playModeInString, out Player newPlayer1, out Player newPlayer2)
        {
            newPlayer1 = player1;
            newPlayer2 = player2;
            int.TryParse(playModeInString, out int playMode);
            newPlayer1.isBot = false;
            if (playMode == 2) newPlayer2.isBot = false;
            else newPlayer2.isBot = true;
        }

        char[,] CreateOneField()
        {
            char[,] field = new char[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    field[x, y] = (char)fieldCells.empty;
                }
            }

            field = PlaceShips(field);
            return field;
        }

        char[,] PlaceShips(char[,] field)
        {
            for (int i = 0; i < maxAmountOfShipCells; i++)
            {
                int x = rnd.Next(0, width);
                int y = rnd.Next(0, height);

                if (field[x, y] != (char)fieldCells.empty) field[x, y] = (char)fieldCells.ship;
                else
                {
                    while (field[x, y] == (char)fieldCells.ship)
                    {
                        x = rnd.Next(0, width);
                        y = rnd.Next(0, height);
                    }
                    field[x, y] = (char)fieldCells.ship;
                }
            }
            return field;
        }

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
                WriteThisChar(player1.field, y, player1.playerNumber == currentPlayer.playerNumber);
                Console.Write(" | ");
                WriteThisChar(player2.field, y, player2.playerNumber == currentPlayer.playerNumber);
                Console.WriteLine();
            }
        }

        void WriteThisChar(char[,] field, int y, bool isYou)
        {
            for (int x = 0; x < width; x++)
            {
                if (!isYou)
                {
                    if (field[x, y] == (char)fieldCells.ship) Console.Write((char)fieldCells.empty);
                    else Console.Write(field[x, y]);
                    //Console.Write(field[x, y]);
                }
                else Console.Write(field[x, y]);
            }
        }
        void WriteThisLetter(int width)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(letters[x]);
            }
        }

        (int, int) GetPlayerShootPosition(char[,] enemyField)
        {
            Console.WriteLine("write a number");
            string answerYinString = Console.ReadLine();
            int answerY;
            int.TryParse(answerYinString, out answerY);
            answerY -= 1;
            Console.WriteLine("write a letter");
            char answerXinChar = Console.ReadKey().KeyChar;
            int answerX = width;
            for (int i = 0; i < letters.Length; i++)
            {
                if (answerXinChar == letters[i])
                {
                    answerX = i;
                    break;
                }
            }
            while (!CheckIfShootablePosition(answerX, answerY, enemyField))
            {
                Console.WriteLine();
                Console.WriteLine("Write a number");
                answerYinString = Console.ReadLine();
                int.TryParse(answerYinString, out answerY);
                answerY -= 1;

                Console.WriteLine("Write a letter");
                Console.WriteLine();
                answerXinChar = Console.ReadKey().KeyChar;
                for (int i = 0; i < letters.Length; i++)
                {
                    if (answerXinChar == letters[i])
                    {
                        answerX = i;
                        break;
                    }
                }
            }
            return (answerX, answerY);
        }

        bool CheckIfShootablePosition(int answerX, int answerY, char[,] enemyField)
        {
            if (answerY < 0) return false;
            else if (answerY >= height) return false;
            else if (answerX >= width) return false;
            else if (enemyField[answerX, answerY] == (char)fieldCells.brokenShip) return false;
            else if (enemyField[answerX, answerY] == (char)fieldCells.miss) return false;
            return true;
        }

        bool ShootAndCheckMiss(int x, int y, Player enemy, out int newEnemyShipCount)
        {
            newEnemyShipCount = enemy.shipCount;
            if (enemy.field[x, y] == (char)fieldCells.ship)
            {
                enemy.field[x, y] = (char)fieldCells.brokenShip;
                newEnemyShipCount -= 1;
                return false;
            }
            else if (enemy.field[x, y] == (char)fieldCells.empty) enemy.field[x, y] = (char)fieldCells.miss;
            return true;
        }

        (int, int) GetRandomShootPosition(char[,] enemyField)
        {
            int x = rnd.Next(0, width);
            int y = rnd.Next(0, height);
            while (!CheckIfShootablePosition(x, y, enemyField))
            {
                x = rnd.Next(0, width);
                y = rnd.Next(0, height);
            }
            return (x, y);
        }

        (Player, Player) ChangeCurrentPlayer(Player currentPlayer, Player notCurrentPlayer)
        {
            if (currentPlayer.playerNumber == player1.playerNumber)
            {
                currentPlayer = player2;
                notCurrentPlayer = player1;
            }
            else
            {
                currentPlayer = player1;
                notCurrentPlayer = player2;
            }
            return (currentPlayer, notCurrentPlayer);
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