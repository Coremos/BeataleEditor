using System.Collections.Generic;

namespace Beatale.ChartSystem
{
    public class TimeCalculator
    {
        public static void GetBPM(Chart chart)
        {
            var bpmList = new Dictionary<double, double>();
            var bpm = chart.BPM;
            var length = chart.Length + chart.Offset;
            for (int index = 0; index < chart.BPMChanges.Count; index++)
            {
                //chart.BPMChanges[index];
            }
        }

        delegate void CalculateExpression();

        public static void CalculateAdjustTime(Chart chart)
        {
            chart.BPMChanges[0].Position.Time = 0.0;
            for (int index = 1; index < chart.BPMChanges.Count; index++)
            {
                var fractionValue = 0.0;
                var position = chart.BPMChanges[index].Position - chart.BPMChanges[index - 1].Position;

                if (position.Denominator != 0) fractionValue = (position.Numerator) / (double)position.Denominator;

                chart.BPMChanges[index].Position.Time = chart.BPMChanges[index - 1].Position.Time +
                    60.0 / chart.BPMChanges[index - 1].BPM *
                    (position.Bar + fractionValue);
            }
        }

        public static void CalculateBPMTime(Chart chart)
        {
            chart.BPMChanges[0].Position.Time = 0.0;
            for (int index = 1; index < chart.BPMChanges.Count; index++)
            {
                var fractionValue = 0.0;
                var position = chart.BPMChanges[index].Position - chart.BPMChanges[index - 1].Position;

                if (position.Denominator != 0) fractionValue = (position.Numerator) / (double)position.Denominator;

                chart.BPMChanges[index].Position.Time = chart.BPMChanges[index - 1].Position.Time +
                    chart.BPMChanges[index - 1].BPM / 60.0 / 20.0 * 1 *
                    (position.Bar + fractionValue);
            }
        }

        public static void CalculateFractionTime(float time, float bpm, float division, out int position, out int numerator, out int denominator)
        {
            var oneBeatTime = 60.0f / bpm;
            var quntitizeTime = oneBeatTime / division;

            var index = time / oneBeatTime; // 몇 번째인지 계산됨
            position = (int)index;
            var difference = index - position;
            numerator = (int)(difference * oneBeatTime / quntitizeTime);
            denominator = (int)division;
        }
    }
}
