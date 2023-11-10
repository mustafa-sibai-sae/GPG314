namespace Networking.Lobby
{
    public class PlayerJoinedLobbyPacket : BasePacket
    {
        public PlayerJoinedLobbyPacket()
        {
        }

        public PlayerJoinedLobbyPacket(PlayerData playerData) : base(PacketType.PlayerJoinedLobby, playerData)
        {
        }

        public new byte[] Serialize()
        {
            base.Serialize();

            return serializeMemoryStream.ToArray();
        }

        public new PlayerJoinedLobbyPacket Deserialize(byte[] buffer)
        {
            base.Deserialize(buffer);

            return this;
        }
    }
}