using Networking;
using Networking.Lobby;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    public List<LobbyPlayerData> lobbyPlayerData;

    private void Awake()
    {
        lobbyPlayerData = new List<LobbyPlayerData>(4);
    }

    public bool TryAddPlayerToLobby(PlayerData playerData)
    {
        bool playerAdded = false;
        bool isPlayerHost = false;

        if (lobbyPlayerData.Count < 1)
            isPlayerHost = true;

        if (lobbyPlayerData.Count < 4)
        {
            lobbyPlayerData.Add(new LobbyPlayerData(playerData, isPlayerHost, false));
            playerAdded = true;
        }

        return playerAdded;
    }
}