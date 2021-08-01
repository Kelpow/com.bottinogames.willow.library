using UnityEngine;

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
        public static bool MouseOverlapsBounds(this Camera camera, Bounds bounds, Transform parent)
        {
            Vector3 position = parent ? parent.position : Vector3.zero;
            Quaternion rotation = parent ? parent.rotation : Quaternion.identity;

            Ray mouseray = camera.ScreenPointToRay(Input.mousePosition);
            Vector3 relativePos = mouseray.origin - position;
            Ray rotatedRay = new Ray(
                Quaternion.Inverse(rotation) * relativePos,
                Quaternion.Inverse(rotation) * mouseray.direction);
            Debug.Log(parent.lossyScale);
            return bounds.IntersectRay(rotatedRay);
        }
        #endregion
    }
}