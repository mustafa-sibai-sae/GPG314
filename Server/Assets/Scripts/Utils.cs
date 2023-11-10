using Networking;
using System.Net.Sockets;
using UnityEngine;

public class Utils
{
    public static GameObject InstantiateFromNetwork(InstantiatePacket instantiatePacket)
    {
        GameObject prefabToInstantiate = Resources.Load<GameObject>(instantiatePacket.PrefabName);
        GameObject go = Object.Instantiate(
            prefabToInstantiate,
            instantiatePacket.Position,
            instantiatePacket.Rotation);

        NetworkComponent nc = go.GetComponent<NetworkComponent>();
        nc.SetNetworkComponentData(instantiatePacket.playerData.ID, instantiatePacket.GameObjectID);

        return go;
    }

    public static GameObject InstantiateOverNetwork(
        Socket socket,
        PlayerData playerData,
        string PrefabName,
        Vector3 Position,
        Quaternion Rotation)
    {
        GameObject prefabToInstantiate = Resources.Load<GameObject>(PrefabName);
        GameObject go = Object.Instantiate(prefabToInstantiate, Position, Rotation);
        NetworkComponent nc = go.GetComponent<NetworkComponent>();
        nc.SetNetworkComponentData(playerData.ID, System.Guid.NewGuid().ToString());

        InstantiatePacket ip = new InstantiatePacket(
            playerData,
            nc.GameObjectID,
            "Player/Prefab/Player",
            go.transform.position,
            go.transform.rotation);

        socket.Send(ip.Serialize());

        Debug.LogError($"[Server] Sent Instantiate Packet to client with player ID of {ip.playerData.ID}");
        Debug.LogError($"[Server] And player Name of {ip.playerData.Name}");
        Debug.LogError($"[Server] And prefabName of {ip.PrefabName}");
        Debug.LogError($"[Server] And position of {ip.Position}");
        Debug.LogError($"[Server] And rotation of {ip.Rotation.eulerAngles}");

        return go;
    }

    public static void DestoryOverNetwork(
        Socket socket,
        PlayerData playerData,
        GameObject gameObject)
    {
        NetworkComponent nc = gameObject.GetComponent<NetworkComponent>();

        DestroyPacket dp = new DestroyPacket(
            playerData,
            nc.GameObjectID);

        socket.Send(dp.Serialize());

        Debug.LogError($"[Server] Sent Destroy Packet to client with player ID of {dp.playerData.ID}");
        Debug.LogError($"[Server] And player Name of {dp.playerData.Name}");
        Debug.LogError($"[Server] And gameObjectID of {dp.GameObjectID}");

        Object.Destroy(gameObject);
    }

    public static void DestroyFromNetwork(DestroyPacket destroyPacket)
    {
        NetworkComponent[] networkComponents = Object.FindObjectsOfType<NetworkComponent>();

        for (int i = 0; i < networkComponents.Length; i++)
        {
            if (networkComponents[i].GameObjectID == destroyPacket.GameObjectID)
            {
                Object.Destroy(networkComponents[i].gameObject);
                break;
            }
        }
    }
}