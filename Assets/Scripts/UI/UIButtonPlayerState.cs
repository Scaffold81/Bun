using Core.Data;
using Game.Core.Enums;
using UnityEngine;

namespace Core.UI
{
    public class UIButtonPlayerState : UICustomButtonBase
    {
        [SerializeField]
        private GameDataNames _gameDataNames;
        [SerializeField]
        private GameState _gameState;

        private SceneDataProvider _sceneDataProvider;

        private void Start()
        {
            _sceneDataProvider = SceneDataProvider.Instance;
        }

        public override void OnClick()
        {
            _sceneDataProvider.Publish(_gameDataNames, _gameState);
        }
    }
}
