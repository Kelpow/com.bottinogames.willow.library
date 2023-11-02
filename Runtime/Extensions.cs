using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Willow.Library
{
    public static class Extensions
    {
        #region Vectors
        
        /// <summary>
        /// Flatten the Vector along the Y axis.
        /// </summary>
        public static Vector3 Flatten(this Vector3 vector)
        {
            return new Vector3(vector.x, 0f, vector.z);
        }

        /// <summary>
        /// Flatten the Vector along the X axis.
        /// </summary>
        public static Vector3 FlattenX(this Vector3 vector)
        {
            return new Vector3(0f, vector.y, vector.z);
        }

        /// <summary>
        /// Flatten the Vector along the Z axis.
        /// </summary>
        public static Vector3 FlattenZ(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, 0f);
        }

        /// <summary>
        /// Flatten the Vector along the specified vector. Vector is assumed to be normalized
        /// </summary>
        /// <param name="axis">Vector to be flattened along. Must be normalized.</param>
        public static Vector3 Flatten(this Vector3 vector, Vector3 axis)
        {
            return vector - (axis * Vector3.Dot(vector, axis));
        }

        /// <summary>
        /// Optionally replace any components of a vector.
        /// </summary>
        public static Vector3 Replace(this Vector3 v, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(
                x ?? v.x,
                y ?? v.y,
                z ?? v.z
            );
        }

        /// <summary>
        /// Optionally replace any components of a vector.
        /// </summary>
        public static Vector2 Replace(this Vector2 v, float? x = null, float? y = null)
        {
            return new Vector2(
                x ?? v.x,
                y ?? v.y
            );
        }

        /// <summary>
        /// Optionally add to any components of a vector.
        /// </summary>
        public static Vector3 Add(this Vector3 v, float x = 0f, float y = 0f, float z = 0f)
        {
            return new Vector3(
                v.x + x,
                v.y + y,
                v.z + z
                );
        }
        
        /// <summary>
        /// Optionally add to any components of a vector.
        /// </summary>
        public static Vector2 Add(this Vector2 v, float x = 0f, float y = 0f)
        {
            return new Vector2(
                v.x + x,
                v.y + y
                );
        }



        #endregion
        #region Vector Arrays
        public static Vector3 NearestTo(this IEnumerable<Vector3> enumerable, Vector3 point)
        {
            float sqrdist = float.MaxValue;
            Vector3 nearest = Vector3.zero;
            foreach (Vector3 vector in enumerable)
            {
                float sd = (point - vector).sqrMagnitude;
                if(sd < sqrdist)
                {
                    sqrdist = sd;
                    nearest = vector;
                }
            }
            return nearest;
        }
        #endregion

        #region Quaternions
        static readonly Quaternion FORUP_SWAPROT = new Quaternion(0f,0.707f,0.707f,0f);
        public static Quaternion SwapForwardAndUp(this Quaternion rotation)
        {
            return rotation * FORUP_SWAPROT;
        }

        #endregion

        #region Transform
        public static void InverseLockLookAt(this Transform transform, Vector3 forward, Vector3 up)
        {
            transform.rotation = Quaternion.LookRotation(up, forward) * FORUP_SWAPROT;
        }
        #endregion

        #region Floats
        /// <summary>
        /// Returns the float clamped between 0 and 1.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Clamp01(this float value)
        {
            return Mathf.Clamp01(value);
        }

        /// <summary>
        /// Returns the float clamped between 1 and -1.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Clamp11(this float value)
        {
            return Mathf.Clamp(value, -1, 1);
        }

        /// <summary>
        /// Remaps a range of numbers to 0-1.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="zero">the value which will be remapped to 0</param>
        /// <param name="one">the value which will be remapped to 1</param>
        /// <returns>the remaped float</returns>
        public static float Remap01(this float value, float zero, float one)
        {
            return (value - zero) / (one - zero);
        }

        /// <summary>
        /// returns the absolute value of the float
        /// </summary>
        public static float Abs(this float value)
        {
            return Mathf.Abs(value);
        }

        /// <summary>
        /// returns the module of value % divisor, will always be positive
        /// </summary>
        public static float Mod(this float value, float divisor)
        {
            return ((value % divisor) + divisor) % divisor;
        }

        /// <summary>
        /// Use a float as a timer, 'cause you have no self respect
        /// </summary>
        /// <param name="delta">The delta time with which to decrement the timer</param>
        /// <returns>returns true if the timer has expired</returns>
        public static bool AsTimer(this ref float value, float delta)
        {
            value -= delta;
            return value <= 0f;
        }
        #endregion

        #region Ints

        /// <summary>
        /// returns the module of value % divisor, will always be positive
        /// </summary>
        public static int Mod(this int value, int divisor)
        {
            return ((value % divisor) + divisor) % divisor;
        }

        /// <summary>
        /// Use a int as a countdown, 'cause you've got low self esteem
        /// </summary>
        /// <param name="delta">The delta with which to decrement the counter</param>
        /// <returns>returns true if the countdown has ended</returns>
        public static bool AsTimer(this ref int value, int delta)
        {
            value -= delta;
            return value <= 0;
        }

        #endregion

        #region Audio
        public static bool SetLinearVolume(this UnityEngine.Audio.AudioMixer mixer, string name, float volume)
        {
            float dB;
            if (volume > 0)
                dB = 20.0f * Mathf.Log10(volume);
            else
                dB = -144.0f;
            
            return mixer.SetFloat(name, dB);
        }
        #endregion

        #region Misc
        public static bool MouseOverlapsBounds(this Camera camera, Bounds bounds, Transform parent = null)
        {
            return MouseOverlapsBounds(camera, bounds, out Vector3 throwAwayVector, parent);
        }
        public static bool MouseOverlapsBounds(this Camera camera, Bounds bounds, out Vector3 localIntersectionPoint, Transform parent = null)
        {
            return ScreenPositionOverlapsBounds(camera, bounds, out localIntersectionPoint, Input.mousePosition, parent);
        }
        public static bool ScreenPositionOverlapsBounds(this Camera camera, Bounds bounds, Vector3 screenPosition, Transform parent = null)
        {
            return ScreenPositionOverlapsBounds(camera, bounds, out Vector3 throwAwayVector, screenPosition, parent);
        }
        public static bool ScreenPositionOverlapsBounds(this Camera camera, Bounds bounds, out Vector3 localIntersectionPoint, Vector3 screenPosition, Transform parent = null)
        {
            Ray mouseray = camera.ScreenPointToRay(screenPosition);    

            Ray transformedRay = new Ray(
                parent.InverseTransformPoint(mouseray.origin),
                parent.InverseTransformVector(mouseray.direction));

            bool hit = bounds.IntersectRay(transformedRay, out float distance);

            localIntersectionPoint = transformedRay.GetPoint(distance);

            return hit;
        }


        public static bool Populate<T>(this MonoBehaviour mono, ref T component) where T : Component
        {
            return mono.TryGetComponent<T>(out component);
        }
        public static bool Populate<T>(this GameObject go, ref T component) where T : Component
        {
            return go.TryGetComponent<T>(out component);
        }




        public static string Colorize(this string value, Color color)
        {
            return $"<color={ColorUtility.ToHtmlStringRGBA(color)}>{value}</color>";
        }





        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static T ToObject<T>(this byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            T obj = (T)binForm.Deserialize(memStream);
            return obj;
        }


        #endregion

    }
}