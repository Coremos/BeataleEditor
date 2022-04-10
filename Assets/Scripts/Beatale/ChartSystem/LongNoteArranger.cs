using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Beatale.ChartSystem
{
    public class LongNoteArranger : MonoBehaviour
    {
        public static void Project(Chart chart)
        {
            var angleStep = 360.0 / 10.0;
            for (int index = 0; index < chart.LongNotes.Count; index++)
            {
                var noteMesh = new LongNoteMesh();
                var verticesList = new List<List<AngleVertex>>();

                var longNoteSamples = chart.LongNotes[index].LongNoteSamples;
                for (int sampleIndex = 0; sampleIndex < longNoteSamples.Count; sampleIndex++)
                {
                    var vertices = new List<AngleVertex>();
                    var halfDegree = longNoteSamples[sampleIndex].Width * 0.5f;
                    float startAngle = longNoteSamples[sampleIndex].Degree - halfDegree;
                    float endAngle = longNoteSamples[sampleIndex].Degree + halfDegree;
                    vertices.Add(new AngleVertex((float)startAngle, longNoteSamples[sampleIndex].Time));

                    for (var angle = -360.0; angle < endAngle; angle += angleStep)
                    {
                        if (angle > startAngle)
                        {
                            var vertex = new AngleVertex((float)angle, longNoteSamples[sampleIndex].Time);

                            vertices.Add(vertex);
                        }
                    }
                    vertices.Add(new AngleVertex((float)endAngle, longNoteSamples[sampleIndex].Time));
                    verticesList.Add(vertices);
                }
                
                noteMesh.AngleVertices = GenerateAngleVertices(verticesList);
                noteMesh.Triangles = GenerateTriangles(verticesList);
                noteMesh.UVS = GenerateUVs(verticesList);

                chart.LongNotes[index].LongNoteMesh = noteMesh;
            }
        }

        public static AngleVertex[] GenerateAngleVertices(List<List<AngleVertex>> verticesList)
        {
            var angleVertices = new List<AngleVertex>();
            
            for (int index = 0; index < verticesList.Count; index++)
            {
                for (int column = 0; column < verticesList[index].Count; column++)
                {
                    angleVertices.Add(verticesList[index][column]);
                }
            }
            
            return angleVertices.ToArray();
        }

        public static int[] GenerateTriangles(List<List<AngleVertex>> verticesList)
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
                        while (++nextIndex < nextList.Count)
                        {
                            triangles.Add(vertexIndex + index);
                            triangles.Add(vertexIndex + list.Count + nextIndex - 1);
                            triangles.Add(vertexIndex + list.Count + nextIndex);
                        }
                    }
                }
                vertexIndex += list.Count;
            }
            return triangles.ToArray();
        }


        public static Vector2[] GenerateUVs(List<List<AngleVertex>> verticesList)
        {
            var uvs = new List<Vector2>();

            var rowStart = verticesList[0][0].Time;
            var rowStep = 1 / (verticesList[verticesList.Count - 1][0].Time - rowStart);

            for (int row = 0; row < verticesList.Count; row++)
            {
                var columnStart = verticesList[row][0].Degree;
                var columnStep = 1 / verticesList[row][verticesList[row].Count - 1].Degree - columnStart;
                
                for (int column = 0; column < verticesList[row].Count; column++)
                {
                    var uv = new Vector2(verticesList[row][column].Degree * columnStep, (float)((verticesList[row][0].Time - rowStart) * rowStep));
                    uvs.Add(uv);
                }
            }

            return uvs.ToArray();
        }

        public static void DebugAngleVerticesList(List<List<AngleVertex>> verticesList)
        {
            for (int index = 0; index < verticesList.Count; index++)
            {
                for (int column = 0; column < verticesList[index].Count; column++)
                {
                    Debug.Log(index + "row / " + column + "col : " + verticesList[index][column].Degree);
                }
            }
        }

        public static void DebugAngleVerticesListCount(List<List<AngleVertex>> verticesList)
        {
            var value = "";
            var count = 0;
            for (int index = 0; index < verticesList.Count; index++)
            {
                for (int column = 0; column < verticesList[index].Count; column++)
                {
                    value += count++ + ",";
                }
                value += "/";
            }
            Debug.Log(value);
        }

        public static void DebugLongNoteMeshes(Chart chart)
        {
            for (int index = 0; index < chart.LongNotes.Count; index++)
            {
                var value = "MeshNumber" + index + ":";
                var count = 0;
                var triangles = chart.LongNotes[index].LongNoteMesh.Triangles;
                for (int vertexIndex = 0; vertexIndex < triangles.Length; vertexIndex++)
                {
                    value += triangles[vertexIndex];
                    if (count++ < 2) value += ",";
                    else
                    {
                        value += "/";
                        count = 0;
                    }
                }
                Debug.Log(value);
            }
        }
    }
}
