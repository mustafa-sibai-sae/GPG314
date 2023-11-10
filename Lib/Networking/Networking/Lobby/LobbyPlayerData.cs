namespace Networking.Lobby
{
    public class LobbyPlayerData
    {
        public PlayerData PlayerData { get; private set; }
        public bool IsHost { get; private set; }
        public bool IsReady;

        public LobbyPlayerData(PlayerData playerData, bool isHost, bool isReady)
        {
            PlayerData = playerData;
            IsHost = isHost;
            IsReady = isReady;
        }
    }
}