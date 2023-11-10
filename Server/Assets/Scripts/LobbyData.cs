using Networking;
using UnityEngine;

public class LobbyData : MonoBehaviour
{
    public PlayerData[] PlayersData = new PlayerData[4];
    public bool[] PlayersReady = new bool[4];

    private void Awake()
    {
        PlayersData = new PlayerData[4];

        for (int i = 0; i < PlayersData.Length; i++)
            PlayersData[i] = new PlayerData("", "");

        PlayersReady = new bool[4];
    }

    public bool TryAddPlayerToLobby(PlayerData playerData)
    {
        bool playerAdded = false;

        for (int i = 0; i < PlayersData.Length; i++)
        {
            if (PlayersData[i].Name == "")
            {
                PlayersData[i] = playerData;
                playerAdded = true;
                break;
            }
        }

        return playerAdded;
    }
}