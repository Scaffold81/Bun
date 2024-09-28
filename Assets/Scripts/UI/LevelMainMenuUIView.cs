using UnityEngine;
using UnityEngine.UI;

public class LevelMainMenuUIView : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]
    private Button leaveRoomBtn;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        leaveRoomBtn.onClick.AddListener(gameManager.LeaveRoom);
    }
}
