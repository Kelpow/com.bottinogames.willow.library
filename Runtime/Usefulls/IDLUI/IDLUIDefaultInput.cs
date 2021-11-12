using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Willow.IDLUI
{
    public class DefaultInput : Input
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
        


        public Vector3 Analogue_screenPosition { get => UnityEngine.Input.mousePosition; }
        public Vector3 Analogue_screenDelta { get => new Vector3(UnityEngine.Input.GetAxisRaw("Mouse X"), UnityEngine.Input.GetAxisRaw("Mouse Y")); }


        public bool Analogue_select { get => UnityEngine.Input.GetMouseButtonUp(0); }
        
    }
}