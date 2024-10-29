using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public class UIPanelStateSwitcherBase : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private float _duration = 0.5f;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected void PanelOn()
        {
            var startValue = 0f;
            var endValue = 1f;

            DOTween.To(() => startValue, x => startValue = x, endValue, _duration)
                .OnUpdate(() =>
                {
                    _canvasGroup.alpha = startValue; // Обновление прозрачности в процессе анимации
                })
                .OnComplete(() =>
                {
                    _canvasGroup.blocksRaycasts = true;
                    _canvasGroup.interactable = true;
                });

        }

        protected void PanelOff()
        {
            var startValue = 1f;
            var endValue = 0f;

            DOTween.To(() => startValue, x => startValue = x, endValue, _duration)
               .OnUpdate(() =>
               {
                   _canvasGroup.alpha = startValue; 
               })
               .OnComplete(() =>
               {
                   _canvasGroup.blocksRaycasts = false;
                   _canvasGroup.interactable = false;
               });
        }
    }
}
