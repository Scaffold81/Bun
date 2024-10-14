using Photon.Pun;
using System;
using UnityEngine;

namespace Core.Player.Controllers
{
    public class CollisionHandler : MonoBehaviour
    {
        private Rigidbody _rb;

        private float _timeBetweenCollisions = 0.5f;

        private bool _canApplyForce = true;

        public Action<Vector3> DamageImpulse;
        private Rigidbody otherRigidbody;

        public void Init(Rigidbody rb)
        {
            _rb = rb;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_canApplyForce)
            {
                Debug.Log("Too fast collision detection. Waiting for cooldown.");
                return; // ��������� ���������� ������
            }

             otherRigidbody = collision.gameObject.GetComponent<Rigidbody>();
           
            if (otherRigidbody != null)
            {
                var impactForce = _rb.velocity * _rb.mass * 5; // ����������� ������� ����
                
                var forceDirection = otherRigidbody.transform.position - _rb.transform.position;
                
                var force = forceDirection.normalized * impactForce.magnitude; 

                // �������� RPC ������� �������, ������ ��� ������ � ��������� (��������� ������� ����)
                var photonView = collision.gameObject.GetComponent<PhotonView>();

                if (photonView != null)
                    photonView.RPC("HandleCollision", RpcTarget.Others, force);

                _canApplyForce = false; // ������������� ���� "canApplyForce" � false
                Invoke(nameof(ResetForceFlag), _timeBetweenCollisions); // �������� ����� ������ ����� ����� ���������� �������
            }
        }

        [PunRPC]
        private void HandleCollision(Vector3 impactForce)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            DamageImpulse(impactForce);
        }

        private void ResetForceFlag()
        {
            _canApplyForce = true; // ���������� ���� �� ����������� ���������� ����
        }
    }
}
