using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player.Controllers
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private PlayerData playerData;
        private ControllerViewBase view;
        private PlayerUIView playerUIView;

        public PlayerData PlayerData { get { return playerData; } }

        private void Awake()
        {
            playerData = new PlayerData();
            view = GetComponent<ControllerViewBase>();
            playerData.UpdateStatsOnLevelUp();
        }

        public void OnRotateInput(InputAction.CallbackContext context)
        {
            var rotate = context.ReadValue<Vector2>();
            view.OnRotate(rotate, playerData.SpeedRotate);
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            var movement = context.ReadValue<Vector2>();
            view.OnMove(movement, playerData.Speed);
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
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
