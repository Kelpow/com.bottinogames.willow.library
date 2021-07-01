using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Willow
{
    public class GUI
    {
        public void OutlinedLable(string text, GUIStyle style, Color foreground, Color background, params GUILayoutOption[] options)
        {
            OutlinedLable(new GUIContent(text), style, foreground, background, options);
        }
        public void OutlinedLable(GUIContent content, GUIStyle style, Color foreground, Color background, params GUILayoutOption[] options)
        {
            style.normal.textColor = background;

            Rect textRect = GUILayoutUtility.GetRect(content, style, options);
            textRect.x--;
            textRect.y--;

            UnityEngine.GUI.Label(textRect, content, style);
            textRect.x++;
            UnityEngine.GUI.Label(textRect, content, style);
            textRect.x++;
            UnityEngine.GUI.Label(textRect, content, style);
            textRect.y++;
            UnityEngine.GUI.Label(textRect, content, style);
            textRect.y++;
            UnityEngine.GUI.Label(textRect, content, style);
            textRect.x--;
            UnityEngine.GUI.Label(textRect, content, style);
            textRect.x--;
            UnityEngine.GUI.Label(textRect, content, style);
            textRect.y--;
            UnityEngine.GUI.Label(textRect, content, style);
            textRect.x++;

            style.normal.textColor = foreground;
            UnityEngine.GUI.Label(textRect, content, style);
        }
    }
}