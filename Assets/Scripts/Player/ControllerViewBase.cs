using UnityEngine;

namespace Core.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class ControllerViewBase : MonoBehaviour
    {
        private Rigidbody rb;

        [SerializeField]
        private bool isGrounded;
        private Transform groundCheck;
        [SerializeField]
        private LayerMask groundMask;
        [SerializeField]
        private float groundDistance = 0.55f;

        private float stopVelocity;
        private float moveSpeed; 
        
        private Vector3 moveInput;
        private Vector3 smoothVelocity = Vector3.zero;

        private float rotationInput;
        private float rotateSpeed;

        private void Awake()
        {
            groundCheck = transform;
            rb = GetComponent<Rigidbody>();
        }

        public void OnMove(Vector2 direction, float speed)
        {
            moveInput = direction;
            moveSpeed = speed;
        }

        public void StopCharacter()
        {
            stopVelocity = Mathf.SmoothDamp(rb.velocity.magnitude, 0f, ref stopVelocity, 0.5f);
            rb.velocity = rb.velocity.normalized * stopVelocity;
        }

        public void OnRotate(Vector2 rotation, float rotationSpeed)
        {
            rotationInput = rotation.normalized.x * rotationSpeed;
            rotateSpeed = rotationSpeed;
        }

        public void OnJump(float jumpForce)
        {
            if (isGrounded)
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void Move()
        {
            var targetVelocity = transform.TransformDirection(new Vector3(0, 0, moveInput.y)) * moveSpeed;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref smoothVelocity, 0.5f);
        }

        private void Rotate()
        {
            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * rotationInput * rotateSpeed * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        private void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }

        private void FixedUpdate()
        {
            Rotate();
            Move();
        }
    }
}
