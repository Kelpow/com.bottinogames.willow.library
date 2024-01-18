namespace Willow.Library
{
    public struct MinMax
    {
        public float min;
        public float max;

        public MinMax Default => new MinMax(float.MaxValue, float.MinValue);

        public MinMax(float Min, float Max)
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

        public override string ToString()
        {
            return $"({min}->{max})";
        }
    }
}