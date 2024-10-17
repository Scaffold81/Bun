using Core.UI;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player.Controllers
{
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(PhotonView))]
    public class PlayerController : MonoBehaviour
    {
        private GameObject _go;
        private Rigidbody _rb;

        private PlayerData _playerData;
        private ControllerViewBase _view;
        private UIPlayerView _playerUIView;
        private PhotonView _photonView;
        private CollisionHandler _collisionHandler;
        private PlayerView _playerView;

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
            _view.Init(_rb);
            _collisionHandler.Init(_rb, _photonView, this);

            _playerData.IsMine = _photonView.IsMine;
            _photonView.RPC("UpdateStats", RpcTarget.All);
        }

        public void SetPlayerUI(UIPlayerView playerUIView)
        {
            _playerUIView = playerUIView;

            _playerUIView.JoysticHandler.Direction += MoveEndRotate;
            _playerUIView.JumpButtonHandler.Jump += OnJump;
        }

        private void MoveEndRotate(Vector2 vector)
        {
            vector = vector.normalized;
            _view.OnMove(new Vector2(0, vector.y), _playerData.Speed);
            _view.OnRotate(new Vector2(vector.x, 0), _playerData.SpeedRotate);
        }

        public void OnJump()
        {
            if (!_playerData.IsMine) return;
            _view.OnJump(_playerData.JumpForce);
        }

        public void OnRotateInput(InputAction.CallbackContext context)
        {
            if (!_playerData.IsMine) return;
            var rotate = context.ReadValue<Vector2>();
            _view.OnRotate(rotate, _playerData.SpeedRotate);
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            if (!_playerData.IsMine) return;
            var movement = context.ReadValue<Vector2>();
            _view.OnMove(movement, _playerData.Speed);
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (!_playerData.IsMine) return;
            _view.OnJump(_playerData.JumpForce);
        }

        public void OnDamageRPC(Vector3 impactForce)
        {
            _photonView.RPC("Damage", RpcTarget.Others, impactForce);
        }

        public void IncreaseMassRPC(Vector3 impactForce)
        {
            _photonView.RPC("IncreaseMass", RpcTarget.All, impactForce);
        }

        private void OnDead()
        {
            Debug.Log("Player " + _photonView.ViewID + " dead");
        }

        #region Pun RPC

        [PunRPC]
        private void UpdateStats()
        {
            _playerData.UpdateStats();
        }

        [PunRPC]
        public void Damage(Vector3 direction)
        {
            PlayerData.Mass -= direction.magnitude * 0.01f;
            _view.SetInpulse(new Vector3(direction.x, 0, direction.z));
            if (PlayerData.Mass < PlayerData.BaseMinMass)
                OnDead();
        }

        [PunRPC]
        public void IncreaseMass(Vector3 direction)
        {
            PlayerData.Mass += direction.magnitude * 0.01f;
        }

        #endregion  Pun RPC

        private void OnDestroy()
        {
            _playerUIView.JoysticHandler.Direction -= MoveEndRotate;
            _playerUIView.JumpButtonHandler.Jump -= OnJump;
            _playerView.UnSubscribe();
        }


    }
}
