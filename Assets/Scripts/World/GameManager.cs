using Core.Data;
using Core.Player.Controllers;
using Core.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    private SceneDataProvider _sceneDataProvider;

    [SerializeField]
    private UILevelMainMenuView levelMainMenuUI;
    [SerializeField]
    private SpawnPlayers spawnPlayers;
    
    [SerializeField]
    private float startingCountdownTime = 1;
    private float startUpTimer = 1f;

    private void Start()
    {
        levelMainMenuUI.Init(this);
        _sceneDataProvider = SceneDataProvider.Instance; 

        Subscription();

        StartCoroutine(StartGame());
        
    }

    private void Subscription()
    {
        _sceneDataProvider.Receive<float>(PlayerDataNames.Timer).Subscribe(newValue =>
        {
           
        });

        _sceneDataProvider.Receive<List<PlayerController>>(PlayerDataNames.Players).Subscribe(newValue =>
        {

        });
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
            }
        }
    }

    #region Photon Callbacks

    public override void OnMasterClientSwitched(Player other)
    {
        _sceneDataProvider.Publish(PhotonCallbacksNames.OnMasterClientSwitched, other.NickName);
    }
    
    public override void OnPlayerEnteredRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _sceneDataProvider.Publish(PhotonCallbacksNames.OnPlayerEnteredRoom, other.NickName);
        }
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
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
    #endregion Private Methods
}
