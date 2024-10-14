using Core.Player.Controllers;
using Core.UI;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCameraPrefab;

    [SerializeField]
    private GameObject playerUIprefab;

    [SerializeField]
    private GameObject playerControllerPrefab;

    private float spawnRadius = 100;
    private float newPriority=1;

    public void SpawnPlayer()
    {
        Vector3 spawnPosition = CalculatePositionInRadius();

        if (playerControllerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

            var player = PhotonNetwork.Instantiate(this.playerControllerPrefab.name, spawnPosition, Quaternion.identity, 0);
            var photonView = player.GetPhotonView();
            if (photonView != null)
            {
                if (photonView.IsMine)
                {
                    var playerCamera = Instantiate(playerCameraPrefab, player.transform.position+new Vector3(0,1,0),Quaternion.identity);
                    playerCamera.GetComponent<PlayerCameraView>().Init(player.transform);
                    playerCamera.GetComponent<Camera>().depth = newPriority;

                    var playerUI = Instantiate(playerUIprefab);
                    player.GetComponent<PlayerController>().Init(playerUI.GetComponent<UIPlayerView>());
                }
            }
        }
    }

    private Vector3 CalculatePositionInRadius()
    {
        var randomAngle = Random.Range(0f, 2f * Mathf.PI);  // Генерируем случайный угол в радианах
        var randomRadius = Mathf.Sqrt(Random.Range(0f, 1f)) * spawnRadius;  // Генерируем случайный радиус

        var x = randomRadius * Mathf.Cos(randomAngle);  // Вычисляем x координату
        var z = randomRadius * Mathf.Sin(randomAngle);  // Вычисляем z координату

        Vector3 spawnPosition = new Vector3(x, 5, z);  // Создаем позицию для инстанцирования объекта  
        return spawnPosition;
    }
}
