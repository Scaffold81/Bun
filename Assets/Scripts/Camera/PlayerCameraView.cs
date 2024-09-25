using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraView : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;
    
    [SerializeField]
    private float smoothSpeed = 5.125f;
    private Vector3 velocity = Vector3.zero;
    public Transform Target { get ; set; }
    public Vector3 Offset { get;}
    
    private void Start()
    {

    }

    public void Init(Transform target)
    {
        this.target = target;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Quaternion targetRotation = target.rotation;
            Vector3 desiredPosition = target.position + targetRotation * offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(target);
        }
    }
}
