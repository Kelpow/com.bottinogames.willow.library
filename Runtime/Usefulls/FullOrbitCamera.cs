using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Willow.Library;


public class FullOrbitCamera : MonoBehaviour
{
    [Min(0)]
    public float dist = 10f;
    [Range(0.2f, 10f)]
    public float smoothing = 2f;
    [Range(0f, 90f)]
    public float verticalClamping = 85f;
    [Range(0f, 360f)]
    public float horizontalClamping = 360f;

    public Transform target;

    private Vector3 targetFallback;

    private float targetX;
    private float targetY;
    private float x;
    private float y;

    private void Start()
    {
        targetFallback = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            targetX += Input.GetAxisRaw("Mouse X");
            if (horizontalClamping != 360f)
            {
                targetX = Mathf.Clamp(targetX, -horizontalClamping, horizontalClamping);
            }

            targetY += Input.GetAxisRaw("Mouse Y");
            targetY = Mathf.Clamp(targetY, -verticalClamping, verticalClamping);
        }

        x = Maths.Damp(x, targetX, smoothing * smoothing, true);
        y = Maths.Damp(y, targetY, smoothing * smoothing, true);

        transform.rotation = Quaternion.Euler(-y, x, 0f);

        Vector3 targetPos = target ? target.position : targetFallback;
        transform.position = targetPos + transform.forward * -dist;
    }
}
