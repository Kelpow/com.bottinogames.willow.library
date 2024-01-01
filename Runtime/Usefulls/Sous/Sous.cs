using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Willow.Library;

namespace Willow
{
    [DefaultExecutionOrder(250)]
    public class Sous : MonoBehaviour
    {
        public enum Mode
        {
            Fill,
            Fit,
            Center
        }

        public Mode mode = Mode.Fill;
        public int width = 320;
        public int height = 240;
        public bool alphaBlend = false;
        public int depth;

        RenderTexture lowResTexture;

        void Awake()
        {
            SetResolution(width, height);

            GetComponent<Camera>().allowMSAA = false;
        }

        private void OnEnable()
        {
            Willow.SousManager.singleton.AddSousToActive(this);
        }

        private void OnDisable()
        {
            Willow.SousManager.singleton.RemoveSousFromActive(this);
        }


        public void SetResolution(int x, int y)
        {
            width = x;
            height = y;

            lowResTexture = new RenderTexture(x, y, 24, RenderTextureFormat.Default);
            lowResTexture.antiAliasing = 1;
            lowResTexture.filterMode = FilterMode.Point;

            GetComponent<Camera>().targetTexture = lowResTexture;
        }


        public RenderTexture GetTex() => lowResTexture;
    }
}
