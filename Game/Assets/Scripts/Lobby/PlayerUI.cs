using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] Image hostImage;
        [SerializeField] TMP_Text playerName;
        [SerializeField] Image readyImage;

        void Start()
        {
            UpdateUI("", false, false);
        }

        public void UpdateUI(string name, bool isHost, bool isRady)
        {
            playerName.text = name;

            if (isHost)
                hostImage.enabled = true;
            else
                hostImage.enabled = false;

            if (isRady)
                readyImage.color = new Color(0, 1, 0, 1);
            else
                readyImage.color = new Color(1, 0, 0, 1);
        }
    }
}