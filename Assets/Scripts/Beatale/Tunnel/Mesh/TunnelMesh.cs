using System.Collections.Generic;
using UnityEngine;

namespace Beatale.Tunnel
{
    public struct TunnelMesh
    {
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public int[] Triangles;
        public Vector2[] UV;

        public static TunnelMesh GenerateMesh(Mesh mesh)
        {
            var newMesh = new TunnelMesh();
            newMesh.Vertices = mesh.vertices;
            newMesh.Normals = mesh.normals;
            newMesh.Triangles = mesh.triangles;
            newMesh.UV = mesh.uv;

            return newMesh;
        }
    }
}