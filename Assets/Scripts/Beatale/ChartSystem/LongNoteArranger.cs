using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Beatale.ChartSystem
{
    public class LongNoteArranger : MonoBehaviour
    {
        public Chart chart;

        public void PreLoad()
        {
            LongNoteSampler.LongNoteSampling(chart.LongNotes);

            TimeCalculator.CaculateNotesTime(chart.Notes);

            var s = chart.Notes.Select(note => note.Position);
        }

        public void LoadLongNotes()
        {
            
        }

        public void Project()
        {
            float angleStep = 360.0f / 10;
            for (int index = 0; index < chart.LongNotes.Count; index++)
            {
                var longNoteSamples = chart.LongNotes[index].LongNoteSamples;
                var vertices = new List<Vector3>();
                for (int sampleIndex = 0; sampleIndex < longNoteSamples.Count; sampleIndex++)
                {
                    var longNoteSample = longNoteSamples[sampleIndex];
                    var halfDegree = longNoteSample.Width * 0.5f;
                    float startAngle = longNoteSamples[sampleIndex].Degree - halfDegree;
                    float endAngle = longNoteSamples[sampleIndex].Degree + halfDegree;
                    for (var angle = startAngle; angle < endAngle; angle += angleStep)
                    {
                        var vector = new Vector3(angle, 0.0f);
                        //vertices = 
                    }
                }
            }
        }
    }
}
