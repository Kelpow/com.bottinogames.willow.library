using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif


namespace Willow.IDLUI 
{
    public class IDLUIButton : MonoBehaviour
    {
        

        private void OnEnable() { Manager.AddActiveButton(this);}

        private void OnDisable() { Manager.RemoveActiveButton(this); }

        public Manager.Category category;

        public int priority = 0;

        public Bounds bounds = new Bounds(Vector3.zero, Vector3.one);

        public UnityEvent onSelect = new UnityEvent();
        public UnityEvent onGainFocus = new UnityEvent();
        public UnityEvent onLoseFocus = new UnityEvent();

        [HideInInspector]
        public IDLUIButton up;
        [HideInInspector]
        public IDLUIButton down;
        [HideInInspector]
        public IDLUIButton left;
        [HideInInspector]
        public IDLUIButton right;


        //Built-In Behaviours
        public void SetActiveCategory(Manager.Category category) { Manager.activeCategory = category; }

        public void ForceFocus(IDLUIButton button) { Manager.FocusButton(button); }


        public void CloseApplication(float delay) { StartCoroutine(CloseAfterDelay(delay)); }
        private IEnumerator CloseAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Application.Quit();
        }


        //Extension Class
        [RequireComponent(typeof(IDLUIButton))]
        public class Extension : MonoBehaviour
        {
            IDLUIButton button;

            private void Awake() { button = GetComponent<IDLUIButton>(); }

            private void OnEnable() 
            {
                button = GetComponent<IDLUIButton>();
                button.onSelect.AddListener(OnSelect);
                button.onGainFocus.AddListener(OnGainFocus);
                button.onLoseFocus.AddListener(OnLoseFocus);
            }
            private void OnDisable() 
            {
                button.onSelect.RemoveListener(OnSelect);
                button.onGainFocus.RemoveListener(OnGainFocus);
                button.onLoseFocus.RemoveListener(OnLoseFocus);
            }

            protected virtual void OnSelect() { }
            protected virtual void OnGainFocus() { }
            protected virtual void OnLoseFocus() { }
        }


    }

#if UNITY_EDITOR
    [CustomEditor(typeof(IDLUIButton))]
    public class IDLUIButtonInspector : Editor
    {

        BoxBoundsHandle handle;

        private void OnEnable()
        {
            if (handle == null)
                handle = new BoxBoundsHandle();
        }

        private void OnSceneGUI()
        {
            IDLUIButton button = (IDLUIButton)target;

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

            Handles.color = Color.green;
            if (button.up)
                Handles.DrawLine(button.transform.position, button.up.transform.position);
            Handles.color = Color.blue;
            if (button.down)
                Handles.DrawLine(button.transform.position, button.down.transform.position);
            Handles.color = Color.red;
            if (button.right)
                Handles.DrawLine(button.transform.position, button.right.transform.position);
            Handles.color = Color.yellow;
            if (button.left)
                Handles.DrawLine(button.transform.position, button.left.transform.position);

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            IDLUIButton button = (IDLUIButton)target;

            var up = serializedObject.FindProperty("up");
            var down = serializedObject.FindProperty("down");
            var left = serializedObject.FindProperty("left");
            var right = serializedObject.FindProperty("right");

            GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(up);
                if (GUILayout.Button("Reciprocate", GUILayout.Width(70)) && button.up)
                    button.up.down = button;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(down);
                if (GUILayout.Button("Reciprocate", GUILayout.Width(70)) && button.down)
                    button.down.up = button;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(left);
                if (GUILayout.Button("Reciprocate", GUILayout.Width(70)) && button.left)
                    button.left.right = button;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(right);
                if (GUILayout.Button("Reciprocate", GUILayout.Width(70)) && button.right)
                    button.right.left = button;
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }


#endif
}