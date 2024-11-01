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
using Game.Core.Enums;
using System.Reactive.Disposables;
using RxExtensions;

namespace Game.Core.Managers
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        private SceneDataProvider _sceneDataProvider;

        [SerializeField]
        private UILevelMainView levelMainMenuUI;
        [SerializeField]
        private SpawnPlayers spawnPlayers;

        [SerializeField]
        private float startingCountdownTime = 1;
        private float startUpTimer = 1f;

        private CompositeDisposable _disposables = new();

        private void Start()
        {
            levelMainMenuUI.Init(this);
            _sceneDataProvider = SceneDataProvider.Instance;

            Subscription();

            StartCoroutine(StartGame());

        }
        
        #region Private Methods
        private void Subscription()
        {
            _sceneDataProvider.Receive<GameEventName>(GameDataNames.GameState).Subscribe(newValue =>
            {
                if (newValue == GameEventName.End)
                    EndGame();
                else if (newValue == GameEventName.Pause)
                    PauseGame();
                else if (newValue == GameEventName.Release)
                    ReleaseGame();
                else if (newValue == GameEventName.Leave)
                    LeaveGame();
                else if (newValue == GameEventName.Ressurect)
                    RessurectPlayer();

            }).AddTo(_disposables);
            
            _sceneDataProvider.Receive<PlayerController>(PlayerDataNames.PlayerDeleted).Subscribe(newValue =>
            {
                RemovePlayerInList(newValue);
            }).AddTo(_disposables);
        }
        
        private void PauseGame()
        {
           var player = (PlayerController)_sceneDataProvider.GetValue(PlayerDataNames.CurrentPlayer) ?? new PlayerController();
           player.PlayerData.IsActive = false;
        }

        private void ReleaseGame()
        {
            var player = (PlayerController)_sceneDataProvider.GetValue(PlayerDataNames.CurrentPlayer) ?? new PlayerController();
            player.PlayerData.IsActive = true;
        }

        private void EndGame()
        {
            var players = (List<PlayerController>)_sceneDataProvider.GetValue(PlayerDataNames.Players) ?? new List<PlayerController>();
            players.ForEach(player => player.PlayerData.IsActive = false);
            _sceneDataProvider.Publish(GameDataNames.GameState, GameEventName.DeadPanelOff);
            _sceneDataProvider.Publish(GameDataNames.GameState, GameEventName.MainMenuPanelOff);
            _sceneDataProvider.Publish(GameDataNames.GameState, GameEventName.PausePanelOff);
            _sceneDataProvider.Publish(GameDataNames.GameState, GameEventName.EndGamePanelOn);
        }

        private void RemovePlayerInList(PlayerController value)
        {
            var players = (List<PlayerController>)_sceneDataProvider.GetValue(PlayerDataNames.Players) ?? new List<PlayerController>();
            players.Remove(value);
            _sceneDataProvider.Publish(PlayerDataNames.Players, players);
        }

        private void LeaveGame()
        {
            LeaveRoom();
        }

        private void RessurectPlayer()
        {
            _sceneDataProvider.Publish(GameDataNames.GameState, GameEventName.DeadPanelOff);
            spawnPlayers.SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            spawnPlayers.SpawnPlayer();
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
                    SpawnPlayer();
                }
            }
        }

        #endregion Private Methods
       
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
        
        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
