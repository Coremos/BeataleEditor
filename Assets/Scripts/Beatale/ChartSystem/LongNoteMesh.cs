using UnityEngine;

namespace Beatale.ChartSystem
{
    public struct AngleVertex
    {
        public float Degree;
        public double Interval;

        public AngleVertex(float degree, double interval)
        {
            Degree = degree;
            Interval = interval;
        }
    }

    public class LongNoteMesh
    {
        public AngleVertex[] AngleVertices;
        public Vector3[] Vertices;
        public int[] Triangles;
        public Vector2[] UVS;
    }
}