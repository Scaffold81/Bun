using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class UIPlayerNameInputField : MonoBehaviour
    {
        const string playerNamePrefKey = "PlayerName";

        private TMP_InputField _inputField;

        private void Start()
        {
            var defaultName = string.Empty;
             _inputField = this.GetComponent<TMP_InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerName(string value)
        {
            
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
    }
}
