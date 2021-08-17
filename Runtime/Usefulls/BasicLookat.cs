using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BasicLookat : MonoBehaviour
{
    public Transform target;
    public Transform upTarget;

#if UNITY_EDITOR
    public bool runInEditMode = false;
#endif

    void LateUpdate()
    {
#if UNITY_EDITOR
        if (!runInEditMode && !Application.isPlaying)
            return;
#endif
        if (target)
        {
            Vector3 up = upTarget ? upTarget.position - transform.position : Vector3.up;
            
            transform.LookAt(target, up);
        }
    }
}
