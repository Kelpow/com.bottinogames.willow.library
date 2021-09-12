using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LocalTransformAnimator : MonoBehaviour
{
    public bool isRealtime = false;

    public AnimationAxis positionX;
    public AnimationAxis positionY;
    public AnimationAxis positionZ;

    public AnimationAxis rotationX;
    public AnimationAxis rotationY;
    public AnimationAxis rotationZ;

    private void Update()
    {
        float t = isRealtime ? Time.unscaledTime : Time.time;

        transform.localPosition = new Vector3(
            positionX.mode == AnimationAxis.Mode.None ? transform.localPosition.x : positionX.Evaluate(t),
            positionY.mode == AnimationAxis.Mode.None ? transform.localPosition.y : positionY.Evaluate(t),
            positionZ.mode == AnimationAxis.Mode.None ? transform.localPosition.z : positionZ.Evaluate(t)
            );

        transform.rotation = Quaternion.Euler(new Vector3(
            rotationX.Evaluate(t) * 360f,
            rotationY.Evaluate(t) * 360f,
            rotationZ.Evaluate(t) * 360f
            ));


    }



    
}

[System.Serializable]
public struct AnimationAxis
{
    public Mode mode;
    public float speed;
    public float scale;
    [Range(0,1)]
    public float offset;
    public float Evaluate(float time)
    {

        float t = (time + offset) * speed;

        switch (mode)
        {
            case Mode.None:
                return 0f;
            case Mode.Constant:
                break;
            case Mode.Sin:
                t = Sin(t);
                break;
            case Mode.Saw:
                t = Saw(t);
                break;
            case Mode.Triangle:
                t = Triangle(t);
                break;
            case Mode.Static:
                t = 1f;
                break;
        }

        return t * scale;
    }
    public enum Mode
    {
        None,
        Constant,
        Sin,
        Saw,
        Triangle,
        Static
    }
    private static float Constant(float t) { return t; }
    private static float Sin(float t) { return Mathf.Sin(t * Mathf.PI); }
    private static float Saw(float t) { return (t % 2) - 1; }
    private static float Triangle(float t) { return Mathf.Abs((Mathf.Abs(t) % 4) - 2) - 1; }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(AnimationAxis))]
public class AnimationAxisDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        {

            Rect labelRect = new Rect(position.x, position.y, 80, position.height);
            position.x += 80 + 5;
            Rect modeRect = new Rect(position.x, position.y, 75, position.height);
            position.x += 75 + 5;
            Rect speedLabelRect = new Rect(position.x, position.y, 10, position.height);
            position.x += 10 + 5;
            Rect speedRect = new Rect(position.x, position.y, 40, position.height);
            position.x += 40 + 5;
            Rect scaleLabelRect = new Rect(position.x, position.y, 10, position.height);
            position.x += 10 + 5;
            Rect scaleRect = new Rect(position.x, position.y, 40, position.height);
            position.x += 40 + 5;
            Rect offsetLabelRect = new Rect(position.x, position.y, 10, position.height);
            position.x += 10 + 5;
            Rect offsetRect = new Rect(position.x, position.y, 40, position.height);

            EditorGUI.LabelField(labelRect, label);
            EditorGUI.PropertyField(modeRect, property.FindPropertyRelative("mode"), GUIContent.none);
            EditorGUI.LabelField(speedLabelRect, "P");
            EditorGUI.PropertyField(speedRect, property.FindPropertyRelative("speed"), GUIContent.none);
            EditorGUI.LabelField(scaleLabelRect, "A");
            EditorGUI.PropertyField(scaleRect, property.FindPropertyRelative("scale"), GUIContent.none);
            EditorGUI.LabelField(offsetLabelRect, "O");
            EditorGUI.PropertyField(offsetRect, property.FindPropertyRelative("offset"), GUIContent.none);
        }
        EditorGUI.EndProperty();
    }
}
#endif