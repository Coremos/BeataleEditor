namespace Beatale.ChartSystem
{
    public class ChartCalculator
    {
        private static float speed = 1.0f;
        private static double constant = 1.0 / 60.0 / 20.0;

        public static void CalculateBPM(Chart chart)
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

        public static void CalculateChart(Chart chart)
        {
            var bpmChanges = chart.BPMChanges;
            var lastBPM = bpmChanges[0];

            var notes = chart.Notes;
            var longNotes = chart.LongNotes;
            var nextBPMIndex = 1;
            var oneBeatTime = 60.0 / lastBPM.BPM;
            var oneBeatInterval = lastBPM.BPM * constant * speed;

            for (int index = 0; index < notes.Count; index++)
            {
                while (nextBPMIndex < bpmChanges.Count)
                {
                    if (notes[index].Position > bpmChanges[nextBPMIndex].Position)
                    {
                        lastBPM = bpmChanges[nextBPMIndex++];
                        oneBeatTime = 60.0 / lastBPM.BPM;
                        oneBeatInterval = lastBPM.BPM * constant * speed;
                        continue;
                    }
                    break;
                }

                var fractionValue = 0.0;
                var position = notes[index].Position - lastBPM.Position;

                if (position.Denominator != 0) fractionValue = position.Numerator / (double)position.Denominator;

                notes[index].Position.Time = lastBPM.Position.Time +
                    oneBeatTime * (position.Bar + fractionValue);
                notes[index].Position.Interval = (float)(lastBPM.Position.Interval +
                    oneBeatInterval * (position.Bar + fractionValue));
            }

            lastBPM = bpmChanges[0];
            nextBPMIndex = 1;
            oneBeatTime = 60.0 / lastBPM.BPM;

            for (int longNoteIndex = 0; longNoteIndex < longNotes.Count; longNoteIndex++)
            {
                var vertices = longNotes[longNoteIndex].LongNoteVertices;
                for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
                {
                    while (nextBPMIndex < bpmChanges.Count)
                    {
                        if (vertices[vertexIndex].Position > bpmChanges[nextBPMIndex].Position)
                        {
                            lastBPM = bpmChanges[nextBPMIndex++];
                            oneBeatTime = 60.0 / lastBPM.BPM;
                            oneBeatInterval = lastBPM.BPM * constant * speed;
                            continue;
                        }
                        break;
                    }

                    var fractionValue = 0.0;
                    var position = vertices[vertexIndex].Position - lastBPM.Position;

                    if (position.Denominator != 0) fractionValue = position.Numerator / (double)position.Denominator;

                    vertices[vertexIndex].Position.Time = lastBPM.Position.Time +
                        oneBeatTime * (position.Bar + fractionValue);
                    vertices[vertexIndex].Position.Interval = (float)(lastBPM.Position.Time +
                        oneBeatInterval * (position.Bar + fractionValue));
                }
            }
        }

        
    }
}