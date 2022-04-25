namespace Beatale.ChartSystem
{
    public struct LongNoteSample
    {
        public float Degree;
        public float Width;
        public float Interval;
        public double Time;

        public LongNoteSample(float degree, float width, float interval, double time)
        {
            Degree = degree;
            Width = width;
            Interval = interval;
            Time = time;
        }

        public static LongNoteSample Lerp(LongNoteSample sample1, LongNoteSample sample2, float t)
        {
            return new LongNoteSample(
                sample1.Degree + t * (sample2.Degree - sample1.Degree),
                sample1.Width + t * (sample2.Width - sample1.Width),
                sample1.Interval + t * (sample2.Interval - sample1.Interval),
                sample1.Time + t * (sample2.Time - sample1.Time)
                );
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var routeSample = (LongNoteSample)obj;
            return Degree.Equals(routeSample.Degree) &&
                Width.Equals(routeSample.Width) &&
                Interval.Equals(routeSample.Interval) &&
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
