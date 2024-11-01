namespace Game.Core.Enums
{
    public enum PlayerDataNames
    {
        CurrentPlayer,
        Players,
        PlayerState,
        PlayerDeleted
    }
    public enum GameDataNames
    {
        GameState,
        Timer
    }

    public enum GameEventName
    {   /// Game events
        Start = 0,
        Pause = 1,
        Release = 2,
        Ressurect = 3,
        Stop = 4,
        End = 5,
        Leave = 6,
        PlayerDead = 7,

        /// UI events
        PausePanelOn = 8,
        PausePanelOff = 9,
        DeadPanelOn = 10,
        DeadPanelOff = 11,
        EndGamePanelOn=12,
        EndGamePanelOff=13,
        MainMenuPanelOn = 14,
        MainMenuPanelOff = 15,
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

