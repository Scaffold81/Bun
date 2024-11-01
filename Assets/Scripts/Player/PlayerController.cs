using Core.Data;
using Core.UI;
using Game.Core.Enums;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player.Controllers
{
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(PhotonView))]
    public class PlayerController : MonoBehaviour, IPunObservable
    {
        private GameObject _go;
        private Rigidbody _rb;

        private PlayerData _playerData;
        private ControllerViewBase _view;
        private UIPlayerView _playerUIView;
        private PhotonView _photonView;
        private CollisionHandler _collisionHandler;
        private PlayerView _playerView;

        private SceneDataProvider _sceneDataProvider;

        public PlayerData PlayerData { get { return _playerData; } }

        private void Awake()
        {
            _go = this.gameObject;
            _rb = GetComponent<Rigidbody>();
            _playerData = new PlayerData();
            _view = GetComponent<ControllerViewBase>();
            _photonView = GetComponent<PhotonView>();
            _collisionHandler = GetComponent<CollisionHandler>();
            _playerView = new PlayerView(_rb, _go, _playerData);
        }

        private void Start()
        {
            _sceneDataProvider = SceneDataProvider.Instance;

            _view.Init(_rb);
            _collisionHandler.Init(_rb, _photonView, this);
            _photonView.RPC("UpdateStats", RpcTarget.All);
        }

        public void SetPlayerUI(UIPlayerView playerUIView)
        {
            _playerUIView = playerUIView;

            if (!PlayerData.IsMine) return;
            _playerUIView.JoysticHandler.Direction += MoveAndRotate;
            _playerUIView.JumpButtonHandler.Jump += OnJump;
        }

        private void MoveAndRotate(Vector2 vector)
        {
            if (!_playerData.IsMine || !_playerData.IsActive) return;
            vector = vector.normalized;
            _view.OnMove(new Vector2(0, vector.y), _playerData.Speed);
            _view.OnRotate(new Vector2(vector.x, 0), _playerData.SpeedRotate);
        }

        public void OnJump()
        {
            if (!_playerData.IsMine || !_playerData.IsActive) return;
            _view.OnJump(_playerData.JumpForce);
        }

        public void OnRotateInput(InputAction.CallbackContext context)
        {
            if (!_playerData.IsMine || !_playerData.IsActive) return;
            var rotate = context.ReadValue<Vector2>();
            _view.OnRotate(rotate, _playerData.SpeedRotate);
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            var movement = context.ReadValue<Vector2>();

            if (!_playerData.IsMine || !_playerData.IsActive) return;

            _view.OnMove(movement, _playerData.Speed);
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (!_playerData.IsMine || !_playerData.IsActive) return;
            _view.OnJump(_playerData.JumpForce);
        }

        public void OnDamage(Vector3 impactForce)
        {
            if (!_playerData.IsMine) return;
            PlayerData.Mass -= impactForce.magnitude * 0.01f;
            if (PlayerData.Mass < PlayerData.BaseMinMass)
                OnDead();
        }

        public void IncreaseMass(Vector3 impactForce)
        {
            if (!_playerData.IsMine || !_playerData.IsActive) return;
            PlayerData.Mass += impactForce.magnitude * 0.01f;
        }

        private void OnDead()
        {
            _playerData.IsActive = false;
            _sceneDataProvider.Publish(PlayerDataNames.PlayerDeleted, this);

            if (_playerData.IsMine)
            {
                _sceneDataProvider.Publish(GameDataNames.GameState, GameEventName.DeadPanelOn);
                _sceneDataProvider.Publish(GameDataNames.GameState, GameEventName.PausePanelOff);
                _sceneDataProvider.Publish(GameDataNames.GameState, GameEventName.MainMenuPanelOff);
            }

            PhotonNetwork.Destroy(_photonView);
        }

        #region Pun RPC

        [PunRPC]
        private void UpdateStats()
        {
            _playerData.UpdateStats();
        }

        [PunRPC]
        public void SetImpulceRPC(Vector3 force)
        {
            _view.SetInpulse(new Vector3(force.x, 0, force.z));
        }

        #endregion  Pun RPC

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(PlayerData.Mass);
            }
            else
            {
                // Network player, receive data
                PlayerData.Mass = (float)stream.ReceiveNext();
            }
        }

        private void OnDestroy()
        {
            _sceneDataProvider.Publish(PlayerDataNames.PlayerDeleted, this);

            if (!PlayerData.IsMine) return;

            _playerUIView.JoysticHandler.Direction -= MoveAndRotate;
            _playerUIView.JumpButtonHandler.Jump -= OnJump;
            _playerView.UnSubscribe();
        }

    }
}
