namespace Networking.Lobby
{
    public class PlayerReadyStatusPacket : BasePacket
    {
        public bool IsPlayerRead { get; private set; }

        public PlayerReadyStatusPacket()
        {
        }

        public PlayerReadyStatusPacket(
            PlayerData playerData,
            bool isPlayerRead) : base(PacketType.PlayerReadyStatus, playerData)
        {
            IsPlayerRead = isPlayerRead;
        }

        public new byte[] Serialize()
        {
            base.Serialize();

            binaryWriter.Write(IsPlayerRead);

            return serializeMemoryStream.ToArray();
        }

        public new PlayerReadyStatusPacket Deserialize(byte[] buffer)
        {
            base.Deserialize(buffer);

            IsPlayerRead = binaryReader.ReadBoolean();

            return this;
        }
    }
}