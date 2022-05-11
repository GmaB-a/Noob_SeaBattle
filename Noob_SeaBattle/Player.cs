using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    internal class Player
    {
        public char[,] field;
        public int shipCount;
        public bool isPlayer;

        const int width = Game.width;
        const int height = Game.height;
        const int maxAmountOfShipCells = Game.maxAmountOfShipCells;
        Random rnd;

        enum fieldCells
        {
            empty = Game.fieldCells.empty,
            ship = Game.fieldCells.ship,
            miss = Game.fieldCells.miss,
            brokenShip = Game.fieldCells.brokenShip
        }

        public Player CreatePlayer()
        {
            rnd = new Random();
            Player player = new Player();
            player.field = CreateOneField();
            player.shipCount = maxAmountOfShipCells;
            return player;
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
            for (int i = 0; i < Game.maxAmountOfShipCells; i++)
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