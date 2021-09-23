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
        {
            if (oldTarget != target ||
                (positionSync != SyncMode.None && (positionSync == SyncMode.Local ? target.localPosition : target.position) != oldPosition) ||
                (rotationSync != SyncMode.None && (rotationSync == SyncMode.Local ? target.localRotation : target.rotation) != oldRotation) ||
                (scaleSync == SyncMode.Local && transform.localScale != oldScale)
                )
            {
                Sync();
            }
        }
    }
    private void LateUpdate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && !executeInEditMode)
            return;
#endif

        if (syncTime == SyncTime.LateUpdate)
        {
            if (oldTarget != target ||
                (positionSync != SyncMode.None && (positionSync == SyncMode.Local ? target.localPosition : target.position) != oldPosition) ||
                (rotationSync != SyncMode.None && (rotationSync == SyncMode.Local ? target.localRotation : target.rotation) != oldRotation) ||
                (scaleSync == SyncMode.Local && transform.localScale != oldScale)
                )
            {
                Sync();
            }
        }
    }

    void Sync()
    {
        oldTarget = target;
        if (!target)
            return;

        switch (positionSync)
        {
            case SyncMode.Local:
                transform.localPosition = target.localPosition;
                oldPosition = target.localPosition;
                break;
            case SyncMode.World:
                transform.position = target.position;
                oldPosition = target.position;
                break;
        }

        switch (rotationSync)
        {
            case SyncMode.Local:
                transform.localRotation = target.localRotation;
                oldRotation = target.localRotation;
                break;
            case SyncMode.World:
                transform.rotation = target.rotation;
                oldRotation = target.rotation;
                break;
        }

        switch (scaleSync)
        {
            case SyncMode.Local:
                transform.localScale = target.localScale;
                oldScale = target.localScale;
                break;
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

            target.executeInEditMode = EditorGUILayout.ToggleLeft("Execute in Edit mode", target.executeInEditMode);

            if (GUILayout.Button("Setup Reverse Sync"))
            {
                if (target.target)
                {
                    bool toSync = true;
                    TransformSync preexistingSync = null;
                    TransformSync[] targetSyncs = target.target.GetComponents<TransformSync>();
                    if(targetSyncs.Length > 0)
                    {
                        foreach (TransformSync sync in targetSyncs)
                        {
                            if (sync.target == target.transform)
                            {
                                if (!EditorUtility.DisplayDialog("Overwrite Transform Sync?", "A Transform Sync targeting this object already exists on the target. Do you wish to overwrite the settings of that Transform Sync?", "Overwrite", "Cancel"))
                                    toSync = false;
                                else
                                    preexistingSync = sync;
                                
                                break;
                            }
                        }
                    }

                    if (toSync)
                    {
                        TransformSync targetSync;
                        if (preexistingSync)
                            targetSync = preexistingSync;
                        else
                            targetSync = ObjectFactory.AddComponent(target.target.gameObject, typeof(TransformSync)) as TransformSync;

                        targetSync.target = target.transform;
                        targetSync.syncTime = target.syncTime;
                        targetSync.positionSync = target.positionSync;
                        targetSync.rotationSync = target.rotationSync;
                        targetSync.scaleSync = target.scaleSync;
                        targetSync.executeInEditMode = target.executeInEditMode;

                        if (target.executeInEditMode)
                            target.SendMessage("Sync");
                    }

                } 
                else
                    Debug.LogWarning("A target must be provided to set up Reverse Sync");
            }

            EditorGUI.indentLevel--;
        }
    }
}
#endif