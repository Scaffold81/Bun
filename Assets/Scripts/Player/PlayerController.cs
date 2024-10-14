using Core.UI;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerData playerData;
        private ControllerViewBase view;
        private UIPlayerView playerUIView;
        private PhotonView photonView;

        

        public PlayerData PlayerData { get { return playerData; } }

        private void Awake()
        {
            playerData = new PlayerData();
            view = GetComponent<ControllerViewBase>();
            photonView=GetComponent<PhotonView>();
        }
        
        public void Init(UIPlayerView playerUIView)
        {
            this.playerUIView = playerUIView;

            playerUIView.JoysticHandler.Direction += MoveEndRotate;
            playerUIView.JumpButtonHandler.Jump += OnJump;

            playerData.UpdateStatsOnLevelUp();
            playerData.IsMine = photonView.IsMine;
        }
       
        private void MoveEndRotate(Vector2 vector) 
        {
            vector=vector.normalized;
            view.OnMove(new Vector2(0, vector.y), playerData.Speed);
            view.OnRotate(new Vector2(vector.x, 0), playerData.SpeedRotate);
        }
        public void OnJump()
        {
            if (!playerData.IsMine) return;
            view.OnJump(playerData.JumpForce);
        }

        public void OnRotateInput(InputAction.CallbackContext context)
        {
            if (!playerData.IsMine)return;
            var rotate = context.ReadValue<Vector2>();
            view.OnRotate(rotate, playerData.SpeedRotate);
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            if (!playerData.IsMine) return;
            var movement = context.ReadValue<Vector2>();
            view.OnMove(movement, playerData.Speed);
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (!playerData.IsMine) return;
            view.OnJump(playerData.JumpForce);
        }
        
        public void LevelUp()
        {
            playerData.LevelUp(playerData.Level++);
            playerData.UpdateStatsOnLevelUp();
        }

        public void Damage(int damage)
        {

        }

        private void OnDestroy()
        {
            playerUIView.JoysticHandler.Direction -= MoveEndRotate;
            playerUIView.JumpButtonHandler.Jump -= OnJump;
        }
    }
}
