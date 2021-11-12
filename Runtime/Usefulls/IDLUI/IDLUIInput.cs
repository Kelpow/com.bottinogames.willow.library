using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Willow.IDLUI
{
    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public interface Input
    {
        Direction Digital_GetDirInput();
        bool Digital_select { get; }
        


        Vector3 Analogue_screenPosition { get; }
        Vector3 Analogue_screenDelta { get; }    
        bool Analogue_select { get; }
        
    }
}