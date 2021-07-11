using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
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

    

    private void Update()
    {
        if (syncTime == SyncTime.Update)
            Sync();
    }
    private void LateUpdate()
    {
        if (syncTime == SyncTime.LateUpdate)
            Sync();
    }

    void Sync()
    {
        if (!target)
            return;

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
}
