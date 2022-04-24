using UnityEngine;
using System.IO;
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

        #endregion

        #region Quaternions
        static readonly Quaternion FORUP_SWAPROT = new Quaternion(0f,0.7f,0.7f,0f);
        public static void SwapForwardAndUp(this Quaternion rotation)
        {
            rotation = rotation * FORUP_SWAPROT;
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