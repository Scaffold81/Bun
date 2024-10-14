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
    public Transform Target { get ;private set; }
    public Vector3 Offset { get;}

    public void Init(Transform target)
    {
        this.Target = target;
    }

    private void LateUpdate()
    {
        if (Target != null)
        {
            Quaternion targetRotation = Target.rotation;
            Vector3 desiredPosition = Target.position + targetRotation * offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(Target);
        }
    }
}
