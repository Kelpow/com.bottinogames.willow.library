using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

[EditorTool("3D Cursor Tool")]
public class CursorTool : EditorTool
{
    Transform target;
    Vector3 position;

    public override void OnToolGUI(EditorWindow window)
    {
        base.OnToolGUI(window);

        Transform active = Selection.activeTransform;
        if(active)
        {
            if(target != active)
            {
                target = active;
                position = target.position;
            }
        } else
        {
            target = null;
            return;
        }

        Quaternion rotation = target.rotation;
        float scale = 1f;

        EditorGUI.BeginChangeCheck();
        Handles.TransformHandle(ref position, ref rotation, ref scale);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "3D Cursor Tool");

            Vector3 localPivot = target.InverseTransformPoint(position);

            target.rotation = rotation;

            Vector3 newPivot = target.TransformPoint(localPivot);

            Vector3 diff = position - newPivot;

            target.position += diff;
        }


    }
}
