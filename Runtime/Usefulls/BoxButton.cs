using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Willow.Library;
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
            if (camera.MouseOverlapsBounds(bounds))
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

        Matrix4x4 rotatedMatrix = button.transform.localToWorldMatrix;
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