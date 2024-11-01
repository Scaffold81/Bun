using Core.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public class UIButtonPlayerState : UICustomButtonBase
    {
        [SerializeField]
        private List<EventGameData> eventGameDatas;

        private SceneDataProvider _sceneDataProvider;

        private void Start()
        {
            _sceneDataProvider = SceneDataProvider.Instance;
        }

        public override void OnClick()
        {
            foreach (var gameData in eventGameDatas)
            {
                _sceneDataProvider.Publish(gameData.gameDataNames, gameData.gameState);
            }
        }
    }
}
