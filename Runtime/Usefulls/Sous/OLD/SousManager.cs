using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Willow
{
    public class SousManager : MonoBehaviour
    {
        private static SousManager _singleton;
        public static SousManager singleton
        {
            get
            {
                if (!_singleton)
                {
                    GameObject newManager = new GameObject("Sous Manager", typeof(SousManager));
                    DontDestroyOnLoad(newManager);
                    _singleton = newManager.GetComponent<SousManager>();
                }

                return _singleton;
            }
        }

        public static System.Action OnDrawGui;

        public List<Sous> activeSous;

        Texture2D filler;
        GUIStyle fillerStyle;

        
        private void Awake()
        {
            activeSous = new List<Sous>();
        }

        private void Start()
        {
            filler = new Texture2D(1, 1);
            filler.SetPixel(0, 0, Color.black);
            filler.Apply();

            fillerStyle = new GUIStyle();
            fillerStyle.normal.background = filler;
        }


        public void AddSousToActive(Sous sous)
        {
            activeSous.Add(sous);
            activeSous.Sort((a, b) => { return a.depth.CompareTo(b.depth); });
        }

        public void RemoveSousFromActive(Sous sous)
        {
            activeSous.Remove(sous);
        }


        void OnGUI()
        {
            UnityEngine.GUI.depth = 1001;
            UnityEngine.GUI.Box(new Rect(0,0,Screen.width,Screen.height), GUIContent.none, fillerStyle);
            UnityEngine.GUI.depth = 1000;
            
            foreach (Sous sous in activeSous)
            {
                RenderTexture lowResTexture = sous.GetTex();

                if (lowResTexture)
                {
                    switch (sous.mode)
                    {
                        case Sous.Mode.Fill:
                            UnityEngine.GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
                            UnityEngine.GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), lowResTexture, ScaleMode.StretchToFill, sous.alphaBlend);
                            UnityEngine.GUI.EndGroup();
                            break;
                        case Sous.Mode.Fit:
                            {
                                float widthScale = (float)Screen.width / (float)lowResTexture.width;
                                float scaledHeight = widthScale * lowResTexture.height;
                                if (scaledHeight > Screen.height)
                                {
                                    float heightScale = (float)Screen.height / (float)lowResTexture.height;
                                    int halfWidth = Screen.width / 2;
                                    int scaledTexWidth = Mathf.FloorToInt(lowResTexture.width * heightScale);
                                    int texHalfWidth = scaledTexWidth / 2;

                                    UnityEngine.GUI.BeginGroup(new Rect(halfWidth - texHalfWidth, 0, halfWidth - texHalfWidth + scaledTexWidth, Screen.height));
                                    UnityEngine.GUI.DrawTexture(new Rect(0, 0, scaledTexWidth, Screen.height), lowResTexture, ScaleMode.StretchToFill, sous.alphaBlend);
                                    UnityEngine.GUI.EndGroup();
                                }
                                else
                                {
                                    int halfHeight = Screen.height / 2;
                                    int scaledTexHeight = Mathf.FloorToInt(lowResTexture.height * widthScale);
                                    int texHalfHeight = scaledTexHeight / 2;

                                    UnityEngine.GUI.BeginGroup(new Rect(0, halfHeight - texHalfHeight, Screen.width, halfHeight - texHalfHeight + scaledTexHeight));
                                    UnityEngine.GUI.DrawTexture(new Rect(0, 0, Screen.width, scaledTexHeight), lowResTexture, ScaleMode.StretchToFill, sous.alphaBlend);
                                    UnityEngine.GUI.EndGroup();
                                }
                            }
                            break;
                        case Sous.Mode.Center:
                            {
                                int halfWidth = Screen.width / 2;
                                int halfHeight = Screen.height / 2;
                                int texHalfWidth = lowResTexture.width / 2;
                                int texHalfHeight = lowResTexture.height / 2;

                                UnityEngine.GUI.BeginGroup(new Rect(halfWidth - texHalfWidth, halfHeight - texHalfHeight, halfWidth - texHalfWidth + lowResTexture.width, halfHeight - texHalfHeight + lowResTexture.width));
                                UnityEngine.GUI.DrawTexture(new Rect(0, 0, lowResTexture.width, lowResTexture.height), lowResTexture);
                                UnityEngine.GUI.EndGroup();
                            }
                            break;
                    }
                }
            }

            OnDrawGui.Invoke();
        }
    }
}