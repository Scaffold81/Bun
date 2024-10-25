using Core.Data;
using Core.Player.Controllers;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Core.Player.Gameplay
{
    public class MatchTimer : MonoBehaviour, IPunObservable
    {
        [SerializeField]
        private float _currentTimer = 900f;
        private bool _isTimerRunning = false;

        private SceneDataProvider _sceneDataProvider;
        private Coroutine _coroutine;
       
        public float CurrentTimer
        {
            get => _currentTimer;
            set
            {
                _currentTimer = value;
                SendTimeTimer(value);
            }
        }

        private void Start()
        {
            _sceneDataProvider = SceneDataProvider.Instance;

            _sceneDataProvider.Receive<string>(PhotonCallbacksNames.OnMasterClientSwitched).Subscribe(newValue =>
            {
                if(PhotonNetwork.IsMasterClient)
                    StartTimer();
            });

            _sceneDataProvider.Receive<PlayerController>(PhotonCallbacksNames.OnPlayerMasterClientSpawn).Subscribe(newValue =>
            {
                if (PhotonNetwork.IsMasterClient)
                    StartTimer();
            });
        }

        private IEnumerator Timer()
        {
            while (_isTimerRunning)
            {
                yield return new WaitForSeconds(1);
                CurrentTimer -= 1;

                if (CurrentTimer <= 0)
                {
                    // Обработка завершения таймера
                    StopTimer();
                }
            }
        }

        #region public 
        public void StartTimer()
        {
            _isTimerRunning = true;
            _coroutine = StartCoroutine(nameof(Timer));
        }

        public void StopTimer()
        {
            _isTimerRunning = false;
            StopCoroutine(nameof(Timer));
            _coroutine = null;
        }
        #endregion public 

        #region private
        private void SendTimeTimer(float timeRemaining)
        {
            _sceneDataProvider.Publish(PlayerDataNames.Timer, timeRemaining);
        }
        #endregion private

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting&&PhotonNetwork.IsMasterClient)
            {
                // We own this player: send the others our data
                stream.SendNext(CurrentTimer);
            }
            else
            {
                // Network player, receive data
                this.CurrentTimer = (float)stream.ReceiveNext();
            }
        }
        private void OnDestroy()
        {
            StopTimer();
        }
    }
}
