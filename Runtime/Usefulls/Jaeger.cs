using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Mania
{
    public class Jaeger : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool visible = true;

        public List<RigPeice> list = new List<RigPeice>();

        void OnDrawGizmos()
        {
            if (!visible)
                return;
            foreach (RigPeice peice in list)
            {
                DrawShape(peice);
            }
        }

        Vector3[] GetShape(RigPeice.Shape shape)
        {
            switch (shape)
            {
                case RigPeice.Shape.Disc:
                    return RigPeice.disc;
                case RigPeice.Shape.Square:
                    return RigPeice.square;
                case RigPeice.Shape.Arrow:
                    return RigPeice.arrow;
                case RigPeice.Shape.Cog:
                    return RigPeice.cog;
                case RigPeice.Shape.Cardinal:
                    return RigPeice.cardinal;
                default:
                    return RigPeice.square;
            }
        }


        void DrawShape(RigPeice peice)
        {
            
            Vector3[] shape = GetShape(peice.shape);

            Quaternion rotation;

            switch (peice.orientation)
            {
                case RigPeice.Orientation.Up:
                    rotation = transform.rotation;
                    break;
                case RigPeice.Orientation.Right:
                    rotation = transform.rotation * Quaternion.Euler(0f, 0f, 90f);
                    break;
                case RigPeice.Orientation.Forward:
                    rotation = transform.rotation * Quaternion.Euler(90f, 0f, 0f);
                    break;
                case RigPeice.Orientation.GlobalUp:
                    rotation = Quaternion.identity;
                    break;
                case RigPeice.Orientation.GlobalRight:
                    rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
                case RigPeice.Orientation.GlobalForward:
                    rotation = Quaternion.Euler(90f, 0f, 0f);
                    break;
                default:
                    rotation = Quaternion.identity;
                    break;
            }



            bool selected = Selection.Contains(this.gameObject);
            Gizmos.color = selected ? peice.activeColor : peice.color;

            float lossyScaleAgg = (transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) * 0.33f;
            
            for (int i = 0; i < shape.Length - 1; i++)
            {
                Vector3 start = rotation * (shape[i] * peice.size * lossyScaleAgg);
                Vector3 end = rotation * (shape[i + 1] * peice.size * lossyScaleAgg);

                Vector3 offset = peice.globalOffset * lossyScaleAgg + transform.TransformVector(peice.localOffset);
                for (int a = 0; a < peice.array; a++)
                {
                    Vector3 arrayOffset = offset + (peice.globalArrayOffset * a) + (transform.TransformVector(peice.localArrayOffset) * a);
                    Gizmos.DrawLine(transform.position + start + arrayOffset, transform.position + end + arrayOffset);
                }
            }
        }
#endif
    }
