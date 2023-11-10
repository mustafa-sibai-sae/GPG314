using Networking;
using Networking.Lobby;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientNetworkManager : ClientDelegate
{
    [SerializeField] ushort port;

    Socket socket;
    bool clientConnected = false;

    public static ClientNetworkManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    void Update()
    {
        if (clientConnected)
        {
            if (socket.Available > 0)
            {
                try
                {
                    byte[] buffer = new byte[socket.Available];
                    socket.Receive(buffer);

                    BasePacket bp = new BasePacket();
                    bp.Deserialize(buffer);

                    Debug.LogWarning($"[Client] Received a packet from the server");
                    Debug.LogWarning($"[Client] Packet Type is {bp.packetType}");
                    Debug.LogWarning($"[Client] Player ID of {bp.playerData.ID} and player name of {bp.playerData.Name}");

                    switch (bp.packetType)
                    {
                        case BasePacket.PacketType.LobbyInfo:
                            LobbyInfoPacket lip = new LobbyInfoPacket().Deserialize(buffer);
                            Debug.LogWarning($"[Client] Lobby Info Packet");

                            if (LobbyInfoReceivedEvent != null)
                                LobbyInfoReceivedEvent(lip);

                            break;

                        case BasePacket.PacketType.PlayerJoinedLobby:
                            PlayerJoinedLobbyPacket pjlp = new PlayerJoinedLobbyPacket().Deserialize(buffer);
                            Debug.LogWarning($"[Client] Player Joined Lobby Packet Content is: {pjlp.playerData.ID}");
                            Debug.LogWarning($"[Client] Player Joined Lobby Packet Content is: {pjlp.playerData.Name}");

                            if (ClientJoinedLobbyEvent != null)
                                ClientJoinedLobbyEvent();

                            break;

                        case BasePacket.PacketType.StartGame:
                            StartGamePacket sgp = new StartGamePacket().Deserialize(buffer);
                            Debug.LogWarning($"[Client] Start Game Packet");
                            Debug.LogWarning($"[Client] Start Game Packet");

                            if (StartGameEvent != null)
                                StartGameEvent();

                            break;

                        case BasePacket.PacketType.Position:

                            PositionPacket pp = new PositionPacket().Deserialize(buffer);
                            Debug.LogWarning($"[Client] Position Packet Content is: {pp.Position}");
                            break;

                        case BasePacket.PacketType.Instantiate:

                            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
                            InstantiatePacket ip = new InstantiatePacket().Deserialize(buffer);
                            Debug.LogWarning($"[Client] Instantiate Packet Content is: {ip.PrefabName}");
                            Debug.LogWarning($"[Client] Instantiate Packet Content is: {ip.Position}");
                            Debug.LogWarning($"[Client] Instantiate Packet Content is: {ip.Rotation.eulerAngles}");

                            GameObject go = Utils.InstantiateFromNetwork(ip);

                            break;


                        case BasePacket.PacketType.Destroy:

                            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
                            DestroyPacket dp = new DestroyPacket().Deserialize(buffer);
                            Debug.LogWarning($"[Client] Destroy Packet Content is: {dp.GameObjectID}");
                            Utils.DestroyFromNetwork(dp);

                            break;
                    }
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                    {
                        Debug.LogWarning($"[Client] {ex}");
                    }
                }
            }
        }
    }

    public void Connect(string serverIPv4Address)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Debug.LogWarning("[Client] Connecting to server...");
        socket.Connect(new IPEndPoint(IPAddress.Parse(serverIPv4Address), port));
        clientConnected = true;
        Debug.LogWarning("[Client] Connected to server!");

        if (ClientConnectedToServerEvent != null)
            ClientConnectedToServerEvent();
    }

    public void SendPacket(byte[] buffer)
    {
        socket.Send(buffer);
    }
}