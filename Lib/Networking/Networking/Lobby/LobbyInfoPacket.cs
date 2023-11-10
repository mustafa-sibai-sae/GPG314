namespace Networking.Lobby
{
    public class LobbyInfoPacket : BasePacket
    {
        public PlayerData[] PlayersData { get; private set; }
        public bool[] PlayersReady { get; private set; }

        public LobbyInfoPacket()
        {
            PlayersData = new PlayerData[4];
            PlayersReady = new bool[4];
        }

        public LobbyInfoPacket(
            PlayerData playerData,
            PlayerData[] playersData,
            bool[] playersReady) : base(PacketType.LobbyInfo, playerData)
        {
            PlayersData = playersData;
            PlayersReady = playersReady;
        }

        public new byte[] Serialize()
        {
            base.Serialize();

            for (int i = 0; i < PlayersData.Length; i++)
            {
                binaryWriter.Write(PlayersData[i].ID);
                binaryWriter.Write(PlayersData[i].Name);
            }

            for (int i = 0; i < PlayersReady.Length; i++)
                binaryWriter.Write(PlayersReady[i]);

            return serializeMemoryStream.ToArray();
        }

        public new LobbyInfoPacket Deserialize(byte[] buffer)
        {
            base.Deserialize(buffer);

            for (int i = 0; i < PlayersData.Length; i++)
                PlayersData[i] = new PlayerData(binaryReader.ReadString(), binaryReader.ReadString());

            for (int i = 0; i < PlayersReady.Length; i++)
                PlayersReady[i] = binaryReader.ReadBoolean();

            return this;
        }
    }
}