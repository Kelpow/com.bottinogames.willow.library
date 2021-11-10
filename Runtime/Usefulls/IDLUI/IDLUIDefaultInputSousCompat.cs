using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Willow.Library;

namespace Willow.IDLUI
{
    [RequireComponent(typeof(Sous))]
    public class IDLUIDefaultInputSousCompat : MonoBehaviour,Input
    {
#if IDLUI_DEFAULT_CUSTOM_INPUT
        const string INPT_HORIZ = "IDLUI_Horizontal";
        const string INPT_VERT = "IDLUI_Vertical";

        const string INPT_SELECT = "IDLUI_Select";
#else
        const string INPT_HORIZ = "Horizontal";
        const string INPT_VERT = "Vertical";

        const string INPT_SELECT = "Submit";
#endif

        const float INPT_ANLG_DEADZN = 0.75f;


        private Vector2 analogueToDigitalIn { get => new Vector2(UnityEngine.Input.GetAxisRaw(INPT_HORIZ), UnityEngine.Input.GetAxisRaw(INPT_VERT)); }

        const float REPEAT_DELAY = 0.5f;
        const float REPEAT_TIME = 0.3f;

        private bool repeating;
        private float lastInputTime;
        private Input.Direction lastDirection;

        public Input.Direction Digital_GetDirInput()
        {
            Input.Direction unfilteredDirection = Input.Direction.None;

            float horiz = analogueToDigitalIn.x;
            float verti = analogueToDigitalIn.y;

            if (verti > INPT_ANLG_DEADZN)
                unfilteredDirection = Input.Direction.Up;
            else if (verti < -INPT_ANLG_DEADZN)
                unfilteredDirection = Input.Direction.Down;
            else if (horiz > INPT_ANLG_DEADZN)
                unfilteredDirection = Input.Direction.Right;
            else if (horiz < -INPT_ANLG_DEADZN)
                unfilteredDirection = Input.Direction.Left;



            if(unfilteredDirection == Input.Direction.None)
            {
                repeating = false;
                lastDirection = Input.Direction.None;
                lastInputTime = 0f;
                return Input.Direction.None;
            }

            if(lastDirection != unfilteredDirection)
            {
                repeating = false;
                lastDirection = unfilteredDirection;
                lastInputTime = Time.realtimeSinceStartup;
                return unfilteredDirection;
            }
            else
            {
                if(Time.realtimeSinceStartup > lastInputTime + (repeating ? REPEAT_TIME : REPEAT_DELAY))
                {
                    repeating = true;
                    lastInputTime = Time.realtimeSinceStartup;
                    return unfilteredDirection;
                }
                else
                {
                    return Input.Direction.None;
                }
            }
        }
    
        public bool Digital_select { get => UnityEngine.Input.GetButtonDown(INPT_SELECT); }

        Sous sous;
        private void Awake() { sous = GetComponent<Sous>(); }

        public Vector3 Analogue_screenPosition { 
            get
            {
                if (!sous)
                    return UnityEngine.Input.mousePosition;

                switch (sous.mode)
                {
                    case Sous.Mode.Fill:
                        return new Vector3(
                            (UnityEngine.Input.mousePosition.x.Remap01(0f, Screen.width).Clamp01() * sous.width),
                            (UnityEngine.Input.mousePosition.y.Remap01(0f, Screen.height).Clamp01() * sous.height)
                            );

                    case Sous.Mode.Fit:
                        float widthScale = (float)Screen.width / (float)sous.width;
                        float scaledHeight = widthScale * sous.height;
                        if (scaledHeight > Screen.height)
                        {
                            float heightScale = (float)Screen.height / (float)sous.height;
                            int halfWidth = Screen.width / 2;
                            int scaledTexWidth = Mathf.FloorToInt(sous.width * heightScale);
                            int texHalfWidth = scaledTexWidth / 2;

                            return new Vector3(
                                ((UnityEngine.Input.mousePosition.x - (halfWidth - texHalfWidth)).Remap01(0f, scaledTexWidth).Clamp01() * sous.width),
                                (UnityEngine.Input.mousePosition.y.Remap01(0f, Screen.height).Clamp01() * sous.height)
                                );
                        }
                        else
                        {
                            int halfHeight = Screen.height / 2;
                            int scaledTexHeight = Mathf.FloorToInt(sous.height * widthScale);
                            int texHalfHeight = scaledTexHeight / 2;

                            return new Vector3(
                                (UnityEngine.Input.mousePosition.x.Remap01(0f, Screen.width).Clamp01() * sous.width),
                                ((UnityEngine.Input.mousePosition.y - (halfHeight - texHalfHeight)).Remap01(0f, scaledTexHeight).Clamp01() * sous.height)
                                );
                        }

                    case Sous.Mode.Center:
                        {
                            int halfWidth = Screen.width / 2;
                            int halfHeight = Screen.height / 2;
                            int texHalfWidth = sous.width / 2;
                            int texHalfHeight = sous.height / 2;

                            return new Vector3(
                                ((UnityEngine.Input.mousePosition.x - (halfWidth - texHalfWidth)).Remap01(0f, sous.width).Clamp01() * sous.width),
                                ((UnityEngine.Input.mousePosition.y - (halfHeight - texHalfHeight)).Remap01(0f, sous.height).Clamp01() * sous.height)

                                );

                        }
                }
                return UnityEngine.Input.mousePosition;

            }
        }
        public Vector3 Analogue_screenDelta { get => new Vector3(UnityEngine.Input.GetAxisRaw("Mouse X"), UnityEngine.Input.GetAxisRaw("Mouse Y")); }


        public bool Analogue_select { get => UnityEngine.Input.GetMouseButtonUp(0); }
        
    }
}