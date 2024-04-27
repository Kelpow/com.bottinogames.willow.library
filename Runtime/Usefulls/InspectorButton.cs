using System;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct InspectorButton
{
#if UNITY_EDITOR
    private string method { get; }
    private bool shouldRecordUndo { get; }

    public InspectorButton(string nameof, bool recordUndo)
    {
        method = nameof;
        shouldRecordUndo = recordUndo;
    }
    public static implicit operator InspectorButton(string nameof) => new InspectorButton(nameof, true);
#else
    public static implicit operator InspectorButton(string nameof) => null;
    public InspectorButton(string nameof, bool recordUndo) {}
#endif

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InspectorButton))]
    public class InspectorButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (GUI.Button(position, label))
            {
                var target = property.serializedObject.targetObject;
                var data = (InspectorButton)fieldInfo.GetValue(target);

                var obj = property.serializedObject.targetObject;

                bool isStatic = false;

                var method = obj.GetType().GetMethod(data.method, BindingFlags.Public | BindingFlags.Instance);
                if(method == null)
                    method = obj.GetType().GetMethod(data.method, BindingFlags.NonPublic | BindingFlags.Instance);

                if(method == null)
                {
                    isStatic = true;

                    method = obj.GetType().GetMethod(data.method, BindingFlags.Public | BindingFlags.Static);
                    if (method == null)
                        method = obj.GetType().GetMethod(data.method, BindingFlags.NonPublic | BindingFlags.Static);

                    if(method == null)
                    {
                        Debug.LogError($"No function of name \"{data.method}\" was found");
                        return;
                    }
                }

                if (method.GetParameters().Length > 0)
                {
                    Debug.LogError("Inspector Buttons will only work on functions with no input parameters.");
                    return;
                }

                if(data.shouldRecordUndo)
                    Undo.RecordObject(target, label.text);

                object methodOut = isStatic ? method.Invoke(null, null) : method.Invoke(obj, null);

                if (!data.shouldRecordUndo || methodOut == null) return;

                if (methodOut is UnityEngine.Object changedobj)
                    Undo.RecordObject(changedobj, label.text);
                else if(methodOut is UnityEngine.Object[] changedobjs)
                    foreach (var changed in changedobjs)
                        Undo.RecordObject(changed, label.text);

                Undo.FlushUndoRecordObjects();
            }
        }
    }
#endif
}
