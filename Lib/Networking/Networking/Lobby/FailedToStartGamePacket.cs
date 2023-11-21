namespace Networking.Lobby
{
    public class FailedToStartGamePacket : BasePacket
    {
        public enum ErrorCode
        {
            Unknown = -1,
            None = 0,
            YoureNotHost = 1
        }
        public ErrorCode PacketErrorCode { get; private set; }

        public FailedToStartGamePacket()
        {
            PacketErrorCode = ErrorCode.Unknown;
        }

        public FailedToStartGamePacket(PlayerData playerData) :
            base(PacketType.FailedToStartGame, playerData)
        {
        }

        public byte[] Serialize()
        {
            BeginSerialize();

            binaryWriter.Write((int)PacketErrorCode);

            EndSerialize();

            return serializeMemoryStream.ToArray();
        }

        public new FailedToStartGamePacket Deserialize(byte[] buffer)
        {
            base.Deserialize(buffer);

            PacketErrorCode = (ErrorCode)binaryReader.ReadInt32();

            return this;
        }
    }
}
