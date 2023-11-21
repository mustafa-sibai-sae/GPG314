using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using Networking;
using Networking.Lobby;
using System.Linq;
using System.Text;
using Unity.VisualScripting;

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
                clients[i].Send(new PositionPacket(
                    PlayerInformation.Instance.PlayerData,
                    new Vector3(1, 2, 3)).Serialize());

                clients[i].Send(new PositionPacket(
                    PlayerInformation.Instance.PlayerData,
                    new Vector3(7, 8, 9)).Serialize());

                clients[i].Send(new PositionPacket(
                    PlayerInformation.Instance.PlayerData,
                    new Vector3(20, 30, 40)).Serialize());
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