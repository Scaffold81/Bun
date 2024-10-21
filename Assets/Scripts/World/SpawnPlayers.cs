#nullable enable
using Core.Data;
using Core.Player.Controllers;
using Core.UI;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    private SceneDataProvider _sceneDataProvider;

    [SerializeField]
    private GameObject _playerCameraPrefab;

    [SerializeField]
    private GameObject _playerUIprefab;

    [SerializeField]
    private GameObject _playerControllerPrefab;

    private readonly float _spawnRadius = 10;
    private readonly float _newPriority = 1;

    private void Start()
    {
        _sceneDataProvider = SceneDataProvider.Instance;
    }

    public void SpawnPlayer()
    {
        var players = (List<PlayerController>?)_sceneDataProvider.GetValue(PlayerDataNames.Players) ?? new List<PlayerController>();
        
        var spawnPosition = CalculatePositionInRadius();

        if (_playerControllerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            var player = PhotonNetwork.Instantiate(this._playerControllerPrefab.name, spawnPosition, Quaternion.identity, 0).GetComponent<PlayerController>();
            var photonView = player.gameObject.GetPhotonView();
            
            if (photonView != null)
            {
                if (photonView.IsMine)
                {
                    var playerCamera = Instantiate(_playerCameraPrefab, player.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    playerCamera.GetComponent<PlayerCameraView>().Init(player.transform);
                    playerCamera.GetComponent<Camera>().depth = _newPriority;

                    var playerUI = Instantiate(_playerUIprefab);
                    player.SetPlayerUI(playerUI.GetComponent<UIPlayerView>());
                    player.PlayerData.IsMine = true;
                }
                else
                {
                    player.PlayerData.IsMine = false;
                }
            }

            player.PlayerData.PlayerState = PlayerStates.Alive;
            players.Add(player);
            _sceneDataProvider.Publish(PlayerDataNames.Players, players);


        }
    }

    private Vector3 CalculatePositionInRadius()
    {
        var randomAngle = Random.Range(0f, 2f * Mathf.PI);  // Генерируем случайный угол в радианах
        var randomRadius = Mathf.Sqrt(Random.Range(0f, 1f)) * _spawnRadius;  // Генерируем случайный радиус

        var x = randomRadius * Mathf.Cos(randomAngle);  // Вычисляем x координату
        var z = randomRadius * Mathf.Sin(randomAngle);  // Вычисляем z координату

        Vector3 spawnPosition = new Vector3(x, 5, z);  // Создаем позицию для инстанцирования объекта  
        return spawnPosition;
    }
}
