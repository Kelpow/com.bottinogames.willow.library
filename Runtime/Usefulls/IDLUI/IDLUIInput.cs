using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Willow.IDLUI
{
    public interface Input
    {
        public enum Direction
        {
            None,
            Up,
            Down,
            Left,
            Right
        }

        public Direction Digital_GetDirInput();
        public bool Digital_select { get; }
        


        public Vector3 Analogue_screenPosition { get; }
        public Vector3 Analogue_screenDelta { get; }
        public bool Analogue_select { get; }
        
    }
}