using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Willow
{
    public static class Maths
    {
        /// <summary>
        /// a frame-independant alternative to Lerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="dt">The delta-time. Ususally Time.deltaTime</param>
        /// <returns></returns>
        public static Vector3 Damp(Vector3 from, Vector3 to, float lambda, float dt)
        {
            return Vector3.Lerp(from, to, 1 - Mathf.Exp(-lambda * dt));
        }

        /// <summary>
        /// a frame-independant alternative to Lerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static Vector3 Damp(Vector3 from, Vector3 to, float lambda, bool isRealtime = false)
        {
            float dt = isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
            return Vector3.Lerp(from, to, 1 - Mathf.Exp(-lambda * dt));
        }

        /// <summary>
        /// a frame-independant alternative to Lerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static float Damp(float from, float to, float lambda, bool isRealtime = false)
        {
            float dt = isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
            return Mathf.Lerp(from, to, 1 - Mathf.Exp(-lambda * dt));
        }

        /// <summary>
        /// a frame-independant alternative to Slerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="dt">The delta-time. Ususally Time.deltaTime</param>
        /// <returns></returns>
        public static Vector3 SDamp(Vector3 from, Vector3 to, float lambda, float dt)
        {
            return Vector3.Slerp(from, to, 1 - Mathf.Exp(-lambda * dt));
        }
        /// <summary>
        /// a frame-independant alternative to Slerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static Vector3 SDamp(Vector3 from, Vector3 to, float lambda, bool isRealtime = false)
        {
            float dt = isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
            return Vector3.Slerp(from, to, 1 - Mathf.Exp(-lambda * dt));
        }

        /// <summary>
        /// a frame-independant alternative to Slerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="dt">The delta-time. Ususally Time.deltaTime</param>
        /// <returns></returns>
        public static Quaternion SDamp(Quaternion from, Quaternion to, float lambda, float dt)
        {
            return Quaternion.Slerp(from, to, 1 - Mathf.Exp(-lambda * dt));
        }
        /// <summary>
        /// a frame-independant alternative to Slerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static Quaternion SDamp(Quaternion from, Quaternion to, float lambda, bool isRealtime = false)
        {
            float dt = isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
            return Quaternion.Slerp(from, to, 1 - Mathf.Exp(-lambda * dt));
        }
    }
}