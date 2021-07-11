using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Willow.Editor
{
    public static class GUI
    {
        public static bool ObjectArrayField<T>(bool expanded, string label, ref T[] array)
        {
            

            if (!typeof(T).IsSubclassOf(typeof(UnityEngine.Object)))
                throw new Exception("T must by UnityEngine.Object or a subclass.");



            expanded = EditorGUILayout.BeginFoldoutHeaderGroup(expanded, label);

            if (expanded)
            {

                EditorGUI.indentLevel++;
                if (array == null)
                {
                    int l = EditorGUILayout.DelayedIntField("Size", 0);
                    if (l > 0)
                        array = new T[l];

                }
                else
                {
                    int l = EditorGUILayout.DelayedIntField("Size", array.Length);
                    if (l != array.Length)
                    {
                        T[] newArray = new T[Mathf.Max(l,0)];
                        for (int i = 0; i < l; i++)
                        {
                            if (i < array.Length)
                                newArray[i] = array[i];
                            else if (array.Length != 0)
                                newArray[i] = array[array.Length - 1];
                        }

                        array = newArray;
                    }

                    for (int i = 0; i < array.Length; i++)
                    {
                        EditorGUILayout.ObjectField($"Element {i}", array[i] as UnityEngine.Object, typeof(T), allowSceneObjects: false);
                    }
                }
                EditorGUI.indentLevel--;
            }

            return expanded;
        }
    }
}