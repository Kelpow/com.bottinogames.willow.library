using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Willow
{
    public static class Utilities
    {
        public static IEnumerator TransitionCoroutine(float length, Action<float> action)
        {
            float time = length;
            while (time > 0)
            {
                float t = (length - time) / length;
                action.Invoke(t);
                yield return null;
                time -= Time.deltaTime;
            }
            action.Invoke(1f);
        }
    }
}