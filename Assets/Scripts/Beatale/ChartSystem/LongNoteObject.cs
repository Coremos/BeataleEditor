using UnityEngine;

namespace Beatale.ChartSystem
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class LongNoteObject : MonoBehaviour
    {
        private MeshFilter meshFilter;
        private Mesh mesh;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        public void InitializeMesh(LongNoteMesh longNoteMesh)
        {
            mesh = new Mesh();
            mesh.vertices = longNoteMesh.Vertices;
            mesh.triangles = longNoteMesh.Triangles;
            mesh.uv = longNoteMesh.UVS;
            meshFilter.sharedMesh = mesh;
        }

        public void UpdateMesh(LongNoteMesh longNoteMesh)
        {
            mesh.vertices = longNoteMesh.Vertices;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
        }
    }
}
