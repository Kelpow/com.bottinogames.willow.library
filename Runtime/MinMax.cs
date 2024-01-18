namespace Willow.Library
{
    public struct MinMax
    {
        public float min;
        public float max;

        public MinMax(float Min = float.MaxValue, float Max = float.MinValue)
        {
            min = Min;
            max = Max;
        }

        public void Encapsulate(float value)
        {
            min = min < value ? min : value;
            max = max > value ? max : value;
        }

        public float RemapValue(float value) => value.Remap01(min, max);
    }
}