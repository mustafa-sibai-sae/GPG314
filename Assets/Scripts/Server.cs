using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    [SerializeField] ushort port;
    Socket serverSocket;
    Socket client;
    bool clientConnected = false;

    void Start()
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        serverSocket.Listen(10);
        serverSocket.Blocking = false;
        Debug.LogError("[Server] Listening for clients to connect");
        SceneManager.LoadScene("Client", LoadSceneMode.Additive);
    }

    void Update()
    {
        try
        {
            client = serverSocket.Accept();
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
            try
            {
                client.Send(Encoding.ASCII.GetBytes("Hello World! I am the server!"));
                Debug.LogError("[Server] Sent message to client!");
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