using Core.Data;
using Game.Core.Enums;
using RxExtensions;
using System;
using System.Reactive.Disposables;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class UIMatchTimer : MonoBehaviour
    {
        private SceneDataProvider _sceneDataProvider;
        private CompositeDisposable _disposables = new();

        [SerializeField]
        private GameDataNames _dataNames;

        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _text.text = "00:00:00";
        }

        private void Start()
        {
            _sceneDataProvider = SceneDataProvider.Instance;
            _sceneDataProvider.Receive<float>(_dataNames).Subscribe(newValue =>
            {
                _text.text = "" + TimeSpan.FromMinutes(newValue);

            }).AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
