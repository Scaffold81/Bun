namespace Game.Core.Enums
{
    public enum PlayerDataNames
    {
        CurrentPlayer,
        Players,
        PlayerState
    }
    public enum GameDataNames
    {
        GameState,
        Timer
    }

    public enum GameState
    {
        Start,
        Pause,
        Release,
        Stop,
        End,
        Leave
    }
    public enum PlayerState
    {
        Alive,
        Dead
    }

    public enum PhotonCallbacksNames
    {
        OnPlayerMasterClientSpawn,
        OnPlayerClientSpawn,
        OnMasterClientSwitched,
        OnPlayerEnteredRoom
    }
}

