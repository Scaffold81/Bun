using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UICustomButtonBase : MonoBehaviour
    {
        protected Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _btn.onClick.AddListener(OnClick);
        }

        public virtual void OnClick()
        {

        }

        private void OnDisable()
        {
           _btn.onClick.RemoveAllListeners();
        }
    }
}