#if UNITY_EDITOR
    [System.Serializable]
    public class RigPeice
    {
        public bool expanded;

        public static readonly Vector3[] disc = {
        Vector3.forward, new Vector3(0.707f, 0f, 0.707f), Vector3.right, new Vector3(0.707f, 0f, -0.707f), Vector3.back, new Vector3(-0.707f, 0f, -0.707f), Vector3.left, new Vector3(-0.707f, 0f, 0.707f), Vector3.forward };
        public static readonly Vector3[] square = {
        new Vector3(1f, 0f, 1f),
        new Vector3(-1f, 0f, 1f),
        new Vector3(-1f, 0f, -1f),
        new Vector3(1f, 0f, -1f),
        new Vector3(1f, 0f, 1f) };
        public static readonly Vector3[] arrow = {
        new Vector3(0f, 0f, 0f),
        new Vector3(0f, 1f, 0f),
        new Vector3(0.2f, 0.8f, 0f),
        new Vector3(-0.2f, 0.8f, 0f),
        new Vector3(0f, 1f, 0f) };
        public static readonly Vector3[] cog = {
        new Vector3(0.2f, 0, 1),
        new Vector3(0.2f, 0, 0.8f),
        new Vector3(0.462f,0f,0.69f),
        new Vector3(0.606f,0f,0.834f),
        new Vector3(0.834f,0f,0.606f),
        new Vector3(0.69f,0f,0.462f),
        new Vector3(0.8f,0f,0.2f),
        new Vector3(1f, 0f, 0.2f),
        new Vector3(1f, 0f, -0.2f),
        new Vector3(0.8f, 0f, -0.2f),
        new Vector3(0.69f, 0f, -0.462f),
        new Vector3(0.834f, 0f, -0.606f),
        new Vector3(0.606f, 0f, -0.834f),
        new Vector3(0.462f, 0f, -0.69f),
        new Vector3(0.2f, 0f, -0.8f),
        new Vector3(0.2f, 0f, -1f),
        new Vector3(-0.2f, 0f, -1f),
        new Vector3(-0.2f, 0, -1f),
        new Vector3(-0.2f, 0, -0.8f),
        new Vector3(-0.462f, 0f, -0.69f),
        new Vector3(-0.606f, 0f, -0.834f),
        new Vector3(-0.834f, 0f, -0.606f),
        new Vector3(-0.69f, 0f, -0.462f),
        new Vector3(-0.8f, 0f, -0.2f),
        new Vector3(-1f, 0f, -0.2f),
        new Vector3(-1f, 0f, 0.2f),
        new Vector3(-0.8f, 0f, 0.2f),
        new Vector3(-0.69f, 0f, 0.462f),
        new Vector3(-0.834f, 0f, 0.606f),
        new Vector3(-0.606f, 0f, 0.834f),
        new Vector3(-0.462f, 0f, 0.69f),
        new Vector3(-0.2f, 0f, 0.8f),
        new Vector3(-0.2f, 0f, 1f),
        new Vector3(0.2f, 0f, 1f) };
        public static readonly Vector3[] cardinal = {
        new Vector3(0f, 0f, 1f),
        new Vector3(0.3f, 0f, 0.7f),
        new Vector3(0.15f, 0f, 0.7f),
        new Vector3(0.3f, 0f, 0.3f),
        new Vector3(0.7f, 0f, 0.15f),
        new Vector3(0.7f, 0f, 0.3f),
        new Vector3(1f, 0f, 0f),
        new Vector3(0.7f,0f,-0.3f),
        new Vector3(0.7f,0f,-0.15f),
        new Vector3(0.3f,0f,-0.3f),
        new Vector3(0.15f, 0f, -0.7f),
        new Vector3(0.3f, 0f, -0.7f),
        new Vector3(0f, 0f, -1f),
        new Vector3(-0.3f, 0f, -0.7f),
        new Vector3(-0.15f, 0f, -0.7f),
        new Vector3(-0.3f, 0f, -0.3f),
        new Vector3(-0.7f, 0f, -0.15f),
        new Vector3(-0.7f, 0f, -0.3f),
        new Vector3(-1f, 0f, 0f),
        new Vector3(-0.7f, 0f, 0.3f),
        new Vector3(-0.7f, 0f, 0.15f),
        new Vector3(-0.3f, 0f, 0.3f),
        new Vector3(-0.15f, 0f, 0.7f),
        new Vector3(-0.3f, 0f, 0.7f),
        new Vector3(0f, 0f, 1f)};

        public enum Shape
        {
            Disc,
            Square,
            Arrow,
            Cog,
            Cardinal
        }


        public Shape shape;

        public enum Orientation
        {
            Up,
            Right,
            Forward,
            GlobalUp,
            GlobalRight,
            GlobalForward,
        }
        public Orientation orientation;

        public float size = 0.1f;
        public Color color = Color.yellow;
        public Color activeColor = Color.white;

        public Vector3 localOffset;
        public Vector3 globalOffset;

        public int array = 1;
        public Vector3 localArrayOffset;
        public Vector3 globalArrayOffset;

        public RigPeice Duplicate()
        {
            RigPeice rigPeice = new RigPeice();
            rigPeice.shape = shape;
            rigPeice.orientation = orientation;
            rigPeice.size = size;
            rigPeice.color = color;
            rigPeice.activeColor = activeColor;
            rigPeice.localOffset = localOffset;
            rigPeice.globalOffset = globalOffset;
            rigPeice.array = array;
            rigPeice.localArrayOffset = localArrayOffset;
            rigPeice.globalArrayOffset = globalArrayOffset;
            return rigPeice;
        }
    }


    [CustomEditor(typeof(Jaeger))]
    [CanEditMultipleObjects]
    public class Jaeger_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            
            bool changed = false;
            Jaeger handles = (Jaeger)target;
            Undo.RecordObject(handles, "Change in Jaeger Inspector");
            handles.visible = GUILayout.Toggle(handles.visible, "Visible");
            foreach (RigPeice peice in handles.list)
            {
                Color guiC = GUI.color;
                GUI.color = Color.Lerp(peice.color, Color.white, 0.5f);
                peice.expanded = EditorGUILayout.BeginFoldoutHeaderGroup(peice.expanded, $"{peice.shape.ToString()}, {peice.size}, {peice.orientation.ToString()}");
                GUI.color = guiC;
                if (peice.expanded)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20f);
                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Duplicate"))
                        handles.list.Add(peice.Duplicate()); changed = true;
                    if (GUILayout.Button("Delete"))
                        handles.list.Remove(peice); changed = true;
                    GUILayout.EndHorizontal();
                    peice.shape = (RigPeice.Shape)EditorGUILayout.EnumPopup("Shape:", peice.shape);
                    peice.orientation = (RigPeice.Orientation)EditorGUILayout.EnumPopup("Orientation:", peice.orientation);
                    peice.size = EditorGUILayout.FloatField("Size: ",peice.size);
                    peice.localOffset = EditorGUILayout.Vector3Field("Local Offset:", peice.localOffset);
                    peice.globalOffset = EditorGUILayout.Vector3Field("Global Offset:", peice.globalOffset);
                    peice.color = EditorGUILayout.ColorField("Color:",peice.color);
                    peice.activeColor = EditorGUILayout.ColorField("Selected Color:",peice.activeColor);
                    peice.array = Mathf.Clamp(EditorGUILayout.IntField("Array:", peice.array), 1, 25);
                    if(peice.array > 1)
                    {
                        peice.localArrayOffset = EditorGUILayout.Vector3Field("Local Array Offset:", peice.localArrayOffset);
                        peice.globalArrayOffset = EditorGUILayout.Vector3Field("Global Array Offset:", peice.globalArrayOffset);
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Add New"))
                handles.list.Add(new RigPeice()); changed = true;

            if (changed || GUI.changed)
            {
                SceneView.RepaintAll();
            }

        }

    }
#endif

}