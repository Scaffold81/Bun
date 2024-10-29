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

        private CompositeDisposable _disposables = new();

        private SceneDataProvider _sceneDataProvider;

        private void Start()
        {
            _sceneDataProvider = SceneDataProvider.Instance;

            _sceneDataProvider.Receive<GameState>(_gameDataNames).Subscribe(newValue =>
            {
                if (newValue == GameState.Pause)
                    PanelOn();
                else if (newValue == GameState.Release) 
                    PanelOff();

            }).AddTo(_disposables);
        }
        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
