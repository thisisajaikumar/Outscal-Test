using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] Transform target;
    [SerializeField,Range(1,10)] float smoothSpeed = 5f; 
    [SerializeField] Vector2 minPosition, maxPosition; 

    private void LateUpdate()
    {
        if (target != null)
        {
            float clampedX = Mathf.Clamp(target.position.x, minPosition.x, maxPosition.x);
            float clampedY = Mathf.Clamp(target.position.y, minPosition.y, maxPosition.y);

            Vector3 targetPosition = new Vector3(clampedX, clampedY, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            transform.position = smoothedPosition;
        }
    }
}

