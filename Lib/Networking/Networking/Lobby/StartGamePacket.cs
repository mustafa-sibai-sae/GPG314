namespace Networking.Lobby
{
    public class StartGamePacket : BasePacket
    {
        public StartGamePacket()
        {
        }

        public StartGamePacket(PlayerData playerData) :
            base(PacketType.StartGame, playerData)
        {
        }

        public byte[] Serialize()
        {
            BeginSerialize();

            EndSerialize();

            return serializeMemoryStream.ToArray();
        }

        public new StartGamePacket Deserialize(byte[] buffer)
        {
            base.Deserialize(buffer);

            return this;
        }
    }
}