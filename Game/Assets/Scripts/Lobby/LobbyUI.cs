using Networking.Lobby;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] PlayerUI[] playersUI;
        public Button ReadyButton;
        public Button StartGameButton;

        void Start()
        {
            StartGameButton.gameObject.SetActive(false);
        }

        public void UpdateUI(List<LobbyPlayerData> lobbyPlayerData)
        {
            for (int i = 0; i < lobbyPlayerData.Count; i++)
            {
                playersUI[i].UpdateUI(
                    lobbyPlayerData[i].PlayerData.Name,
                    lobbyPlayerData[i].IsHost,
                    lobbyPlayerData[i].IsReady);

                if (lobbyPlayerData[i].PlayerData.Name == PlayerInformation.Instance.PlayerData.Name)
                {
                    if (lobbyPlayerData[i].IsHost)
                    {
                        StartGameButton.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}