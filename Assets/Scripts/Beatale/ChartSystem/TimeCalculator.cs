using System.Collections.Generic;

namespace Beatale.ChartSystem
{
    public class TimeCalculator
    {
        public void GetBPM(Chart chart)
        {
            var bpmList = new Dictionary<double, double>();
            var bpm = chart.BPM;
            var length = chart.Length + chart.Offset;
            for (int index = 0; index < chart.BPMChanges.Count; index++)
            {
                chart.BPMChanges[index];
            }
        }

        public void GetTime(Chart chart)
        {
            var lastBPM = chart.BPMChanges[0].Position;


            for (int index = 0; index < )
        }

        public void GetFractionTime()
        {
            int position = 0, numerator = 0, denominator = 0;
            //var BPMChanges = Chart.BPMChanges.Where(x => x.BPM > 10);
        }

        public static void CalculateLongNoteTime(List<LongNote> longNotes)
        {
            for (int index = 0; index < longNotes.Count; index++)
            {
                //longNotes[index].LongNoteSamples
            }
            
        }

        public static float CaculatePositionTime(List<NotePosition> positions)
        {
            var bpm = 60.0f;
            var oneBeatTime = 60.0f / bpm;
            for (int index = 0; index < positions.Count; index++)
            {
                var position = positions[index];
                float time = position.Bar * oneBeatTime + (position.Numerator / position.Denominator);
            }
            return 0;
        }

        public static float CaculateNotesTime(List<Note> notes)
        {
            var bpm = 60.0f;
            var oneBeatTime = 60.0f / bpm;
            for (int index = 0; index < notes.Count; index++)
            {
                var position = notes[index].Position;
                float time = position.Bar * oneBeatTime + (position.Numerator / position.Denominator);
            }
            return 0;
        }

        public static float CalculateFloatTime(NotePosition position, double bpm)
        {
            return CalculateFloatTime(position.Bar, position.Numerator, position.Denominator, bpm);
        }

        public static float CalculateFloatTime(int position, int numerator, int denominator, double bpm)
        {
            var oneBeatTime = 60.0f / bpm;
            float time = position * oneBeatTime + (numerator / denominator);
            return time;
0        }

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
