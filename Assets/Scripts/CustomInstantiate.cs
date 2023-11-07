using UnityEngine;

public class CustomInstantiate
{
    public static GameObject Instantiate(string PrefabName, Vector3 Position, Quaternion Rotation)
    {
        GameObject prefabToInstantiate = Resources.Load<GameObject>(PrefabName);
        return Object.Instantiate(prefabToInstantiate, Position, Rotation);
    }
}