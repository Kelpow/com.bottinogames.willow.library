using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class ScreenCaptureTool
{
    const string firstChars = "BCDFGHJKLMNPQRSTVWXZ";
    const string secondChars = "AEIOUY";
    const string thirdChars = "ABDEFGIJKLMNOPRSTVXZ";

    [MenuItem("ScreenCap/Capture Screenshot/x1")]
    public static void CaptureScreenX1()
    {
        CaptureScreen(1);
    }
    [MenuItem("ScreenCap/Capture Screenshot/x2")]
    public static void CaptureScreenX2()
    {
        CaptureScreen(2);
    }
    [MenuItem("ScreenCap/Capture Screenshot/x4")]
    public static void CaptureScreenX4()
    {
        CaptureScreen(4);
    }


    public static void CaptureScreen(int scale)
    {
        string date = System.DateTime.Now.ToString($"yyyy.MM.dd_h.mm_");
        ScreenCapture.CaptureScreenshot($"{date}{firstChars[Random.Range(0, firstChars.Length)]}{secondChars[Random.Range(0, secondChars.Length)]}{thirdChars[Random.Range(0, thirdChars.Length)]}.png", scale);
    }
}
