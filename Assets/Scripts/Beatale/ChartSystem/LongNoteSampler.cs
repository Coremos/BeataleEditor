using Beatale.RouteSystem.Curve;
using System.Collections.Generic;
using UnityEngine;

namespace Beatale.ChartSystem
{
    public class LongNoteSampler
    {
        private const int DEFAULT_RESOLUTION = 10;

        public static void LongNoteSampling(List<LongNote> longNotes)
        {
            for (int index = 0; index < longNotes.Count; index++)
            {
                longNotes[index].LongNoteSamples = GetLongNoteSamples(longNotes[index]);
            };
        }

        public static List<LongNoteSample> GetLongNoteSamples(LongNote longNote, int resolution = DEFAULT_RESOLUTION)
        {
            var samples = new List<LongNoteSample>();
            for (int noteIndex = 0; noteIndex < longNote.LongNoteVertices.Count - 1; noteIndex++)
            {
                var vertex1 = longNote.LongNoteVertices[noteIndex];
                var vertex2 = longNote.LongNoteVertices[noteIndex + 1];

                var vector1 = new Vector3(vertex1.Degree, 0.0f, 0.0f);
                var vector2 = new Vector3(vertex2.Degree, 1.0f, 0.0f);

                var lengthTable = CubicCurve.GenerateLengthTable(vector1, vertex1.Direction2, vertex2.Direction1, vector2, resolution);
                var lengthStep = 1.0f / (resolution - 1);

                for (int index = 0; index < resolution - 1; index++)
                {
                    if (index == resolution - 2 && noteIndex != longNote.LongNoteVertices.Count - 2) break;
                    
                    var t = CubicCurve.DistanceToTValue(lengthStep * index, lengthTable);
                    
                    var degree = CubicCurve.GetPoint(vector1, vertex1.Direction2, vertex2.Direction1, vector2, t).x;
                    var width = vertex1.Width + t * (vertex2.Width - vertex1.Width);
                    var time = vertex1.Time + t * (vertex2.Time - vertex1.Time);
                    samples.Add(new LongNoteSample(degree, width, time));
                }
            }
            return samples;
        }
    }
}
