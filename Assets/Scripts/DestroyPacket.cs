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

    public new byte[] Serialize()
    {
        base.Serialize();

        binaryWriter.Write(GameObjectID);

        return serializeMemoryStream.ToArray();
    }

    public new DestroyPacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);

        GameObjectID = binaryReader.ReadString();

        return this;
    }
}