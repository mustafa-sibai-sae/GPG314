using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] TMP_Text playerName;
        [SerializeField] Image readyImage;

        void Start()
        {
            SetPlayerName("");
            ChangePlayerReadyStatus(false);
        }

        public void SetPlayerName(string name)
        {
            playerName.text = name;
        }

        public void ChangePlayerReadyStatus(bool ready)
        {
            if (ready)
                readyImage.color = new Color(0, 1, 0, 1);
            else
                readyImage.color = new Color(1, 0, 0, 1);
        }
    }
}