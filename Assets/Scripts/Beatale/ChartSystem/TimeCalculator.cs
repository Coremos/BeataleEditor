using System.Collections.Generic;

namespace Beatale.ChartSystem
{
    public class TimeCalculator
    {
        public void GetFractionTime()
        {
            int position = 0, numerator = 0, denominator = 0;
            //var BPMChanges = Chart.BPMChanges.Where(x => x.BPM > 10);
        }

        public static void CalculateLongNoteTime(IEnumerable<LongNote> longNotes)
        {
            foreach (var note in longNotes)
            {
                
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

        public static float CalculateFloatTime(int position, int numerator, int denominator, float bpm)
        {
            var oneBeatTime = 60.0f / bpm;
            float time = position * oneBeatTime + (numerator / denominator);
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
