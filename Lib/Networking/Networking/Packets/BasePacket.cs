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
            PlayerReadyStatus,

            StartGame,
            FailedToStartGame
        }

        public ushort packetSize { get; private set; }
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

        protected void BeginSerialize()
        {
            serializeMemoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(serializeMemoryStream);

            binaryWriter.Write(packetSize);
            binaryWriter.Write((int)packetType);
            binaryWriter.Write(playerData.ID);
            binaryWriter.Write(playerData.Name);
        }

        protected void EndSerialize()
        {
            packetSize = (ushort)serializeMemoryStream.ToArray().Length;
            long currentPosition = serializeMemoryStream.Seek(-packetSize, SeekOrigin.Current);

            if (currentPosition == 0)
            {
                //2 is the size of the ushort packetSize. Figure it out
                binaryWriter.Write(packetSize);
            }
        }

        public BasePacket Deserialize(byte[] buffer)
        {
            deserializeMemoryStream = new MemoryStream(buffer);
            binaryReader = new BinaryReader(deserializeMemoryStream);

            packetSize = binaryReader.ReadUInt16();
            packetType = (PacketType)binaryReader.ReadInt32();
            playerData = new PlayerData(binaryReader.ReadString(), binaryReader.ReadString());

            return this;
        }
    }
}