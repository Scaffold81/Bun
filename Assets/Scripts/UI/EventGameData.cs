using Game.Core.Enums;
namespace Core.UI
{
    [System.Serializable]
    public struct EventGameData
    {
        public GameDataNames gameDataNames;
        public GameEventName gameState;
    }
}
