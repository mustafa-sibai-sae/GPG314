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

    public static Server instance;
    GameObject instantiatedGameObject;

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
}