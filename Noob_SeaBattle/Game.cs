using System;
namespace Noob_SeaBattle
{
    struct Player
    {
        public char[,] field;
        public int shipCount;
        public bool isBot;
        public int playerNumber;
    }

    internal class Game
    {
        const int width = 10;
        const int height = 10;
        const int maxAmountOfShipCells = 1 + (width * height) / 5;
        const string letters = "abcdefghijklmnopqrstuvwxyz";

        static Random rnd = new Random();
        public void Play()
        {
            Player player1 = CreatePlayer(1);
            Console.WriteLine("Ships: *");
            Console.WriteLine("Broken ship: Х");
            Console.WriteLine("Place, where you have already shot, but missed: #");
            Console.WriteLine("Answer: first write a number representing height or y; after that write a letter, representing width or x");
            Console.WriteLine("Both numbers and letters you can see when the game has started");
            Console.WriteLine("Press either 1 or 2 for different modes; 1 - you vs bot; 2 - you vs another player");
            string playModeInString = Console.ReadLine();
            Player player2 = CreatePlayer(2);
            GetPlayMode(playModeInString, player1, player2, out player1, out player2);
            Console.Clear();
            Player currentPlayer = player1;
            Player notCurrentPlayer = player2;
            while (!IsGameEnd(player1.shipCount, player2.shipCount))
            {
                ShowBothFields(player1, player2, currentPlayer);
                (int x, int y) = GetPositionOfPlayerShoot(notCurrentPlayer.field, notCurrentPlayer.shipCount);
                Shoot(x, y, notCurrentPlayer.field, notCurrentPlayer.shipCount, out notCurrentPlayer.shipCount, out bool haveMissed);
                if (haveMissed)
                Console.Clear();
            }
        }

        Player CreatePlayer(int playerNumber)
        {
            Player player = new Player();
            player.field = CreateOneField();
            player.shipCount = maxAmountOfShipCells;
            player.playerNumber = playerNumber;
            return player;
        }

        void GetPlayMode(string playModeInString, Player player1, Player player2, out Player newPlayer1, out Player newPlayer2)
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
                    field[x, y] = ' ';
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

                if (field[x, y] != ' ') field[x, y] = '*';
                else
                {
                    while (field[x, y] == '*')
                    {
                        x = rnd.Next(0, width);
                        y = rnd.Next(0, height);
                    }
                    field[x, y] = '*';
                }


            }
            return field;
        }

        void ShowBothFields(Player player1, Player player2)
        {
            Console.Write("   ");
            WriteThisLetter(width);
            Console.Write(" | ");
            WriteThisLetter(width);
            Console.WriteLine();
            for (int y = 0; y < height; y++)
            {
                if (y < 9 && height >= 10) Console.Write(" ");
                Console.Write(y + 1 + " ");
                WriteThisChar(player1.field, width, y, false);
                Console.Write(" | ");
                WriteThisChar(player2.field, width, y, true);
                Console.WriteLine();
            }
        }

        void WriteThisChar(char[,] field, int width, int y, bool isYou)
        {
            for (int x = 0; x < width; x++)
            {
                if (isYou)
                {
                    if (field[x, y] == '*') Console.Write(' ');
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

        (int, int) GetPositionOfPlayerShoot(char[,] notCurrentPlayerField, int notCurrentPlayerShipCount)
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
            while (!CheckIfShootablePosition(answerX, answerY, notCurrentPlayerField))
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
            else if (enemyField[answerX, answerY] == 'X') return false;
            else if (enemyField[answerX, answerY] == '#') return false;
            return true;
        }

        void Shoot(int x, int y, char[,] enemyField, int enemyShipCount, out int newEnemyShipCount, out bool haveMissed)
        {
            newEnemyShipCount = enemyShipCount;
            if (enemyField[x, y] == '*')
            {
                enemyField[x, y] = 'X';
                newEnemyShipCount -= 1;
                haveMissed = false;
                return;
            }
            else if (enemyField[x, y] == ' ') enemyField[x, y] = '#';
            haveMissed = true;
        }

        void EnemyShootBack(char[,] playerField, int playerShipCount, out int newPlayerShipCount)
        {
            newPlayerShipCount = playerShipCount;
            int x = rnd.Next(0, width);
            int y = rnd.Next(0, height);
            while (playerField[x, y] == 'X' || playerField[x, y] == '#')
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

        bool IsGameEnd(int player1ShipCount, int player2ShipCount)
        {
            if (player1ShipCount == 0)
            {
                Console.WriteLine("Player2 won!");
                return true;
            }
            if (player2ShipCount == 0)
            {
                Console.WriteLine("Player1 won!");
                return true;
            }
            return false;
        }
    }
}