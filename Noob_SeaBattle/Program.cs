using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    class Program
    {
        static int width = 3;
        static int height = 3;
        static int maxAmountOfShipCells = 1 + (width * height) / 5;
        static string letters = "abcdefghijklmnopqrstuvwxyz";
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            char[,] playerField = CreateOneField();
            Console.WriteLine("Ships: *");
            Console.WriteLine("Broken ship: Х");
            Console.WriteLine("Place, where you have already shot, but missed: #");
            Console.WriteLine("Answer: first write a number representing height or y; after that write a letter, representing width or x");
            Console.WriteLine("Both numbers and letters you can see when the game has started");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Console.Clear();
            char[,] enemyField = CreateOneField();
            int playerShipCount = maxAmountOfShipCells;
            int enemyShipCount = maxAmountOfShipCells;
            while (!IsGameEnd(playerShipCount, enemyShipCount))
            {
                ShowBothFields(playerField, enemyField);
                GetAnswer(enemyField, enemyShipCount, out enemyShipCount);
                EnemyShootBack(playerField, playerShipCount, out playerShipCount);
                Console.Clear();
            }
        }

        static char[,] CreateOneField()
        {
            char[,] field = new char[width, height];
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    field[x, y] = ' ';
                }
            }
            
            field = PlaceShips(field);
            return field;
        }

        static char[,] PlaceShips(char[,] field)
        {
            for (int i = 0; i < maxAmountOfShipCells; i++)
            {
                int x = rnd.Next(0, width);
                int y = rnd.Next(0, height);

                if (field[x, y] != '*') field[x, y] = '*';
                else
                {
                    while(field[x, y] == '*')
                    {
                        x = rnd.Next(0, width);
                        y = rnd.Next(0, height);
                    }
                    field[x, y] = '*';
                }

                
            }
            return field;
        }

        static void ShowBothFields(char[,] playerField, char[,] enemyField)
        {
            Console.Write("  ");
            if (height >= 10) Console.Write(" ");
            WriteThisLetter(width);
            Console.Write(" | ");
            WriteThisLetter(width);
            Console.WriteLine();
            if (height >= 10)
            {
                for (int y = 0; y < (height - 1); y++)
                {
                    Console.Write(" " + (y + 1) + " ");
                    WriteThisChar(playerField, width, y, false);
                    Console.Write(" | ");
                    WriteThisChar(enemyField, width, y, true);
                    Console.WriteLine();
                }
                for (int y = (height - 1); y < height; y++)
                {
                    Console.Write(y + 1 + " ");
                    WriteThisChar(playerField, width, y, false);
                    Console.Write(" | ");
                    WriteThisChar(enemyField, width, y, true);
                    Console.WriteLine();
                }
            }
            else
            {
                for (int y = 0; y < height; y++)
                {
                    Console.Write((y + 1) + " ");
                    WriteThisChar(playerField, width, y, false);
                    Console.Write(" | ");
                    WriteThisChar(enemyField, width, y, true);
                    Console.WriteLine();
                }
            }
        }

        static void WriteThisChar(char[,] field, int width, int y, bool isEnemy)
        {
            for (int x = 0; x < width; x++)
            {
                if (isEnemy)
                {
                    if (field[x, y] == '*') Console.Write(' ');
                    else Console.Write(field[x, y]);
                    //Console.Write(field[x, y]);
                }
                else Console.Write(field[x, y]);
            }
        }
        static void WriteThisLetter(int width)
        {
            
            for (int x = 0; x < width; x++)
            {
                Console.Write(letters[x]);
            }
        }

        static void GetAnswer(char[,] enemyField, int enemyShipCount, out int newEnemyShipCount)
        {
            Console.WriteLine("Write a number");
            string answerYinString = Console.ReadLine();
            int.TryParse(answerYinString, out int answerY);
            answerY -= 1;
            if (answerY < 0 || answerY >= height)
            {
                GetAnswer(enemyField, enemyShipCount, out newEnemyShipCount);
            }

            Console.WriteLine("Write a letter");
            char answerXinChar = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (!Char.IsLetter(answerXinChar))
            {
                GetAnswer(enemyField, enemyShipCount, out newEnemyShipCount);
            }
            
            int answerX = -1;
            for(int i = 0; i < letters.Length; i++)
            {
                if(answerXinChar == letters[i])
                {
                    answerX = i;
                    break;
                }
            }

            if(answerX > width)
            {
                GetAnswer(enemyField, enemyShipCount, out newEnemyShipCount);
            }

            Shoot(answerX, answerY, enemyField, enemyShipCount, out newEnemyShipCount);
        }

        static void Shoot(int x, int y, char[,] enemyField, int enemyShipCount, out int newEnemyShipCount)
        {
            newEnemyShipCount = enemyShipCount;
            if (enemyField[x, y] == '*')
            {
                enemyField[x, y] = 'X';
                newEnemyShipCount -= 1;
            }
            else if (enemyField[x, y] == ' ') enemyField[x, y] = '#';
            else
            {
                Console.WriteLine("bruh");
                GetAnswer(enemyField, enemyShipCount, out newEnemyShipCount);
            }
        }

        static void EnemyShootBack(char[,] playerField, int playerShipCount, out int newPlayerShipCount)
        {
            newPlayerShipCount = playerShipCount;
            int x = rnd.Next(0, width);
            int y = rnd.Next(0, height);
            while(playerField[x, y] == 'X' || playerField[x, y] == '#')
            {
                x = rnd.Next(0, width);
                y = rnd.Next(0, height);
            }
            if (playerField[x, y] == '*')
            {
                playerField[x, y] = 'X';
                newPlayerShipCount -= 1;
            }
            else if (playerField[x, y] == ' ') playerField[x, y] = '#';
        }

        static bool IsGameEnd(int playerShipCount, int enemyShipCount)
        {
            if(playerShipCount == 0)
            {
                Console.WriteLine("You won!");
                return true;
            }
            if (enemyShipCount == 0)
            {
                Console.WriteLine("You lost :(");
                return true;
            }
            return false;
        }
    }
}
