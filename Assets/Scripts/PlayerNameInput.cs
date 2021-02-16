using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonChat.Menus
{
    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] private InputField nameInputField = null;
        [SerializeField] private Button continueButton = null;
        [SerializeField] private GameObject inputPanel = null;
        [SerializeField] private GameObject findOpponentPanel = null;

        private const string PlayerPrefsNameKey = "PlayerName";

        private void Start() => SetUpInpuField();

        private void SetUpInpuField()
        {
            if(!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return;  }

            string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

            nameInputField.text = defaultName;

            SetPlayerName(defaultName);
        }

        public void SetPlayerName(string name)
        {
            continueButton.interactable = !string.IsNullOrEmpty(name);
        }

        public void SavePlayerName()
        {
            string playerName = nameInputField.text;

            PhotonNetwork.NickName = playerName;

            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
            inputPanel.SetActive(false);
            findOpponentPanel.SetActive(true);
        }
    }
}

