using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class SelectionReassignment : MonoBehaviour
{
    public GameObject target;
#if UNITY_EDITOR
    private void OnEnable()
    {
        Selection.selectionChanged += SelectionChanged;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= SelectionChanged;
    }

    private bool targetSelectedLast = false;
    private bool retarget;

    [ContextMenu("Test")]
    void SelectionChanged()
    {
        Debug.Log("test");

        if (!target)
            return;

        if(Selection.activeGameObject == this.gameObject)
        {
            if(!targetSelectedLast)
                retarget = true;
            targetSelectedLast = true;
        }
        else if (Selection.activeGameObject == target)
        {
            targetSelectedLast = true;
        }
        else
        {
            targetSelectedLast = false;
        }
    }

    private void Update()
    {
        if (retarget)
        {
            Selection.SetActiveObjectWithContext(target, target);
            retarget = false;
        }
    }
#endif
}
