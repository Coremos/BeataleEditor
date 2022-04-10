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

        public static void CalculateBPMTime2(Chart chart)
        {
            chart.BPMChanges[0].Position.Time = 0.0;
            for (int index = 1; index < chart.BPMChanges.Count; index++)
            {
                var fractionValue = 0.0;
                var position = chart.BPMChanges[index].Position - chart.BPMChanges[index - 1].Position;

                if (position.Denominator != 0) fractionValue = (position.Numerator) / (double)position.Denominator;

                chart.BPMChanges[index].Position.Time = chart.BPMChanges[index - 1].Position.Time +
                    //(60.0 / chart.BPMChanges[index - 1].BPM) *
                    (chart.BPMChanges[index - 1].BPM / 60.0) *
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
                    (60.0 / chart.BPMChanges[index - 1].BPM) *
                    (position.Bar + fractionValue);
            }
        }

        public static void CalculateBPMTime3(Chart chart)
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

        public static void GetTime(Chart chart)
        {
            var bpmChanges = chart.BPMChanges;
            var lastBPM = bpmChanges[0];

            var notes = chart.Notes;
            var longNotes = chart.LongNotes;
            var nextBPMIndex = 1;
            var oneBeatTime = 60.0 / lastBPM.BPM;

            for (int index = 0; index < notes.Count; index++)
            {
                while (nextBPMIndex < bpmChanges.Count)
                {
                    if (notes[index].Position.Bar >= bpmChanges[nextBPMIndex].Position.Bar)
                    {
                        if ((notes[index].Position.Bar > bpmChanges[nextBPMIndex].Position.Bar) ||
                            (notes[index].Position.Numerator * bpmChanges[nextBPMIndex].Position.Denominator >=
                            bpmChanges[nextBPMIndex].Position.Numerator * notes[index].Position.Denominator
                            ))
                        {
                            lastBPM = bpmChanges[nextBPMIndex++];
                            oneBeatTime = 60.0 / lastBPM.BPM;
                            continue;
                        }
                    }
                    break;
                }

                var fractionValue = 0.0;
                var barDifference = notes[index].Position.Bar - lastBPM.Position.Bar;
                var noteNumerator = notes[index].Position.Numerator;
                var bpmNumerator = lastBPM.Position.Numerator;
                var denominator = (lastBPM.Position.Denominator == 0) ? 1 : lastBPM.Position.Denominator;

                if (lastBPM.Position.Denominator != notes[index].Position.Denominator)
                {
                    noteNumerator *= (lastBPM.Position.Denominator == 0) ? 1 : lastBPM.Position.Denominator;
                    bpmNumerator *= (notes[index].Position.Denominator == 0) ? 1 : notes[index].Position.Denominator;
                    denominator *= (notes[index].Position.Denominator == 0) ? 1 : notes[index].Position.Denominator;
                }

                if (denominator != 0) fractionValue = (noteNumerator - bpmNumerator) / (double)denominator;

                notes[index].Position.Time = lastBPM.Position.Time +
                    oneBeatTime * (barDifference + fractionValue);
            }

            for (int index = 0; index < longNotes.Count; index++)
            {

            }
        }

        public static double CalculateFloatTime(NotePosition position, double bpm)
        {
            return CalculateFloatTime(position.Bar, position.Numerator, position.Denominator, bpm);
        }

        public static double CalculateFloatTime(int position, int numerator, int denominator, double bpm)
        {
            var oneBeatTime = 60.0f / bpm;
            var time = position * oneBeatTime + (numerator / denominator);
            return time;
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
