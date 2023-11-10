using Networking.Lobby;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] List<LobbyPlayerData> lobbyPlayerData;
        [SerializeField] LobbyUI lobbyUI;
        int currentLobbyPlayerIndex = -1;

        void Start()
        {
            ClientNetworkManager.Instance.LobbyInfoReceivedEvent += OnLobbyInfoReceived;
            ClientNetworkManager.Instance.StartGameEvent += OnStartGame;

            lobbyUI.ReadyButton.onClick.AddListener(ReadyButtonPressed);
            lobbyUI.StartGameButton.onClick.AddListener(StartGame);

            ClientNetworkManager.Instance.SendPacket(
                new PlayerJoinedLobbyPacket(PlayerInformation.Instance.PlayerData).Serialize());
        }

        void Update()
        {

        }

        void ReadyButtonPressed()
        {
            LobbyPlayerData currentPlayer = lobbyPlayerData[currentLobbyPlayerIndex];
            currentPlayer.IsReady = currentPlayer.IsReady ? false : true;

            ClientNetworkManager.Instance.SendPacket(new PlayerReadyStatusPacket(
                PlayerInformation.Instance.PlayerData,
                currentPlayer.IsReady).Serialize());
        }

        void OnLobbyInfoReceived(LobbyInfoPacket lobbyInfoPacket)
        {
            lobbyPlayerData = lobbyInfoPacket.LobbyPlayerData;

            for (int i = 0; i < lobbyPlayerData.Count; i++)
            {
                if (lobbyPlayerData[i].PlayerData.Name == PlayerInformation.Instance.PlayerData.Name)
                    currentLobbyPlayerIndex = i;

                lobbyUI.UpdateUI(lobbyPlayerData);
            }
        }

        void StartGame()
        {
            bool allPlayersReady = true;

            for (int i = 0; i < lobbyPlayerData.Count; i++)
            {
                if (!lobbyPlayerData[i].IsReady)
                {
                    allPlayersReady = false;
                    break;
                }
            }

            if(!allPlayersReady)
            {
                Debug.LogError("Cannot start game because not all players are ready");
                return;
            }

            ClientNetworkManager.Instance.SendPacket(new StartGamePacket(
                PlayerInformation.Instance.PlayerData).Serialize());
        }

        void OnStartGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}