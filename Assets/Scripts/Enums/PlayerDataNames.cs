namespace Game.Core.Enums
{
    public enum PlayerDataNames
    {
        CurrenPlayer,
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
        Stop,
        End
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

