using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Willow.Library;
#if UNITY_EDITOR
using UnityEditor;
#endif 

public class FullOrbitCamera : MonoBehaviour
{
    [HideInInspector]
    public bool allowZoom;

    [HideInInspector]
    public float minZoomDist = 5f;
    [HideInInspector]
    public float maxZoomDist = 15f;
    [HideInInspector]
    public float dist = 10f;

    [Space(5)]
    [Range(0.2f, 10f)]
    public float smoothing = 2f;
    [Range(0f, 90f)]
    public float verticalClamping = 85f;
    [Range(0f, 360f)]
    public float horizontalClamping = 360f;


    [Space(5)]
    public float orbitSensitivity = 1f;
    public float zoomSensitivity = 1f;

    [Space(10)]
    public Transform target;

    private Vector3 targetFallback;

    private float targetX;
    private float targetY;
    private float x;
    private float y;
    private float z;

    private void Start()
    {
        targetFallback = transform.position;
        z = dist;
    }

    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            targetX += Input.GetAxisRaw("Mouse X") * orbitSensitivity;
            if (horizontalClamping != 360f)
            {
                targetX = Mathf.Clamp(targetX, -horizontalClamping, horizontalClamping);
            }

            targetY += Input.GetAxisRaw("Mouse Y") * orbitSensitivity;
            targetY = Mathf.Clamp(targetY, -verticalClamping, verticalClamping);
        }

        x = Maths.Damp(x, targetX, smoothing * smoothing, true);
        y = Maths.Damp(y, targetY, smoothing * smoothing, true);

        transform.rotation = Quaternion.Euler(-y, x, 0f);

        Vector3 targetPos = target ? target.position : targetFallback;

        if (allowZoom)
        {
            dist = Mathf.Clamp(dist + -Input.mouseScrollDelta.y * zoomSensitivity * 0.5f, minZoomDist, maxZoomDist);
        }
        z = Maths.Damp(z, dist, smoothing * smoothing, true);

        transform.position = targetPos + transform.forward * -z;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(FullOrbitCamera))]
public class FullOrbitCameraEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FullOrbitCamera foc = (FullOrbitCamera)target;

        Undo.RecordObject(target, "Full Orbit Camera custom inspector");

        bool old = foc.allowZoom;
        foc.allowZoom = EditorGUILayout.Toggle("Allow Zoom", foc.allowZoom);
        if(foc.allowZoom && !old)
        {
            foc.minZoomDist = Mathf.Min(foc.minZoomDist, foc.dist);
            foc.maxZoomDist = Mathf.Max(foc.maxZoomDist, foc.dist);
        }

        if (!foc.allowZoom)
        {
            foc.dist = Mathf.Max(0f,EditorGUILayout.FloatField("Distance", foc.dist));
        }
        else
        {
            foc.minZoomDist = Mathf.Max(0f, EditorGUILayout.FloatField("Minimum Distance", foc.minZoomDist));
            foc.maxZoomDist = Mathf.Max(foc.minZoomDist, EditorGUILayout.FloatField("Maximum Distance", foc.maxZoomDist));
            foc.dist = Mathf.Clamp(EditorGUILayout.Slider("Distance", foc.dist, foc.minZoomDist, foc.maxZoomDist), foc.minZoomDist, foc.maxZoomDist);
        }

        base.OnInspectorGUI();
    }
}
#endif

