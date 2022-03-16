namespace Beatale.ChartSystem
{
    public struct LongNoteSample
    {
        public float Degree;
        public float Width;
        public float Time;

        public LongNoteSample(float degree, float width, float time)
        {
            Degree = degree;
            Width = width;
            Time = time;
        }

        public static LongNoteSample Lerp(LongNoteSample sample1, LongNoteSample sample2, float t)
        {
            return new LongNoteSample(
                sample1.Degree + t * (sample2.Degree - sample1.Degree),
                sample1.Width + t * (sample2.Width - sample1.Width),
                sample1.Time + t * (sample2.Time - sample1.Time)
                );
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var routeSample = (LongNoteSample)obj;
            return Degree.Equals(routeSample.Degree) &&
                Width.Equals(routeSample.Width) &&
                Time.Equals(routeSample.Time);
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
