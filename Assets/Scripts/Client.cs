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
            try
            {
                byte[] buffer = new byte[1024];
                client.Receive(buffer);

                Debug.LogWarning("Received a message from the server!");
                Debug.LogWarning($"[Client] {Encoding.ASCII.GetString(buffer)}");
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