using System;

namespace CrazyChipmunk
{
    public class RangedVal<T> where T : IComparable<T>
    {
        public T Min;
        public T Max;

        public RangedVal(T min, T max)
        {
            if (min.CompareTo(max) > 0)
            {
                //throw ArgumentOutOfRangeException("min must be smaller than max");
            }
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return "(" + Min + ", " + Max + ")";
        }

    }

    [Serializable]
    public class RangedFloat
    {
        public float Min;
        public float Max;
    }
    public class RangedInt : RangedVal<int>
    {
        public RangedInt(int min, int max) : base(min, max) { }
    }

    //[Serializable]
    //public class RangedFloat : RangedVal<float>
    //{
    //    public RangedFloat(float min, float max) : base(min, max) { }
    //}
}

