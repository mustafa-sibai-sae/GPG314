using UnityEngine;

public class InstantiatePacket : BasePacket
{
    public string GameObjectID { get; private set; }
    public string PrefabName { get; private set; }
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }

    public InstantiatePacket()
    {
        GameObjectID = "";
        PrefabName = "";
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
    }

    public InstantiatePacket(
        PlayerData playerData,
        string gameObjectID,
        string prefabName,
        Vector3 position,
        Quaternion rotation) : base(PacketType.Instantiate, playerData)
    {
        GameObjectID = gameObjectID;
        PrefabName = prefabName;
        Position = position;
        Rotation = rotation;
    }

    public new byte[] Serialize()
    {
        base.Serialize();

        binaryWriter.Write(GameObjectID);

        binaryWriter.Write(PrefabName);

        binaryWriter.Write(Position.x);
        binaryWriter.Write(Position.y);
        binaryWriter.Write(Position.z);

        binaryWriter.Write(Rotation.x);
        binaryWriter.Write(Rotation.y);
        binaryWriter.Write(Rotation.z);
        binaryWriter.Write(Rotation.w);

        return serializeMemoryStream.ToArray();
    }

    public new InstantiatePacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);

        GameObjectID = binaryReader.ReadString();

        PrefabName = binaryReader.ReadString();

        Position = new Vector3(
            binaryReader.ReadSingle(),
            binaryReader.ReadSingle(),
            binaryReader.ReadSingle());

        Rotation = new Quaternion(
            binaryReader.ReadSingle(),
            binaryReader.ReadSingle(),
            binaryReader.ReadSingle(),
            binaryReader.ReadSingle());

        return this;
    }
}