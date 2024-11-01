using Game.Core.Managers;
using UnityEngine;

namespace Core.UI
{
    public class UILevelMainView : MonoBehaviour
    {
        private GameManager gameManager;
       

        public void Init(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }
    }
}
