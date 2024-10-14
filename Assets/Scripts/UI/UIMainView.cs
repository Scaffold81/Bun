using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UIMainView : MonoBehaviour
    {
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        [SerializeField]
        private Button loadLevelButton;

        public Button LoadLevelButton { get => loadLevelButton; private set => loadLevelButton = value; }

        private void Awake()
        {
            ControlPanelShow();
        }

        public void ControlPanelShow()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        public void LoadingPanelShow()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
        }
    }
}
