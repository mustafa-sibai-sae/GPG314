using System;
using UnityEngine;

namespace Networking
{
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

        public byte[] Serialize()
        {
            BeginSerialize();

            binaryWriter.Write(Position.x);
            binaryWriter.Write(Position.y);
            binaryWriter.Write(Position.z);

            EndSerialize();

            return serializeMemoryStream.ToArray();
        }

        public new PositionPacket Deserialize(byte[] buffer)
        {
            base.Deserialize(buffer);

            Position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
            return this;
        }
    }
}