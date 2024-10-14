using Core.UI;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rb;

        private PlayerData _playerData;
        private ControllerViewBase _view;
        private UIPlayerView playerUIView;
        private PhotonView _photonView;
        private CollisionHandler _collisionHandler;
        
        public PlayerData PlayerData { get { return _playerData; } }
       
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _playerData = new PlayerData();
            _view = GetComponent<ControllerViewBase>();
            _photonView = GetComponent<PhotonView>();
            _collisionHandler = GetComponent<CollisionHandler>();
        }

        private void Start()
        {
            _view.Init(_rb);
            _collisionHandler.Init(_rb);
        }

        public void Init(UIPlayerView playerUIView)
        {

            this.playerUIView = playerUIView;
            
            playerUIView.JoysticHandler.Direction += MoveEndRotate;
            playerUIView.JumpButtonHandler.Jump += OnJump;
           
            _collisionHandler.DamageImpulse += Damage;

            _playerData.UpdateStatsOnLevelUp();
            _playerData.IsMine = _photonView.IsMine;
        }
       
        private void MoveEndRotate(Vector2 vector) 
        {
            vector=vector.normalized;
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
            if (!_playerData.IsMine)return;
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
        
        public void LevelUp()
        {
            _playerData.LevelUp(_playerData.Level++);
            _playerData.UpdateStatsOnLevelUp();
        }

        public void Damage(Vector3 direction)
        {
            _view.SetInpulse(direction);
        }

        private void OnDestroy()
        {
            playerUIView.JoysticHandler.Direction -= MoveEndRotate;
            playerUIView.JumpButtonHandler.Jump -= OnJump;
        }
    }
}
