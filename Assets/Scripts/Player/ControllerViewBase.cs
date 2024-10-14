using UnityEngine;

namespace Core.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class ControllerViewBase : MonoBehaviour
    {
        private Rigidbody _rb;
       
        #region Check ground fields
        private bool _isGrounded;
        private Transform _groundCheck;
        [SerializeField]
        private LayerMask _groundMask;
        [SerializeField]
        private float _groundDistance = 0.55f;
        #endregion Check ground fields

        #region Move fields
        private float _moveSpeed; 
        private Vector3 _moveInput;
        private Vector3 _smoothVelocity = Vector3.zero;
        #endregion Move fields

        #region Rotate fields
        private float _rotationInput;
        private float _rotateSpeed;
        #endregion Rotate fields

        private void Awake()
        {
            _groundCheck = transform;
        }

        public void Init(Rigidbody rb)
        {
           _rb = rb;
        }

        public void OnMove(Vector2 direction, float speed)
        {
            _moveInput = direction;
            _moveSpeed = speed;
        }

        public void OnRotate(Vector2 rotation, float rotationSpeed)
        {
            _rotationInput = rotation.normalized.x * rotationSpeed;
            _rotateSpeed = rotationSpeed;
        }

        public void OnJump(float jumpForce)
        {
            if (_isGrounded)
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            print("OnJump");
        }

        private void Move()
        {
            if (!_rb) return;
            var targetVelocity = transform.TransformDirection(new Vector3(0, 0, _moveInput.y)) * _moveSpeed;
            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _smoothVelocity, 0.5f);
        }

        private void Rotate()
        {
            if (!_rb) return;
            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * _rotationInput * _rotateSpeed * Time.deltaTime);
            _rb.MoveRotation(_rb.rotation * deltaRotation);
        }

        public void SetInpulse(Vector3 direction)
        {
            if(!_rb)return;
            _rb.AddForce(direction, ForceMode.Impulse);
        }

        private void Update()
        {
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
        }

        private void FixedUpdate()
        {
            Rotate();
            Move();
        }
    }
}

