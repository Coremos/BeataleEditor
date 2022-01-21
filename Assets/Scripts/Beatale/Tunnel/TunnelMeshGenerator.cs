using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Beatale
{
    namespace TunnelSystem
    {
        [RequireComponent(typeof(MeshFilter))]
        [RequireComponent(typeof(MeshRenderer))]
        public class TunnelMeshGenerator : MonoBehaviour
        {
            private static readonly int MIN_RADIAL_SEGMENTS = 3;
            private static readonly int MIN_HEIGHT_SEGMENTS = 1;
            private static readonly int DEFAULT_RADIAL_SEGMENTS = 10;
            private static readonly int DEFAULT_HEIGHT_SEGMENTS = 2;
            private static readonly float DEFAULT_RADIUS = 1;
            private static readonly float DEFAULT_HEIGHT = 5;


            public int RadialSegments = DEFAULT_RADIAL_SEGMENTS;
            public int HeightSegments = DEFAULT_HEIGHT_SEGMENTS;
            public float Radius = DEFAULT_RADIUS;
            public float Height = DEFAULT_HEIGHT;

            private Mesh tunnelMesh;
            private Mesh mesh;
            private MeshFilter meshFilter;

            private void Awake()
            {
            }

            public void CreateTunnelMesh()
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

                for (int row = 0; row <= HeightSegments; row++)
                {
                    for (int column = 0; column < RadialSegments; column++)
                    {
                        float angle = angleStep * column;

                        vertices.Add(new Vector3(row * heightStep, Radius * Mathf.Cos(angle), Radius * Mathf.Sin(angle)));
                        uvs.Add(new Vector2(1 - column * uvWidthStep, row * uvHeightStep));

                        if (row == 0 || column >= RadialSegments) continue;

                        //triangles.Add();
                    }
                }

                tunnelMesh.vertices = vertices.ToArray();
                tunnelMesh.triangles = triangles.ToArray();
                tunnelMesh.uv = uvs.ToArray();
            }

            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {

            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(TunnelMeshGenerator))]
        public class TunnelMeshGeneratorEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                TunnelMeshGenerator obj;
                obj = target as TunnelMeshGenerator;
                if (obj == null)
                {
                    return;
                }

                DrawDefaultInspector();
                if (GUI.changed)
                {
                    obj.CreateTunnelMesh();
                }
            }
        }
#endif
    }
}
