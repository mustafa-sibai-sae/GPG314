using Networking;
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
        Connect("127.0.0.1");
    }

    void Update()
    {
        if (clientConnected)
        {
            if (socket.Available > 0)
            {
                try
                {
                    int totalSize = socket.Available;

                    byte[] buffer = new byte[socket.Available];
                    socket.Receive(buffer);

                    while(totalSize > 0)
                    {
                        BasePacket bp = new BasePacket();
                        bp.Deserialize(buffer);

                        switch (bp.packetType)
                        {
                            case BasePacket.PacketType.Position:

                                PositionPacket pp = new PositionPacket().Deserialize(buffer);
                                Debug.LogWarning($"[Client] Position Packet Content is: {pp.Position}");
                                break;
                        }

                        totalSize -= bp.packetSize;
                        
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