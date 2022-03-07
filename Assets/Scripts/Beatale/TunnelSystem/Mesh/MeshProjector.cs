using UnityEngine;

namespace Beatale.TunnelSystem
{ 
    public class MeshProjector
    {
        public static Vector3 GetProjection(Vector3 start, Vector3 direction, ref TunnelMesh tunnelMesh)
        {
            Vector3 position = Vector3.zero;
            for (int vertex = 0; vertex < tunnelMesh.Triangles.Length;)
            {
                position = GetIntersection(start, direction,
                    tunnelMesh.Vertices[tunnelMesh.Triangles[vertex++]],
                    tunnelMesh.Vertices[tunnelMesh.Triangles[vertex++]],
                    tunnelMesh.Vertices[tunnelMesh.Triangles[vertex++]]
                    );
                if (!position.Equals(Vector3.zero)) break;
            }
            return position;
        }

        public static Vector3 GetIntersection(Vector3 start, Vector3 direction, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            Vector3 vector1 = vertex2 - vertex1;
            Vector3 vector2 = vertex3 - vertex1;

            Vector3 normal = Vector3.Cross(vector1, vector2);

            float d = Vector3.Dot(-direction, normal);
            if (d < 0.0f) return Vector3.zero;

            Vector3 vector = start - vertex1;
            float t = Vector3.Dot(vector, normal);
            if (t < 0.0f) return Vector3.zero;

            Vector3 vector3 = Vector3.Cross(-direction, vector);
            float v = Vector3.Dot(vector2, vector3);
            if (v < 0.0f || v > d) return Vector3.zero;

            float w = -Vector3.Dot(vector1, vector3);
            if (w < 0.0f || v + w > d) return Vector3.zero;

            return start + t / d * direction;
        }
    }
}
