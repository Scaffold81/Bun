using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private LevelMainMenuUIView levelMainMenuUI;
    [SerializeField]
    private SpawnPlayers spawnPlayers;
    [SerializeField]
    private float startingCountdownTime = 5;
    private float startUpTimer = 5f;

    private void Start()
    {
        levelMainMenuUI.Init(this);
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        startUpTimer = startingCountdownTime;
        while (startUpTimer > 0)
        {
            yield return new WaitForSeconds(1);
            startUpTimer--;

            if (startUpTimer == 0)
            {
                spawnPlayers.SpawnPlayer();
                print(startUpTimer);
            }
        }
    }
    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }

    #endregion Photon Callbacks

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion Public Methods

    #region Private Methods
   
    private void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    } 

    #endregion Private Methods
}
