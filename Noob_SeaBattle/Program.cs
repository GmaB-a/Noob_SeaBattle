using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Lobby lobby = new Lobby();
            lobby.Intro();
            lobby.GetPlayMode();

            do
            {
                lobby.StartRound();
                lobby.EndRound();
                lobby.PrintScore();
            } while (!lobby.DoesPlayerHave3Wins());

            if(lobby.player1Wins == 3) Console.WriteLine("Player1 Won! Congrats");
            else Console.WriteLine("Player2 Won! Congrats");
        }
    }
}
