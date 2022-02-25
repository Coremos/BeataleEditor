using UnityEngine;
using System.Collections.Generic;
using Beatale.Route.Curve;

namespace Beatale.TunnelSystem
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class TunnelMeshGenerator : MonoBehaviour
    {
        public int RadialSegments = DEFAULT_RADIAL_SEGMENTS;
        public int HeightSegments = DEFAULT_HEIGHT_SEGMENTS;
        public float Radius = DEFAULT_RADIUS;
        public float Height = DEFAULT_HEIGHT;
        public Vector3 Direction1;
        public Vector3 Direction2;

        private static readonly int MIN_RADIAL_SEGMENTS = 3;
        private static readonly int MIN_HEIGHT_SEGMENTS = 1;
        private static readonly int DEFAULT_RADIAL_SEGMENTS = 10;
        private static readonly int DEFAULT_HEIGHT_SEGMENTS = 2;
        private static readonly float DEFAULT_RADIUS = 1;
        private static readonly float DEFAULT_HEIGHT = 5;

        private Mesh tunnelMesh;
        private MeshFilter meshFilter;

        public TunnelMesh GetTunnelMesh()
        {
            return TunnelMesh.GenerateMesh(GenerateTunnelMesh());
        }

        public float[] GenerateRadiusTable(int resolution)
        {
            Vector3 position1, position2, direction1, direction2;
            position1 = position2 = direction1 = direction2 = Vector3.zero;
            position1.x = 1.0f;
            position2.y = 1.0f;
            direction1 = Direction1;
            direction2 = Direction2;
            //direction1.z = 0.5f;
            //direction2.z = -0.1f;
            var lut = CubicCurve.GenerateLengthTable(position1, direction1, direction2, position2, resolution);

            float lengthStep = 1.0f / (resolution - 1);
            float[] table = new float[resolution];
            table[0] = 1;
            for (int index = 1; index < resolution - 1; index++)
            {
                var t = CubicCurve.DistanceToTValue(lengthStep * index, lut);
                table[index] = CubicCurve.GetPoint(position1, direction1, direction2, position2, t).x;
            }
            table[resolution - 1] = 0;
            return table;
        }

        public Mesh GenerateTunnelMesh()
        {
            tunnelMesh = new Mesh();
            tunnelMesh.name = "TunnelMesh";
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = tunnelMesh;

            if (RadialSegments < MIN_RADIAL_SEGMENTS) RadialSegments = MIN_RADIAL_SEGMENTS;
            if (HeightSegments < MIN_HEIGHT_SEGMENTS) HeightSegments = MIN_HEIGHT_SEGMENTS;

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();

            float angleStep = 2 * Mathf.PI / RadialSegments;
            float heightStep = Height / HeightSegments;
            float uvWidthStep = 1.0f / RadialSegments;
            float uvHeightStep = 1.0f / HeightSegments;

            int columnVertexAmount = RadialSegments + 1;

            float[] radiusTable = GenerateRadiusTable(HeightSegments + 1);

            for (int row = 0; row <= HeightSegments; row++)
            {
                for (int column = 0; column <= RadialSegments; column++)
                {
                    float angle = angleStep * column;

                    vertices.Add(new Vector3(Radius * radiusTable[row] * Mathf.Sin(angle), Radius * radiusTable[row] * Mathf.Cos(angle), row * heightStep));
                    uvs.Add(new Vector2(1 - column * uvWidthStep, row * uvHeightStep));

                    if (row == 0 || column >= RadialSegments) continue;

                    triangles.Add(row * columnVertexAmount + column);
                    triangles.Add((row - 1) * columnVertexAmount + column);
                    triangles.Add(row * columnVertexAmount + column + 1);

                    triangles.Add((row - 1) * columnVertexAmount + column);
                    triangles.Add((row - 1) * columnVertexAmount + column + 1);
                    triangles.Add(row * columnVertexAmount + column + 1);
                }
            }

            tunnelMesh.vertices = vertices.ToArray();
            tunnelMesh.triangles = triangles.ToArray();
            tunnelMesh.uv = uvs.ToArray();

            tunnelMesh.RecalculateNormals();
            tunnelMesh.RecalculateBounds();
            tunnelMesh.RecalculateTangents();

            return tunnelMesh;
        }
    }
}