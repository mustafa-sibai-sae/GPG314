using Networking.Lobby;
using UnityEngine;

namespace Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] LobbyPlayerData[] lobbyPlayerData;
        [SerializeField] LobbyUI lobbyUI;
        int currentLobbyPlayerIndex = -1;

        void Start()
        {
            ClientNetworkManager.Instance.LobbyInfoReceivedEvent += OnLobbyInfoReceived;
            lobbyUI.ReadyButton.onClick.AddListener(ReadyButtonPressed);

            ClientNetworkManager.Instance.SendPacket(
                new PlayerJoinedLobbyPacket(PlayerInformation.Instance.PlayerData).Serialize());
        }

        void Update()
        {

        }

        void ReadyButtonPressed()
        {
            LobbyPlayerData currentPlayer = lobbyPlayerData[currentLobbyPlayerIndex];
            currentPlayer.IsPlayerReady = currentPlayer.IsPlayerReady ? false : true;

            ClientNetworkManager.Instance.SendPacket(new PlayerReadyStatusPacket(
                PlayerInformation.Instance.PlayerData,
                currentPlayer.IsPlayerReady).Serialize());
        }

        void OnLobbyInfoReceived(LobbyInfoPacket lobbyInfoPacket)
        {
            for (int i = 0; i < lobbyInfoPacket.PlayersData.Length; i++)
            {
                lobbyPlayerData[i].PlayerData = lobbyInfoPacket.PlayersData[i];
                lobbyPlayerData[i].IsPlayerReady = lobbyInfoPacket.PlayersReady[i];

                if (lobbyPlayerData[i].PlayerData.Name == PlayerInformation.Instance.PlayerData.Name)
                    currentLobbyPlayerIndex = i;

                lobbyUI.UpdateUI(lobbyInfoPacket.PlayersData, lobbyInfoPacket.PlayersReady);
            }
        }
    }
}