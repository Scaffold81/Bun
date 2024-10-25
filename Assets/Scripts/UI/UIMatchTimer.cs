using Core.Data;
using System;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class UIMatchTimer : MonoBehaviour
    {
        private SceneDataProvider _sceneDataProvider;

        [SerializeField]
        private PlayerDataNames _dataNames;

        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _text.text = "00:00:00";
        }
        private void Start()
        {
            _sceneDataProvider = SceneDataProvider.Instance;
            _sceneDataProvider.Receive<float>(PlayerDataNames.Timer).Subscribe(newValue =>
            {
                _text.text = "" + TimeSpan.FromMinutes(newValue);
            });
        }
    }
}
