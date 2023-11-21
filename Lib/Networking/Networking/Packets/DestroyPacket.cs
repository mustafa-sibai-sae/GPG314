namespace Networking
{
    public class DestroyPacket : BasePacket
    {
        public string GameObjectID { get; private set; }

        public DestroyPacket()
        {
            GameObjectID = "";
        }

        public DestroyPacket(
            PlayerData playerData,
            string gameObjectID) : base(PacketType.Destroy, playerData)
        {
            GameObjectID = gameObjectID;
        }

        public byte[] Serialize()
        {
            BeginSerialize();

            binaryWriter.Write(GameObjectID);

            EndSerialize();

            return serializeMemoryStream.ToArray();
        }

        public new DestroyPacket Deserialize(byte[] buffer)
        {
            base.Deserialize(buffer);

            GameObjectID = binaryReader.ReadString();

            return this;
        }
    }
}