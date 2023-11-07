using UnityEngine;

public class NetworkComponent : MonoBehaviour
{
    [field: SerializeField] public string GameObjectID { get; private set; }
    [field: SerializeField] public string OwnerID { get; private set; }

    public void SetNetworkComponentData(string OwnerID, string GameObjectID)
    {
        this.OwnerID = OwnerID;
        this.GameObjectID = GameObjectID;
    }
}