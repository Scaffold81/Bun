using Core.Network.Louncher.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Core.Network.Louncher 
{
    public class Louncher : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private MainUIView mainUIView;
        
        private string gameVersion = "1";
      
        private bool isConnecting;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        
        private void Start()
        {
            mainUIView.LoadLevelButton.onClick.AddListener(Connect);
        }
        
        private void OnDestroy()
        {
            mainUIView.LoadLevelButton.onClick.RemoveListener(Connect);
        }

        public void Connect()
        {
            mainUIView.LoadingPanelShow();

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
            Debug.Log("OnConnectedToMaster() was called by PUN");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
            mainUIView.LoadingPanelShow();
            Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom("Pull", new RoomOptions() { MaxPlayers=15});
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'GameScene01' ");

                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("GameScene01");
            }
            Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }


        #endregion
    }
}
