using Networking.Lobby;
using UnityEngine;

namespace Networking
{
    public class ClientDelegate : MonoBehaviour
    {
        public delegate void ClientConnectedToServer();
        public ClientConnectedToServer ClientConnectedToServerEvent;

        public delegate void ClientJoinedLobby();
        public ClientJoinedLobby ClientJoinedLobbyEvent;

        public delegate void LobbyInfoReceived(LobbyInfoPacket lobbyInfoPacket);
        public LobbyInfoReceived LobbyInfoReceivedEvent;
    }
}