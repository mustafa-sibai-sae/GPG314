using UnityEngine;

public class PositionPacket : BasePacket
{
    public Vector3 Position { get; private set; }

    public PositionPacket()
    {
        Position = Vector3.zero;
    }

    public PositionPacket(PlayerData playerData, Vector3 position) : base(PacketType.Position, playerData)
    {
        Position = position;
    }

    public new byte[] Serialize()
    {
        base.Serialize();

        binaryWriter.Write(Position.x);
        binaryWriter.Write(Position.y);
        binaryWriter.Write(Position.z);

        return serializeMemoryStream.ToArray();
    }

    public new PositionPacket Deserialize(byte[] buffer)
    {
        base.Deserialize(buffer);

        Position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        return this;
    }
}