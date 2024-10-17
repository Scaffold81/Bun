using DG.Tweening;
using UnityEngine;

namespace Core.Player.Controllers
{
    public class PlayerView
    {
        private Rigidbody _rb;
        private GameObject _gameObject;
        private PlayerData _playerData;

        public PlayerView(Rigidbody rb,  GameObject gameObject, PlayerData playerData)
        {
            _rb = rb;
            _gameObject = gameObject;
            _playerData = playerData;

            Subscribe();
        }

        private void Subscribe()
        {
            _playerData.MassChanged += SetMassPlayer;
        }
        
        public void UnSubscribe()
        {
            _playerData.MassChanged -= SetMassPlayer;
        }
        
        private void SetMassPlayer(float value)
        {
            _rb.mass = value;
            _gameObject.transform.DOScale(_rb.mass * 0.001f, 0.1f);
        }
    }
}
