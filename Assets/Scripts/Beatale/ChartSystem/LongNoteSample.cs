namespace Beatale.ChartSystem
{
    public struct LongNoteSample
    {
        public float Degree;
        public float Width;

        public LongNoteSample(float degree, float width)
        {
            Degree = degree;
            Width = width;
        }

        public static LongNoteSample Lerp(LongNoteSample sample1, LongNoteSample sample2, float t)
        {
            return new LongNoteSample(
                sample1.Degree + t * (sample2.Degree - sample1.Degree),
                sample1.Width + t * (sample2.Width - sample1.Width)
                );
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var routeSample = (LongNoteSample)obj;
            return Degree.Equals(routeSample.Degree) &&
                Width.Equals(routeSample.Width);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(LongNoteSample sample1, LongNoteSample sample2)
        {
            return sample1.Equals(sample2);
        }

        public static bool operator !=(LongNoteSample sample1, LongNoteSample sample2)
        {
            return !sample1.Equals(sample2);
        }
    }
}
