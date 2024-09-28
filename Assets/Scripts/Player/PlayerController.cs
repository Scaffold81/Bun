using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player.Controllers
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private PlayerData playerData;
        private ControllerViewBase view;
        private PlayerUIView playerUIView;
        private PhotonView photonView;
        
        public PlayerData PlayerData { get { return playerData; } }

        private void Awake()
        {
            playerData = new PlayerData();
            view = GetComponent<ControllerViewBase>();
            photonView=GetComponent<PhotonView>();

            playerData.UpdateStatsOnLevelUp();

            playerData.IsMine=photonView.IsMine;
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
    }
}
