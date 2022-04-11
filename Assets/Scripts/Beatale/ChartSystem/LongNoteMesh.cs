using System.Collections.Generic;
using UnityEngine;

namespace Beatale.ChartSystem
{
    public struct AngleVertex
    {
        public float Degree;
        public double Time;

        public AngleVertex(float degree, double time)
        {
            Degree = degree;
            Time = time;
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