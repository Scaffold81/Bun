using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Network.Lobby.UI
{
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants
        const string playerNamePrefKey = "PlayerName";
        #endregion

        #region Private field
        private TMP_InputField _inputField;
        #endregion Private field

        #region MonoBehaviour CallBacks

        void Start()
        {
            var defaultName = string.Empty;
             _inputField = this.GetComponent<TMP_InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    print("Loaded name: " + defaultName);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

        #region Public Methods

        public void SetPlayerName(string value)
        {
            
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
            print("Saved name: " + value);
        }

        #endregion
    }
}
