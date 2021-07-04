using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif


public class BoxButton : MonoBehaviour
{

    [Header("Settings")]
    public new Camera camera;
    public Bounds bounds = new Bounds(Vector3.zero, Vector3.one);

    [Space(10f)]
    [Header("Event")]
    public UnityEvent onClickDown = new UnityEvent();

    private void Update()
    {
        if(onClickDown != null && camera != null && Input.GetMouseButtonDown(0))
        {
            Ray mouseray = camera.ScreenPointToRay(Input.mousePosition);
            Vector3 relativePos = mouseray.origin - transform.position;
            Ray rotatedRay = new Ray(
                Quaternion.Inverse(transform.rotation) * relativePos,
                Quaternion.Inverse(transform.rotation) * mouseray.direction);

            if (bounds.IntersectRay(rotatedRay))
                onClickDown.Invoke();
        }
    }
}





#if UNITY_EDITOR
[CustomEditor(typeof(BoxButton))]
public class BoxButtonInspector : Editor
{

    BoxBoundsHandle handle;

    private void OnEnable()
    {
        if (handle == null)
            handle = new BoxBoundsHandle();
    }

    private void OnSceneGUI()
    {
        BoxButton button = (BoxButton)target;

        Matrix4x4 rotatedMatrix = Handles.matrix * Matrix4x4.TRS(button.transform.position, button.transform.rotation, Vector3.one);
        using (new Handles.DrawingScope(rotatedMatrix))
        {
            Undo.RecordObject(button, "BoxButton Bounds Handle");
            handle.center = button.bounds.center;
            handle.size = button.bounds.size;
            handle.SetColor(Color.yellow);
            handle.DrawHandle();
            button.bounds.center = handle.center;
            button.bounds.size = handle.size;
        }
    }
}
#endif