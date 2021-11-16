using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Willow.Library;
public class Sous : MonoBehaviour {

    public enum Mode
    {
        Fill,
        Fit,
        Center
    }

    public Mode mode = Mode.Fill;
    public int width = 320;
    public int height = 240;


    RenderTexture lowResTexture;
    Texture2D filler;
    GUIStyle fillerStyle;

	void Start () 
    {
        SetResolution(width, height);    

        GetComponent<Camera>().allowMSAA = false;

        filler = new Texture2D(1, 1);
        filler.SetPixel(0, 0, Color.black);
        filler.Apply();

        fillerStyle = new GUIStyle();
        fillerStyle.normal.background = filler;
    }
    
    public void SetResolution(int x, int y)
    {
        lowResTexture = new RenderTexture(x, y, 24, RenderTextureFormat.Default);
        lowResTexture.antiAliasing = 1;
        lowResTexture.filterMode = FilterMode.Point;

        GetComponent<Camera>().targetTexture = lowResTexture;
    }

    void OnGUI() {
        if (lowResTexture)
        {
            GUI.depth = 1001;
            GUI.Box(new Rect(0,0,Screen.width,Screen.height), GUIContent.none, fillerStyle);
            GUI.depth = 1000;
            switch (mode)
            {
                case Mode.Fill:
                    GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), lowResTexture,ScaleMode.StretchToFill, false);
                    GUI.EndGroup();
                    break;
                case Mode.Fit:
                    {
                        float widthScale = (float)Screen.width / (float)lowResTexture.width;
                        float scaledHeight = widthScale * lowResTexture.height;
                        if (scaledHeight > Screen.height)
                        {
                            float heightScale = (float)Screen.height / (float)lowResTexture.height;
                            int halfWidth = Screen.width / 2;
                            int scaledTexWidth = Mathf.FloorToInt(lowResTexture.width * heightScale);
                            int texHalfWidth = scaledTexWidth / 2;

                            GUI.BeginGroup(new Rect(halfWidth - texHalfWidth, 0, halfWidth - texHalfWidth + scaledTexWidth, Screen.height));
                            GUI.DrawTexture(new Rect(0, 0, scaledTexWidth, Screen.height), lowResTexture);
                            GUI.EndGroup();
                        } else
                        {
                            int halfHeight = Screen.height / 2;
                            int scaledTexHeight = Mathf.FloorToInt(lowResTexture.height * widthScale);
                            int texHalfHeight = scaledTexHeight / 2;

                            GUI.BeginGroup(new Rect(0, halfHeight - texHalfHeight, Screen.width, halfHeight - texHalfHeight + scaledTexHeight));
                            GUI.DrawTexture(new Rect(0, 0, Screen.width, scaledTexHeight), lowResTexture);
                            GUI.EndGroup();
                        }

                    }
                    break;
                case Mode.Center:
                    {
                        int halfWidth = Screen.width / 2;
                        int halfHeight = Screen.height / 2;
                        int texHalfWidth = lowResTexture.width / 2;
                        int texHalfHeight = lowResTexture.height / 2;

                        GUI.BeginGroup(new Rect(halfWidth - texHalfWidth, halfHeight - texHalfHeight, halfWidth - texHalfWidth + lowResTexture.width, halfHeight - texHalfHeight + lowResTexture.width));
                        GUI.DrawTexture(new Rect(0, 0, lowResTexture.width, lowResTexture.height), lowResTexture);
                        GUI.EndGroup();
                    }
                    break;
            }
        }
    }

    
}