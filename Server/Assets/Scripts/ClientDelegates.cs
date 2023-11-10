using UnityEngine;

public class ClientDelegate : MonoBehaviour
{
    public delegate void ClientConnectedToServer();
    public ClientConnectedToServer ClientConnectedToServerEvent;

    public delegate void ClientJoinedLobby();
    public ClientJoinedLobby ClientJoinedLobbyEvent;
}