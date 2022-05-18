using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    public class Player
    {
        public char[,] field;
        public int shipCount;
        public bool isPlayer;
        public PlayerInfo thisPlayerInfo;
        public int wins;

        int width;
        int height;
        int maxAmountOfShipCells;

        Random rnd;

        public void CreatePlayer(int widthFromGame, int heightFromGame, int maxAmountOfShipCellsFromGame)
        {
            width = widthFromGame;
            height = heightFromGame;
            maxAmountOfShipCells = maxAmountOfShipCellsFromGame;
            rnd = new Random();
            field = CreateOneField();
            shipCount = maxAmountOfShipCells;
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
            int x;
            int y;
            for (int i = 0; i < maxAmountOfShipCells; i++)
            {
                do{
                    x = rnd.Next(0, width);
                    y = rnd.Next(0, height);
                } while (field[x, y] == (char)fieldCells.ship);
                field[x, y] = (char)fieldCells.ship;
            }
            return field;
        }
    }
}