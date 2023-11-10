using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] TMP_InputField serverAddressInputField;
    [SerializeField] Button connectButton;

    void Start()
    {
        ClientNetworkManager.Instance.ClientConnectedToServerEvent += OnClientConnectedToServer;
        connectButton.onClick.AddListener(() => ClientNetworkManager.Instance.Connect(serverAddressInputField.text));
    }

    void OnClientConnectedToServer()
    {
        PlayerInformation.Instance.SetPlayerName(playerNameInputField.text);
        SceneManager.LoadScene("Lobby");
    }
}