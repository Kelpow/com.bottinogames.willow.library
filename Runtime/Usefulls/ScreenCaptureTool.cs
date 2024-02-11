using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScreenCaptureTool
{
    const string firstChars = "BCDFGHJKLMNPQRSTVWXZ";
    const string secondChars = "AEIOUY";
    const string thirdChars = "ABDEFGIJKLMNOPRSTVXZ";

#if UNITY_EDITOR
    [MenuItem("ScreenCap/Capture Screenshot/1x", priority = 1)]
#endif
    public static void CaptureScreenX1()
    {
        CaptureScreen(1);
    }

#if UNITY_EDITOR
    [MenuItem("ScreenCap/Capture Screenshot/2x", priority = 2)]
#endif
    public static void CaptureScreenX2()
    {
        CaptureScreen(2);
    }

#if UNITY_EDITOR
    [MenuItem("ScreenCap/Capture Screenshot/4x", priority = 4)]
#endif
    public static void CaptureScreenX4()
    {
        CaptureScreen(4);
    }


    public static void CaptureScreen(int scale)
    {
        string date = System.DateTime.Now.ToString($"yyyy.MM.dd_h.mm_");
        string filename = $"{date}{firstChars[Random.Range(0, firstChars.Length)]}{secondChars[Random.Range(0, secondChars.Length)]}{thirdChars[Random.Range(0, thirdChars.Length)]}.png";
        string directory = Application.dataPath.Replace("Assets", "Screenshots");
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        ScreenCapture.CaptureScreenshot(Path.Combine(directory,filename), scale);
    }
}
