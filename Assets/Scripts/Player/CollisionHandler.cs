using Photon.Pun;
using UnityEngine;

namespace Core.Player.Controllers
{
    public class CollisionHandler : MonoBehaviour
    {
        private PlayerController _playerController;
        private PhotonView _photonView;
        private Rigidbody _rb;

        private float _timeBetweenCollisions = 0.5f;

        private bool _canApplyForce = true;

        public void Init(Rigidbody rb, PhotonView photonView, PlayerController playerController)
        {
            _playerController = playerController;
            _photonView = photonView;
            _rb = rb;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_canApplyForce)
            {
                print("Too fast collision detection. Waiting for cooldown.");
                return; // Прерываем выполнение метода
            }

            var otherRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            var otherPlayer = collision.gameObject.GetComponent<PlayerController>();

            if (otherPlayer == null) return;

            var impactForce = _rb.velocity * _rb.mass; // Рассчитывем импульс силы
            var forceDirection = otherPlayer.transform.position - _rb.transform.position;
            var force = forceDirection.normalized * impactForce.magnitude*5;

            if (_rb.velocity.magnitude > otherRigidbody.velocity.magnitude)
            {
                otherPlayer.OnDamage(force);
                _playerController.IncreaseMass(force);
                otherPlayer.gameObject.GetComponent<PhotonView>().RPC("SetImpulceRPC", RpcTarget.Others,force);
        }

            _canApplyForce = false; // Устанавливаем флаг "canApplyForce" в false
            Invoke(nameof(ResetForceFlag), _timeBetweenCollisions); // Вызываем метод сброса флага после указанного времени
        }

        private void ResetForceFlag()
        {
            _canApplyForce = true; // Сбрасываем флаг на возможность применения силы
        }
    }
}
