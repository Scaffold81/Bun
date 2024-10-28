using Game.Core.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UILevelMainMenuView : MonoBehaviour
    {
        private GameManager gameManager;
        [SerializeField]
        private Button leaveRoomBtn;

        public void Init(GameManager gameManager)
        {
            this.gameManager = gameManager;
            leaveRoomBtn.onClick.AddListener(gameManager.LeaveRoom);
        }
    }
}
