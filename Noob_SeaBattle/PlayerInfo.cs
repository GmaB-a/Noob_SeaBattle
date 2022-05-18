using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noob_SeaBattle
{
    class PlayerInfo
    {
        int starterMMR = 500;

        public string playerLogin;
        public string playerPassword;
        public int mmr;
        public int wins = 0;
        public int loses = 0;

        public PlayerInfo(string login, string password)
        {
            playerLogin = login;
            playerPassword = password;
            mmr = starterMMR;
        }

        public PlayerInfo()
        {
            playerLogin = RandomString(8);
            playerPassword = RandomString(8);
            mmr = starterMMR;
        }

        private Random random = new Random();

        public string RandomString(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}