using Core.Player.Controllers;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField]
    GameObject playerCameraPrefab;

    [SerializeField]
    GameObject playerControllerPrefab;
    
    public void SpawnPlayer()
    {
        if (playerControllerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
           
           var player = PhotonNetwork.Instantiate(this.playerControllerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
           var photonView=player.GetPhotonView();
            if (photonView != null)
            {
                if (photonView.IsMine)
                {
                    var playerCamera = Instantiate(playerCameraPrefab);
                    playerCamera.GetComponent<PlayerCameraView>().Init(player.transform);

                }
            }
        }
    }
}
