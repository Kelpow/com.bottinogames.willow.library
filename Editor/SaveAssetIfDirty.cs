#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class SaveAssetIfDirty
{
    [MenuItem("Assets/Save Asset if dirty", priority = 39)]
    public static void SaveAssetIfDirtyCommand()
    {
        foreach (var obj in Selection.objects)
        {
            if (AssetDatabase.IsMainAsset(obj))
            {
                AssetDatabase.SaveAssetIfDirty(obj);
            }
        }
    }
    
    [MenuItem("Assets/Save All Asssets", priority = 39)]
    public static void SaveAssetAllAssetsCommand()
    {
        foreach (var obj in Selection.objects)
        {
            if (AssetDatabase.IsMainAsset(obj))
            {
                AssetDatabase.SaveAssetIfDirty(obj);
            }
        }
    }
}
#endif