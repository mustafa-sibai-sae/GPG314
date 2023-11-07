using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] string serverIPv4Address;
    [SerializeField] ushort port;
    Socket client;
    bool clientConnected = false;

    void Start()
    {
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Debug.LogWarning("[Client] Connecting to server...");
        client.Connect(new IPEndPoint(IPAddress.Parse(serverIPv4Address), port));
        clientConnected = true;
        Debug.LogWarning("[Client] Connected to server!");
    }

    void Update()
    {
        if (clientConnected)
        {
            if (client.Available > 0)
            {
                try
                {
                    byte[] buffer = new byte[client.Available];
                    client.Receive(buffer);

                    BasePacket bp = new BasePacket();
                    bp.Deserialize(buffer);

                    Debug.LogWarning($"[Client] Received a packet from the server");
                    Debug.LogWarning($"[Client] Packet Type is {bp.packetType}");
                    Debug.LogWarning($"[Client] Player ID of {bp.playerData.ID} and player name of {bp.playerData.Name}");

                    switch (bp.packetType)
                    {
                        case BasePacket.PacketType.Position:

                            PositionPacket pp = new PositionPacket().Deserialize(buffer);
                            Debug.LogWarning($"[Client] Position Packet Content is: {pp.Position}");
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
}