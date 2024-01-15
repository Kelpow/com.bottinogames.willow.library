using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Linq;
using System.Text.RegularExpressions;
#endif

public class AutoPopAttribute : PropertyAttribute
{
    public bool allowManualEdit;
    public AutoPopAttribute(bool allowManualEditing = false)
    {
        allowManualEdit = allowManualEditing;
    }
}

#if UNITY_EDITOR
// primarily based off of this script by nrjwolf: https://github.com/Nrjwolf/unity-auto-attach-component-attributes/blob/master/Scripts/Editor/AttachAttributesEditor.cs
// simplified for my tiny brain

[CustomPropertyDrawer(typeof(AutoPopAttribute))]
public class AutoPopAttributePropertyDrawer : PropertyDrawer
{
    static GUIContent _refresh;
    static GUIContent RefreshIcon
    {
        get
        {
            if(_refresh == null)
            {
                _refresh = EditorGUIUtility.IconContent("d_Refresh");
            }
            return _refresh;
        }
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        if(Application.isPlaying)
        {
            EditorGUI.PropertyField(position, property, label, true);
            property.serializedObject.ApplyModifiedProperties();
            return;
        }

        if (!(property.serializedObject.targetObject is MonoBehaviour mono))
            return;

        bool isPropertyValueNull = property.objectReferenceValue == null;

        bool cachedEnable = GUI.enabled;
        Color cachedColor = GUI.color;

        AutoPopAttribute att = (AutoPopAttribute)attribute;
        GUI.enabled = att.allowManualEdit;
        GUI.color = isPropertyValueNull ? Color.red : cachedColor;

        position.width -= 25f;
        EditorGUI.PropertyField(position, property, label, true);

        GUI.enabled = cachedEnable && !isPropertyValueNull;
        GUI.color = cachedColor;

        position.x += position.width;
        position.width = 25f;
        
        if(GUI.Button(position, RefreshIcon, EditorStyles.miniButton))
        {
            property.objectReferenceValue = null;
            isPropertyValueNull = true;
        }
        GUI.enabled = cachedEnable;
        
        if (isPropertyValueNull)
        {
            var type = GetPropertyType(property);
            GameObject go = mono.gameObject;
            property.objectReferenceValue = go.GetComponent(type);
        }

        property.serializedObject.ApplyModifiedProperties();
    }

    private Type GetPropertyType(SerializedProperty property)
    {
        string type = property.type;
        var match = Regex.Match(type, @"PPtr<\$(.*?)>");
        if (match.Success)
            type = match.Groups[1].Value;

        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).First(x => x.IsSubclassOf(typeof(Component)) && x.Name == type);
    }
}
#endif