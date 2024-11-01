using Core.Data;
using Game.Core.Enums;
using System.Reactive.Disposables;
using System;
using UnityEngine;
using RxExtensions;

namespace Core.UI
{
    public class UIPanelGameStateSwitcher : UIPanelStateSwitcherBase
    {
        [SerializeField]
        private GameDataNames _gameDataNames;
       
        [SerializeField]
        private GameEventName _stateForOn;
        [SerializeField]
        private GameEventName _stateForOff;

        private CompositeDisposable _disposables = new();

        private SceneDataProvider _sceneDataProvider;

        private void Start()
        {
            _sceneDataProvider = SceneDataProvider.Instance;

            _sceneDataProvider.Receive<GameEventName>(_gameDataNames).Subscribe(newValue =>
            {
                if (newValue == _stateForOn)
                    PanelOn();
                else if (newValue == _stateForOff) 
                    PanelOff();

            }).AddTo(_disposables);
        }
        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
