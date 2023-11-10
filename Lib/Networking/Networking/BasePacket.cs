using System.IO;

namespace Networking
{
    public class BasePacket
    {
        protected MemoryStream serializeMemoryStream { get; private set; }
        protected BinaryWriter binaryWriter { get; private set; }
        protected MemoryStream deserializeMemoryStream { get; private set; }
        protected BinaryReader binaryReader { get; private set; }

        public enum PacketType
        {
            Unknown = -1,
            None,
            Position,
            Instantiate,
            Destroy,

            PlayerJoinedLobby,
            LobbyInfo,
            PlayerReadyStatus
        }

        public PacketType packetType { get; private set; }
        public PlayerData playerData { get; private set; }

        public BasePacket()
        {
            packetType = PacketType.Unknown;
            playerData = new PlayerData("", "");
        }

        public BasePacket(PacketType packetType, PlayerData playerData)
        {
            this.packetType = packetType;
            this.playerData = playerData;
        }

        protected void Serialize()
        {
            serializeMemoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(serializeMemoryStream);

            binaryWriter.Write((int)packetType);
            binaryWriter.Write(playerData.ID);
            binaryWriter.Write(playerData.Name);

        }

        public BasePacket Deserialize(byte[] buffer)
        {
            deserializeMemoryStream = new MemoryStream(buffer);
            binaryReader = new BinaryReader(deserializeMemoryStream);

            packetType = (PacketType)binaryReader.ReadInt32();
            playerData = new PlayerData(binaryReader.ReadString(), binaryReader.ReadString());

            return this;
        }
    }
}