using Beatale.RouteSystem;
using Beatale.TunnelSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Beatale.TunnelSystem
{
    public class TunnelMeshBender : MonoBehaviour
    {
        public TunnelMeshGenerator TunnelMeshGenerator;
        public float SamplingDistance = 0.1f;
        public RouteSpline RouteSpline;
        public GameObject Tunnel;
        public float Speed;
        public Vector3 TestVector3;
        public TunnelMesh OriginalMesh;
        public Transform CameraTransform;
        public TunnelMesh BentMesh;

        public float Roll;
        public float Distance;

        private Mesh tunnelMesh;
        private Dictionary<float, RouteSample> routeSamples;
        private int currentIndex;
        private bool isMove;
        private bool isNeedUpdate;

        void Awake()
        {
            routeSamples = new Dictionary<float, RouteSample>();
            isMove = false;
            currentIndex = 0;
            Tunnel.transform.position = Vector3.zero;
            InitializeMesh();
        }

        private void InitializeMesh()
        {
            BentMesh = OriginalMesh = TunnelMesh.GenerateMesh(TunnelMeshGenerator.GenerateTunnelMesh());
            
            tunnelMesh = new Mesh();

            tunnelMesh.vertices = BentMesh.Vertices;
            tunnelMesh.triangles = BentMesh.Triangles;
            tunnelMesh.uv = BentMesh.UV;

            tunnelMesh.RecalculateNormals();
            tunnelMesh.RecalculateBounds();
            tunnelMesh.RecalculateTangents();

            Tunnel.GetComponent<MeshFilter>().sharedMesh = tunnelMesh;
        }

        private void BendMesh(Mesh mesh)
        {
            RouteSample routeSample;
            routeSamples.Clear();

            BentMesh.Vertices = new Vector3[OriginalMesh.Vertices.Length];
            BentMesh.Normals = new Vector3[OriginalMesh.Normals.Length];

            for (int index = 0; index < BentMesh.Vertices.Length; index++)
            {
                float distance = OriginalMesh.Vertices[index].z + Distance;
                if (!routeSamples.TryGetValue(distance, out routeSample))
                {
                    routeSample = RouteSpline.GetRouteSample(distance);
                    routeSamples[distance] = routeSample;
                }
                BentMesh.Vertices[index] = routeSample.GetBentPosition(OriginalMesh.Vertices[index]);
                BentMesh.Normals[index] = routeSample.GetBentNormal(OriginalMesh.Normals[index]);
            }

            mesh.vertices = BentMesh.Vertices;
            mesh.normals = BentMesh.Normals;

            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) isMove = !isMove;

            if (!isMove) return;
            isNeedUpdate = true;
        }

        void UpdateMesh()
        {
            Distance += Speed * Time.deltaTime;
            BendMesh(tunnelMesh);
            var sample = RouteSpline.GetRouteSample(Distance);
            CameraTransform.position = sample.Position;
            CameraTransform.rotation = sample.Rotation;
        }

        private void LateUpdate()
        {
            //MeshTest();

            if (isNeedUpdate)
            {
                isNeedUpdate = false;
                UpdateMesh();
            }
        }

        private void MeshTest()
        {
            var testVertices = new Vector3[OriginalMesh.Vertices.Length];
            var samples = new Vector3[OriginalMesh.Vertices.Length];
            for (int index = 0; index < testVertices.Length; index++)
            {
                var sample = RouteSpline.GetRouteSample(OriginalMesh.Vertices[index].z + Distance);
                samples[index] = sample.Position;
                testVertices[index] = sample.GetBentPosition(OriginalMesh.Vertices[index]);
            }

            for (int index = 1; index < testVertices.Length; index++)
            {
                Debug.DrawLine(testVertices[index - 1], testVertices[index], Color.cyan);
                Debug.DrawLine(samples[index - 1], samples[index], Color.red);
            }
        }
    }
}