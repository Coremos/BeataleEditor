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

        public void ConnectVertices(List<List<Vector3>> verticesList)
        {
            var triangles = new List<int>();
            int vertexIndex = 0;
            for (int row = 0; row < verticesList.Count - 1; row++)
            {
                var list = verticesList[row];
                var nextList = verticesList[row + 1];
                var nextIndex = 0;
                for (int index = 0; index < list.Count; index++)
                {
                    if (nextIndex > 0 && nextIndex <= nextList.Count - 1)
                    {
                        triangles.Add(vertexIndex + index);
                        triangles.Add(vertexIndex + list.Count + nextIndex - 1);
                        triangles.Add(vertexIndex + list.Count + nextIndex);
                    }

                    if (index < list.Count - 1)
                    {
                        triangles.Add(vertexIndex + index);
                        triangles.Add(vertexIndex + list.Count + nextIndex);
                        triangles.Add(vertexIndex + index + 1);

                        if (nextIndex < nextList.Count - 1) nextIndex++;
                    }
                    else
                    {
                        while (nextIndex < nextList.Count)
                        {
                            triangles.Add(vertexIndex + index);
                            triangles.Add(vertexIndex + list.Count + nextIndex - 1);
                            triangles.Add(vertexIndex + list.Count + nextIndex);
                            nextIndex++;
                        }
                    }
                }
                vertexIndex += list.Count;
            }
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
                        vertices.Add(vector);
                    }
                }
            }
        }

        public Vector2[] CalculateUVs(List<List<Vector3>> verticesList)
        {
            var uvs = new List<Vector2>();
            var rowStep = 1 / (verticesList.Count - 1);
            for (int row = 0; row < verticesList.Count; row++)
            {
                var step = 1 / (verticesList[row].Count - 1);
                for (int column = 0; column < verticesList[row].Count; column++)
                {
                    var uv = new Vector2(column * step, rowStep * row);
                    uvs.Add(uv);
                }
            }
            return uvs.ToArray();
        }
    }
}
