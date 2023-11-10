using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using Networking;
using Networking.Lobby;
using System.Linq;

public class Server : MonoBehaviour
{
    [SerializeField] ushort port;
    Socket serverSocket;
    List<Socket> clients;
    bool clientConnected = false;

    public static Server instance;
    GameObject instantiatedGameObject;

    Lobby lobby;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        lobby = FindFirstObjectByType<Lobby>();
        PlayerInformation.Instance.SetPlayerName("SERVER");

        clients = new List<Socket>();

        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        serverSocket.Listen(10);
        serverSocket.Blocking = false;
        Debug.LogError("[Server] Listening for clients to connect");
    }

    void Update()
    {
        try
        {
            clients.Add(serverSocket.Accept());
            clientConnected = true;
            Debug.LogError("[Server] Client connected!");
        }
        catch (SocketException ex)
        {
            if (ex.SocketErrorCode != SocketError.WouldBlock)
            {
                Debug.LogError($"[Server] {ex}");
            }
        }

        if (clientConnected)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Available > 0)
                {
                    try
                    {
                        byte[] buffer = new byte[clients[i].Available];
                        clients[i].Receive(buffer);

                        BasePacket bp = new BasePacket();
                        bp.Deserialize(buffer);

                        Debug.LogError($"[Server] Received a packet from the client");
                        Debug.LogError($"[Server] Packet Type is {bp.packetType}");
                        Debug.LogError($"[Server] Player ID of {bp.playerData.ID} and player name of {bp.playerData.Name}");

                        switch (bp.packetType)
                        {
                            case BasePacket.PacketType.PlayerJoinedLobby:
                                Debug.LogError($"[Server] Received a Player Joined Lobby Packet");

                                PlayerJoinedLobbyPacket pjlp = new PlayerJoinedLobbyPacket().Deserialize(buffer);
                                bool result = lobby.TryAddPlayerToLobby(pjlp.playerData);

                                if (result)
                                {
                                    SendPacketsToAllClients(new LobbyInfoPacket(
                                        PlayerInformation.Instance.PlayerData,
                                        lobby.lobbyPlayerData).Serialize());
                                }
                                else
                                {
                                    //Send lobby full packet
                                }

                                break;

                            case BasePacket.PacketType.StartGame:
                                Debug.LogError($"[Server] Received a Start Game Packet");

                                StartGamePacket sgp = new StartGamePacket().Deserialize(buffer);

                                for (int j = 0; j < lobby.lobbyPlayerData.Count; j++)
                                {
                                    if (lobby.lobbyPlayerData[j].PlayerData.Name == sgp.playerData.Name)
                                    {
                                        if (lobby.lobbyPlayerData[j].IsHost)
                                        {
                                            bool allPlayersAreReady = true;

                                            for (int k = 0; k < lobby.lobbyPlayerData.Count; k++)
                                            {
                                                if (!lobby.lobbyPlayerData[k].IsReady)
                                                {
                                                    allPlayersAreReady = false;
                                                    break;
                                                }
                                            }

                                            if (!allPlayersAreReady)
                                            {
                                                //Send failed to start game packet because not all players are ready
                                            }
                                            else
                                            {
                                                SendPacketsToAllClients(new StartGamePacket(
                                                    PlayerInformation.Instance.PlayerData).Serialize());
                                            }
                                        }
                                        else
                                        {
                                            //Send failed to start game packet because player starting game is not host
                                        }
                                        break;
                                    }
                                }
                                break;

                            case BasePacket.PacketType.PlayerReadyStatus:
                                Debug.LogError($"[Server] Received Player Ready Status Packet");

                                PlayerReadyStatusPacket prsp = new PlayerReadyStatusPacket().Deserialize(buffer);

                                for (int j = 0; j < lobby.lobbyPlayerData.Count; j++)
                                {
                                    if (lobby.lobbyPlayerData[j].PlayerData.ID == prsp.playerData.ID)
                                    {
                                        lobby.lobbyPlayerData[j].IsReady = prsp.IsPlayerRead;
                                        break;
                                    }
                                }

                                SendPacketsToAllClients(new LobbyInfoPacket(
                                    PlayerInformation.Instance.PlayerData,
                                    lobby.lobbyPlayerData).Serialize());

                                break;

                                /*case BasePacket.PacketType.Position:

                                    PositionPacket pp = new PositionPacket().Deserialize(buffer);
                                    Debug.LogWarning($"[Client] Position Packet Content is: {pp.Position}");
                                    break;*/

                                /*case BasePacket.PacketType.Instantiate:

                                    InstantiatePacket ip = new InstantiatePacket().Deserialize(buffer);
                                    Debug.LogError($"[Server] Instantiate Packet Content is: {ip.PrefabName}");
                                    Debug.LogError($"[Server] Instantiate Packet Content is: {ip.Position}");
                                    Debug.LogError($"[Server] Instantiate Packet Content is: {ip.Rotation.eulerAngles}");

                                    GameObject go = Utils.InstantiateFromNetwork(ip);
                                    SendPacketsToAllOtherClients(i, buffer);

                                    break;*/


                                /*case BasePacket.PacketType.Destroy:

                                    DestroyPacket dp = new DestroyPacket().Deserialize(buffer);
                                    Debug.LogError($"[Server] Destroy Packet Content is: {dp.GameObjectID}");
                                    Utils.DestroyFromNetwork(dp);
                                    SendPacketsToAllOtherClients(i, buffer);

                                    break;*/
                        }
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode != SocketError.WouldBlock)
                        {
                            Debug.LogError($"[Server] {ex}");
                        }
                    }
                }
            }

        }
    }

    void SendPacketsToAllOtherClients(int currnetClientIndex, byte[] buffer)
    {
        for (int i = 0; i < clients.Count; i++)
        {
            if (i == currnetClientIndex)
                continue;

            clients[i].Send(buffer);
        }
    }

    void SendPacketsToAllClients(byte[] buffer)
    {
        for (int i = 0; i < clients.Count; i++)
        {
            clients[i].Send(buffer);
        }
    }
}


/*
 * 

        if (clientConnected)
        {
            try
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    PositionPacket ps = new PositionPacket(playerData, new Vector3(1, 2, 3));
    client.Send(ps.Serialize());

                    Debug.LogError($"[Server] Sent Position Packet to client with player ID of {ps.playerData.ID}");
                    Debug.LogError($"[Server] And player Name of {ps.playerData.Name}");
                    Debug.LogError($"[Server] And position of {ps.Position}");
                }

if (Input.GetKeyDown(KeyCode.W))
{
    SceneManager.SetActiveScene(SceneManager.GetSceneByName("Server"));
    instantiatedGameObject = Utils.InstantiateOverNetwork(
        client,
        playerData,
        "Player/Prefab/Player",
        new Vector3(5, 4, -9),
        Quaternion.Euler(45, 0, 0));
}

if (Input.GetKeyDown(KeyCode.E))
{
    SceneManager.SetActiveScene(SceneManager.GetSceneByName("Server"));
    Utils.DestoryOverNetwork(
        client,
        playerData,
        instantiatedGameObject);
}
            }
            catch (SocketException ex)
            {
    if (ex.SocketErrorCode != SocketError.WouldBlock)
    {
        Debug.LogError($"[Server] {ex}");
    }
}
        }
    }
*/