using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Willow.Library
{
    public static class Maths
    {
        #region ====Damp====
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
        /// <param name="dt">The delta-time. Ususally Time.deltaTime</param>
        /// <returns></returns>
        public static Vector3 DampUnclamped(Vector3 from, Vector3 to, float lambda, float dt)
        {
            return Vector3.LerpUnclamped(from, to, 1 - Mathf.Exp(-lambda * dt));
        }

        /// <summary>
        /// a frame-independant alternative to Lerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static Vector3 DampUnclamped(Vector3 from, Vector3 to, float lambda, bool isRealtime = false)
        {
            float dt = isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
            return Vector3.LerpUnclamped(from, to, 1 - Mathf.Exp(-lambda * dt));
        }

        /// <summary>
        /// a frame-independant alternative to Lerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static float Damp(float from, float to, float lambda, float dt)
        {
            return Mathf.Lerp(from, to, 1 - Mathf.Exp(-lambda * dt));
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
        /// a frame-independant alternative to Lerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static float DampUnclamped(float from, float to, float lambda, float dt)
        {
            return Mathf.LerpUnclamped(from, to, 1 - Mathf.Exp(-lambda * dt));
        }

        /// <summary>
        /// a frame-independant alternative to Lerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static float DampUnclamped(float from, float to, float lambda, bool isRealtime = false)
        {
            float dt = isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
            return Mathf.LerpUnclamped(from, to, 1 - Mathf.Exp(-lambda * dt));
        }


        /// <summary>
        /// a frame-independant alternative to Lerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static float DampRadians(float from, float to, float lambda, float dt)
        {
            const float tau = Mathf.PI * 2f;

            from = (from + tau) % tau;
            to = (to + tau) % tau;

            to = Mathf.Abs(from - to) < Mathf.PI ? to :
                Mathf.Abs(from - to + tau) < Mathf.PI ? to + tau :
                from - to - tau;
            return (Mathf.Lerp(from, to, 1 - Mathf.Exp(-lambda * dt)) + tau) % tau;
        }

        /// <summary>
        /// a frame-independant alternative to Lerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static float DampRadians(float from, float to, float lambda, bool isRealtime = false)
        {
            float dt = isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
            return DampRadians(from, to, lambda, dt);
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
        public static Vector3 SDampUnclamped(Vector3 from, Vector3 to, float lambda, float dt)
        {
            return Vector3.SlerpUnclamped(from, to, 1 - Mathf.Exp(-lambda * dt));
        }
        /// <summary>
        /// a frame-independant alternative to Slerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static Vector3 SDampUnclamped(Vector3 from, Vector3 to, float lambda, bool isRealtime = false)
        {
            float dt = isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
            return Vector3.SlerpUnclamped(from, to, 1 - Mathf.Exp(-lambda * dt));
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
        
        /// <summary>
        /// a frame-independant alternative to Slerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="dt">The delta-time. Ususally Time.deltaTime</param>
        /// <returns></returns>
        public static Quaternion SDampUnclamped(Quaternion from, Quaternion to, float lambda, float dt)
        {
            return Quaternion.SlerpUnclamped(from, to, 1 - Mathf.Exp(-lambda * dt));
        }
        /// <summary>
        /// a frame-independant alternative to Slerp
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lambda">The strength of the dampening, between 0 and float.PositiveInfinity</param>
        /// <param name="isRealtime"> Whether the damping should be effected by Time.timeScale</param>
        /// <returns></returns>
        public static Quaternion SDampUnclamped(Quaternion from, Quaternion to, float lambda, bool isRealtime = false)
        {
            float dt = isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
            return Quaternion.SlerpUnclamped(from, to, 1 - Mathf.Exp(-lambda * dt));
        }

        #endregion
        #region ====Bezier====
        public static Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 p0 = Vector3.Lerp(a, b, t);
            Vector3 p1 = Vector3.Lerp(b, c, t);
            return Vector3.Lerp(p0, p1, t);
        }
        public static Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            Vector3 p0 = Bezier(a, b, c, t);
            Vector3 p1 = Bezier(b, c, d, t);
            return Vector3.Lerp(p0, p1, t);
        }
        #endregion

        #region ====Physics====
        public static float Drag(float velocity, float drag, float deltaTime)
        {
            return velocity * (1f - deltaTime * drag).Clamp01();
        }

        public static Vector2 Drag(Vector2 velocity, float drag, float deltaTime)
        {
            return velocity * (1f - deltaTime * drag).Clamp01();
        }

        public static Vector3 Drag(Vector3 velocity, float drag, float deltaTime)
        {
            return velocity * (1f - deltaTime * drag).Clamp01();
        }
        #endregion

        /// <summary> Returns the nearest point on line segment AB to point P. </summary>
        public static Vector3 NearestPointOnLine (Vector3 a, Vector3 b, Vector3 p)
        {
            float t = Vector3.Dot(b - a, p - a) / (b - a).sqrMagnitude;
            if (t <= 0)
                return a;
            else if (t >= 1)
                return b;
            else
                return Vector3.Lerp(a, b, t);
        }

        public static Vector3 Barycentric(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
        {
            Vector3 bary = new Vector3();

            Vector2 lb = b - a;
            Vector2 lc = c - a;
            Vector2 lp = p - a;
            float denominatorReciprocal = 1/(lb.x * lc.y - lc.x * lb.y);

            bary.y = (lp.x * lc.y - lc.x * lp.y) * denominatorReciprocal;
            bary.z = (lb.x * lp.y - lp.x * lb.y) * denominatorReciprocal;
            bary.x = 1f - bary.y - bary.z;

            return bary;
        }

        public static Vector3 Barycentric(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
        {
            Vector3 v0 = b - a, v1 = c - a, v2 = p - a;
            float d00 = Vector3.Dot(v0, v0);
            float d01 = Vector3.Dot(v0, v1);
            float d11 = Vector3.Dot(v1, v1);
            float d20 = Vector3.Dot(v2, v0);
            float d21 = Vector3.Dot(v2, v1);
            float denomReciprocal = 1/(d00 * d11 - d01 * d01);

            Vector3 bary = new Vector3();
            bary.y = (d11 * d20 - d01 * d21) * denomReciprocal;
            bary.z = (d00 * d21 - d01 * d20) * denomReciprocal;
            bary.x = 1.0f - bary.y - bary.z;
            return bary;
        }

    }
}