namespace Noob_SeaBattle
{
    internal class PlayerInfo
    {
        int starterMMR = 500;

        string login;
        string password;
        int mmr;
        int wins;
        int loses;

        public PlayerInfo CreateNewPlayer(string login, string password)
        {
            PlayerInfo newPlayerInfo = new PlayerInfo();
            newPlayerInfo.login = login;
            newPlayerInfo.password = password;
            newPlayerInfo.mmr = starterMMR;
            newPlayerInfo.wins = 0;
            newPlayerInfo.loses = 0;
            return newPlayerInfo;
        }
    }
}