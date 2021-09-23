using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(-100), ExecuteAlways] 
public class TransformSync : MonoBehaviour
{
    public enum SyncMode
    {
        None,
        Local,
        World
    }

    public enum SyncTime
    {
        Update,
        LateUpdate
    }

    public Transform target;

    [Space(10)]
    public SyncTime syncTime;

    [Space(10)]
    public SyncMode positionSync;
    public SyncMode rotationSync;
    public SyncMode scaleSync;

    [HideInInspector]
    public bool executeInEditMode;
    [HideInInspector]
    public bool reverseSync;

    private Transform oldTarget;
    private Vector3 oldPosition;
    private Quaternion oldRotation;
    private Vector3 oldScale;

    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && !executeInEditMode)
            return;
#endif

        if (syncTime == SyncTime.Update)
                Sync();
    }
    private void LateUpdate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && !executeInEditMode)
            return;
#endif

        if (syncTime == SyncTime.LateUpdate)
                Sync();
    }

    void Sync()
    {
        if (!target)
            return;

        if (!reverseSync)
        {
            switch (positionSync)
            {
                case SyncMode.Local:
                    transform.localPosition = target.localPosition;
                    break;
                case SyncMode.World:
                    transform.position = target.position;
                    break;
            }

            switch (rotationSync)
            {
                case SyncMode.Local:
                    transform.localRotation = target.localRotation;
                    break;
                case SyncMode.World:
                    transform.rotation = target.rotation;
                    break;
            }

            switch (scaleSync)
            {
                case SyncMode.Local:
                    transform.localScale = target.localScale;
                    break;
            }
        }
        else
        {
            bool syncTargetToThisPos = false;
            bool syncThisToTargetPos = false;

            if (positionSync != SyncMode.None)
            {
                Vector3 targetPosition = positionSync == SyncMode.World ? target.position : target.localPosition;
                Vector3 thisPosition = positionSync == SyncMode.World ? transform.position : transform.localPosition;

                if (targetPosition != oldPosition)
                    syncThisToTargetPos = true;
                else if (thisPosition != oldPosition)
                    syncTargetToThisPos = true;

                if (syncThisToTargetPos)
                {
                    switch (positionSync)
                    {
                        case SyncMode.Local:
                            transform.localPosition = target.localPosition;
                            break;
                        case SyncMode.World:
                            transform.position = target.position;
                            break;
                    }
                }
                else if (syncTargetToThisPos)
                {
                    switch (positionSync)
                    {
                        case SyncMode.Local:
                            target.localPosition = transform.localPosition;
                            break;
                        case SyncMode.World:
                            target.position = transform.position;
                            break;
                    }
                }

                oldPosition = targetPosition;
            }
            
            if (rotationSync != SyncMode.None)
            {
                Quaternion targetRotation = rotationSync == SyncMode.World ? target.rotation : target.localRotation;
                Quaternion thisRotation = rotationSync == SyncMode.World ? transform.rotation : transform.localRotation;

                if (targetRotation != oldRotation)
                    syncThisToTargetPos = true;
                else if (thisRotation != oldRotation)
                    syncTargetToThisPos = true;

                if (syncThisToTargetPos)
                {
                    switch (rotationSync)
                    {
                        case SyncMode.Local:
                            transform.localRotation = target.localRotation;
                            break;
                        case SyncMode.World:
                            transform.rotation = target.rotation;
                            break;
                    }
                }
                else if (syncTargetToThisPos)
                {
                    switch (rotationSync)
                    {
                        case SyncMode.Local:
                            target.localRotation = transform.localRotation;
                            break;
                        case SyncMode.World:
                            target.rotation = transform.rotation;
                            break;
                    }
                }

                oldRotation = targetRotation;
            }


            if (scaleSync == SyncMode.Local)
            {
                Vector3 targetScale = target.localScale;
                Vector3 thisScale = transform.localScale;

                if (targetScale != oldScale)
                    syncThisToTargetPos = true;
                else if (thisScale != oldScale)
                    syncTargetToThisPos = true;

                if (syncThisToTargetPos)
                {
                    transform.localScale = target.localScale;
                }
                else if (syncTargetToThisPos)
                {
                    target.localScale = transform.localScale;
                }

                oldScale = targetScale;
            }
            
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TransformSync))]
public class TransformSyncEditor : Editor
{
    public static bool advancedExpanded;

    bool reverseSyncConfirmation;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TransformSync target = (TransformSync)base.target;

        advancedExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(advancedExpanded, "Advanced");
        if (advancedExpanded)
        {
            Undo.RecordObject(target, "TransformSync Advanced Editor");

            EditorGUI.indentLevel++;

            target.executeInEditMode = EditorGUILayout.Toggle("Execute in Edit mode", target.executeInEditMode);
            target.reverseSync = EditorGUILayout.Toggle("Reverse Sync", target.reverseSync);

            EditorGUI.indentLevel--;
        }
    }
}
#endif