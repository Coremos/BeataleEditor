using UnityEngine;

namespace Beatale.Tunnel
{
    public struct TunnelMesh
    {
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public int[] Triangles;
        public Vector2[] UV;
        public float MinPoint;
        public float Length;

        public static TunnelMesh GenerateMesh(Mesh mesh)
        {
            var newMesh = new TunnelMesh();
            newMesh.Vertices = mesh.vertices;
            newMesh.Normals = mesh.normals;
            newMesh.Triangles = mesh.triangles;
            newMesh.UV = mesh.uv;
            newMesh.MinPoint = float.MaxValue;

            float maxPoint = 0.0f;
            for (int index = 0; index < newMesh.Vertices.Length; index++)
            {
                if (newMesh.Vertices[index].z < newMesh.MinPoint)
                {
                    newMesh.MinPoint = newMesh.Vertices[index].z;
                }
                else if (newMesh.Vertices[index].z > maxPoint)
                {
                    maxPoint = newMesh.Vertices[index].z;
                }
            }

            newMesh.Length = maxPoint - newMesh.MinPoint;

            return newMesh;
        }
    }
}