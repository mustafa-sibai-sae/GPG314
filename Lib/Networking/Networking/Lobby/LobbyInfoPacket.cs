using System.Collections.Generic;

namespace Networking.Lobby
{
    public class LobbyInfoPacket : BasePacket
    {
        public List<LobbyPlayerData> LobbyPlayerData { get; private set; }

        public LobbyInfoPacket()
        {
            LobbyPlayerData = new List<LobbyPlayerData>();
        }

        public LobbyInfoPacket(
            PlayerData playerData,
            List<LobbyPlayerData> lobbyPlayerData) : base(PacketType.LobbyInfo, playerData)
        {
            LobbyPlayerData = lobbyPlayerData;
        }

        public new byte[] Serialize()
        {
            base.Serialize();

            binaryWriter.Write(LobbyPlayerData.Count);

            for (int i = 0; i < LobbyPlayerData.Count; i++)
            {
                binaryWriter.Write(LobbyPlayerData[i].PlayerData.ID);
                binaryWriter.Write(LobbyPlayerData[i].PlayerData.Name);
                binaryWriter.Write(LobbyPlayerData[i].IsHost);
                binaryWriter.Write(LobbyPlayerData[i].IsReady);
            }

            return serializeMemoryStream.ToArray();
        }

        public new LobbyInfoPacket Deserialize(byte[] buffer)
        {
            base.Deserialize(buffer);

            int totalPlayersInLobby = binaryReader.ReadInt32();

            LobbyPlayerData = new List<LobbyPlayerData>(totalPlayersInLobby);

            for (int i = 0; i < totalPlayersInLobby; i++)
            {
                LobbyPlayerData.Add(
                    new Lobby.LobbyPlayerData(
                        new PlayerData(binaryReader.ReadString(), binaryReader.ReadString()),
                        binaryReader.ReadBoolean(),
                        binaryReader.ReadBoolean()));
            }

            return this;
        }
    }
}