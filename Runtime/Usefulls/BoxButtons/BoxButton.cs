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
    [Header("Events")]
    public UnityEvent onClickDown = new UnityEvent();
    public UnityEvent onGainHover = new UnityEvent();
    public UnityEvent onLoseHover = new UnityEvent();

    private bool hasHover;

    private void Update()
    {
        if (camera == null)
            return;

        if (camera.MouseOverlapsBounds(bounds, transform))
        {
            if(!hasHover)
            {
                hasHover = true;
                if (onGainHover != null)
                    onGainHover.Invoke();
            } else
            {
                if (Input.GetMouseButtonDown(0) && onClickDown != null)
                    onClickDown.Invoke();
            }
        }
        else 
        {
            if (hasHover)
            {
                hasHover = false;
                if(onLoseHover != null)
                    onLoseHover.Invoke();
            }
        }
    }



    [RequireComponent(typeof(BoxButton))]
    public class Extension : MonoBehaviour
    {
        protected BoxButton bb;

        private void Awake()
        {
            bb = GetComponent<BoxButton>();
        }

        private void OnEnable()
        {
            bb.onClickDown.AddListener(OnClickDown);
            bb.onGainHover.AddListener(OnGainHover);
            bb.onLoseHover.AddListener(OnLoseHover);
        }

        private void OnDisable()
        {
            bb.onClickDown.RemoveListener(OnClickDown);
            bb.onGainHover.RemoveListener(OnGainHover);
            bb.onLoseHover.RemoveListener(OnLoseHover);
        }

        protected virtual void OnClickDown() { }

        protected virtual void OnGainHover() { }
        
        protected virtual void OnLoseHover() { }

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