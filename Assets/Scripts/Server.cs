using UnityEngine;
using System.Net.Sockets;
using System.Net;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    [SerializeField] ushort port;
    Socket serverSocket;
    Socket client;
    PlayerData playerData;
    bool clientConnected = false;

    void Start()
    {
        playerData = new PlayerData("SERVER_ID", "SERVER");

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
                    InstantiatePacket ip = new InstantiatePacket(
                        playerData,
                        "Player/Prefab/Player",
                        new Vector3(5, 4, -9),
                        Quaternion.Euler(45, 0, 0));

                    client.Send(ip.Serialize());

                    Debug.LogError($"[Server] Sent Instantiate Packet to client with player ID of {ip.playerData.ID}");
                    Debug.LogError($"[Server] And player Name of {ip.playerData.Name}");
                    Debug.LogError($"[Server] And prefabName of {ip.PrefabName}");
                    Debug.LogError($"[Server] And position of {ip.Position}");
                    Debug.LogError($"[Server] And rotation of {ip.Rotation.eulerAngles}");
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