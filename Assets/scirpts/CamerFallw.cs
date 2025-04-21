using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFallw : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        Vector3 desirefdPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desirefdPosition, smoothSpeed);
        transform.position = smoothPosition;

        transform.LookAt(target.position);
    }
}
