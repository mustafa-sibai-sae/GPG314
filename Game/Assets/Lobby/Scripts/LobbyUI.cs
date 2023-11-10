using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] PlayerUI[] playersUI;
        public Button ReadyButton;

        void Start()
        {
        }

        void Update()
        {

        }

        public void UpdateUI(PlayerData[] playersData, bool[] playersReady)
        {
            for (int i = 0; i < playersUI.Length; i++)
            {
                playersUI[i].SetPlayerName(playersData[i].Name);
                playersUI[i].ChangePlayerReadyStatus(playersReady[i]);
            }
        }
    }
}