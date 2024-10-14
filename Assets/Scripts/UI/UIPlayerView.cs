using UnityEngine;

namespace Core.UI
{
    public class UIPlayerView : MonoBehaviour
    {
        [SerializeField] private UIJoysticHandler joysticHandler;
        [SerializeField] private UIJumpButtonHandler jumpButtonHandler;

        public UIJoysticHandler JoysticHandler { get => joysticHandler; set => joysticHandler = value; }
        public UIJumpButtonHandler JumpButtonHandler { get => jumpButtonHandler; set => jumpButtonHandler = value; }
    }

}