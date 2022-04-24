using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(order = 325)]
public class GradientTextureLibrary : ScriptableObject
{
    public enum Resolution
    {
        _8 = 8,
        _16 = 16,
        _32 = 32,
        _64 = 64,
        _128 = 128,
        _256 = 256,
        _512 = 512,
        _1024 = 1024,
        _2048 = 2048,
    }

    public List<Data> textureData = new List<Data>();

    [System.Serializable]
    public class Data
    {
        [Delayed] public string name = "New Gradient";
        public Gradient gradient = new Gradient();
        [HideInInspector] public Texture2D texture;

        public Resolution resolution = Resolution._256;
        public TextureFormat textureFormat = TextureFormat.RGBA32;
        public bool generateMipMaps = false;

        public bool foldout = false;
    }

    public static void WriteGradientToTexture(Texture2D tex, Gradient gradient)
    {
        for (int x = 0; x < tex.width; x++)
        {
            float t = (float)x / (tex.width - 1);
            for (int y = 0; y < tex.height; y++)
            {
                tex.SetPixel(x, y, gradient.Evaluate(t));
            }
        }
        tex.Apply();
    }


}

#if UNITY_EDITOR
[CustomEditor(typeof(GradientTextureLibrary))]
public class GradientTextureLibraryEditor : Editor
{
    new GradientTextureLibrary target;

    private void OnEnable()
    {
        target = (GradientTextureLibrary)base.target;
    }

    public override void OnInspectorGUI()
    {
        for (int i = 0; i < target.textureData.Count; i++)
        {
            EditorGUI.indentLevel = 0;
            GradientTextureLibrary.Data data = target.textureData[i];

            GUILayout.BeginVertical("HelpBox");
            {
                GUILayout.BeginHorizontal();
                {
                    string newName = EditorGUILayout.DelayedTextField(data.name);
                    if (data.name != newName)
                    {
                        data.name = newName;
                        data.texture.name = newName;

                        Reimport(target);
                        EditorApplication.delayCall += DelayedSort(target.textureData);
                    }
                }
                GUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();
                Gradient newGradient = EditorGUILayout.GradientField(data.gradient);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RegisterCompleteObjectUndo(target, "Edit Gradient Library");
                    Undo.RegisterCompleteObjectUndo(data.texture, "Edit Gradient Library");
                    data.gradient = newGradient;
                    GradientTextureLibrary.WriteGradientToTexture(data.texture, data.gradient);
                }

                EditorGUI.BeginChangeCheck();
                TextureWrapMode newWrap = (TextureWrapMode)EditorGUILayout.EnumPopup("Wrap Mode", data.texture.wrapMode);
                FilterMode newFilter = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", data.texture.filterMode);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RegisterCompleteObjectUndo(data.texture, "Edit Gradient Library");
                    data.texture.wrapMode = newWrap;
                    data.texture.filterMode = newFilter;
                }
                EditorGUI.indentLevel = 1;
                if (data.foldout = EditorGUILayout.Foldout(data.foldout, "Advanced"))
                {
                    EditorGUI.BeginChangeCheck();
                    GradientTextureLibrary.Resolution newRes = (GradientTextureLibrary.Resolution)EditorGUILayout.EnumPopup("Resolution", (GradientTextureLibrary.Resolution)data.texture.width);
                    TextureFormat newFormat = (TextureFormat)EditorGUILayout.EnumPopup("Texture Format", data.texture.format);
                    bool newMip = EditorGUILayout.Toggle("Generate Mip Maps", data.texture.mipmapCount != 1);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RegisterCompleteObjectUndo(target, "Edit Gradient Library");
                        Undo.RegisterCompleteObjectUndo(data.texture, "Edit Gradient Library");
                        data.texture.Resize((int)newRes, 1, newFormat, newMip);
                        GradientTextureLibrary.WriteGradientToTexture(data.texture, data.gradient);
                    }
                }
                EditorGUI.indentLevel = 0;

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Delete") && EditorUtility.DisplayDialog("Delete Gradient?", "Are you sure you want to destroy this gradient? This cannot be undone and will clear the undo history for this object for this library.", "Destroy", "Cancel"))
                    {
                        GUI.FocusControl(null);

                        Undo.ClearUndo(target);
                        foreach (var d in target.textureData)
                            Undo.ClearUndo(d.texture);

                        DestroyImmediate(data.texture, true);

                        EditorApplication.delayCall += DelayedRemove(target.textureData, data);

                        EditorUtility.SetDirty(target);

                        Reimport(target);

                        EditorApplication.delayCall += DelayedSort(target.textureData);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Gradient Texture") && EditorUtility.DisplayDialog("Add Gradient?", "Are you sure you want to add a gradient? This will clear the undo history for this library.", "Add", "Cancel"))
        {
            Undo.ClearUndo(target);
            foreach (var data in target.textureData)
                Undo.ClearUndo(data.texture);

            GradientTextureLibrary.Data newData = new GradientTextureLibrary.Data();
            GenerateTexture(newData);
            target.textureData.Add(newData);
        }
    }

    private void GenerateTexture(GradientTextureLibrary.Data data)
    {
        Texture2D newTexture = new Texture2D((int)data.resolution, 1, data.textureFormat, data.generateMipMaps);
        newTexture.name = data.name;
        newTexture.filterMode = FilterMode.Bilinear;
        newTexture.wrapMode = TextureWrapMode.Clamp;

        GradientTextureLibrary.WriteGradientToTexture(newTexture, data.gradient);

        data.texture = newTexture;

        AssetDatabase.AddObjectToAsset(data.texture, target);
        Reimport(target);
    }


    public static EditorApplication.CallbackFunction DelayedSort(List<GradientTextureLibrary.Data> list)
    {
        list.Sort((a,b)=>b.name.CompareTo(a.name));
        return null;
    }
    
    public static EditorApplication.CallbackFunction DelayedRemove(List<GradientTextureLibrary.Data> list, GradientTextureLibrary.Data toRemove)
    {
        list.Remove(toRemove);
        return null;
    }


    static bool toReimport;
    public void Reimport(Object target)
    {
        if (toReimport)
            return;

        toReimport = true;

        EditorApplication.delayCall += ReimportAsset(target);
    }
    public static EditorApplication.CallbackFunction ReimportAsset(Object target)
    {
        //Black magic to get the project view to refresh
        string assetName = target.name;
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), assetName + "(PROCESSING)");
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), assetName);
        AssetDatabase.Refresh();

        toReimport = false;
        return null;
    }
}
#endif


