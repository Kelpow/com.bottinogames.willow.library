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
        private Direction lastDirection;

        public Direction Digital_GetDirInput()
        {
            Direction unfilteredDirection = Direction.None;

            float horiz = analogueToDigitalIn.x;
            float verti = analogueToDigitalIn.y;

            if (verti > INPT_ANLG_DEADZN)
                unfilteredDirection = Direction.Up;
            else if (verti < -INPT_ANLG_DEADZN)
                unfilteredDirection = Direction.Down;
            else if (horiz > INPT_ANLG_DEADZN)
                unfilteredDirection = Direction.Right;
            else if (horiz < -INPT_ANLG_DEADZN)
                unfilteredDirection = Direction.Left;



            if(unfilteredDirection == Direction.None)
            {
                repeating = false;
                lastDirection = Direction.None;
                lastInputTime = 0f;
                return Direction.None;
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
                    return Direction.None;
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
            }
        }
        public Vector3 Analogue_screenDelta { get => new Vector3(UnityEngine.Input.GetAxisRaw("Mouse X"), UnityEngine.Input.GetAxisRaw("Mouse Y")); }


        public bool Analogue_select { get => UnityEngine.Input.GetMouseButtonUp(0); }
        
    }
}