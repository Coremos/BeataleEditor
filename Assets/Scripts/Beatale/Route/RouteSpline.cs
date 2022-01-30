using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Beatale.Route
{
    public class RouteSpline : MonoBehaviour
    {
        public List<RouteVertex> RouteVertices;
        public float VertexRadius;
        public Color VertexColor;
        public Color SplineColor;
        public int Resolution;
        public bool IsLoop;

        private void OnDrawGizmos()
        {
            DrawVertices();
            DrawSpline();
        }

        public void DrawVertices()
        {
            Gizmos.color = VertexColor;
            Gizmos.DrawSphere(RouteVertices[0].Position, VertexRadius);
            for (int index = 1; index < RouteVertices.Count - 1; index++)
            {
                Gizmos.DrawSphere(RouteVertices[index].Position, VertexRadius);
            }
            Gizmos.DrawSphere(RouteVertices[RouteVertices.Count - 1].Position, VertexRadius);
        }

        public void DrawSpline()
        {
            Gizmos.color = SplineColor;
            if (RouteVertices.Count > 1)
            {
                for (int index = 0; index < RouteVertices.Count - 1; index++)
                {
                    DrawCurve(RouteVertices[index], RouteVertices[index + 1]);
                }

                if (IsLoop)
                {
                    DrawCurve(RouteVertices[RouteVertices.Count - 1], RouteVertices[0]);
                }
            }
        }

        public void DrawCurve(RouteVertex vertex1, RouteVertex vertex2)
        {
            var currentPoint = vertex1.Position;
            var resolutionStep = 1.0f / Resolution;
            for (int index = 1; index <= Resolution; index++)
            {
                var nextPoint = GetPoint(vertex1, vertex2, resolutionStep * index);
                Gizmos.DrawLine(currentPoint, nextPoint);
                currentPoint = nextPoint;
            }
        }

        public Vector3 GetPoint(RouteVertex vertex1, RouteVertex vertex2, float t)
        {
            if (vertex1.Direction2 == Vector3.zero)
            {
                if (vertex2.Direction1 == Vector3.zero) return Line.GetPoint(vertex1.Position, vertex2.Position, t);
                else return QuadraticCurve.GetPoint(vertex1.Position, vertex2.GlobalDirection1, vertex2.Position, t);
            }
            else
            {
                if (vertex2.Direction1 == Vector3.zero) return QuadraticCurve.GetPoint(vertex1.Position, vertex1.GlobalDirection2, vertex2.Position, t);
                else return CubicCurve.GetPoint(vertex1.Position, vertex1.GlobalDirection2, vertex2.GlobalDirection1, vertex2.Position, t);
            }
        }

        
    }

    public class CubicCurve
    {
        
        public float GetLength()
        {
            return 0;
        }

        public float DistanceToTValue()
        {
            return 0;
        }

        public static Vector3 GetVelocity(Vector3 position1, Vector3 direction1, Vector3 direction2, Vector3 position2, float t)
        {
            float subT = 1.0f - t;

            return -subT * subT * position1 +
                (3 * subT * subT - 2 * subT) * direction1 +
                (-3 * t * t + 2 * t) * direction2 +
                t * t * position2;
        }

        public static Vector3 GetPoint(Vector3 position1, Vector3 direction1, Vector3 direction2, Vector3 position2, float t)
        {
            float subT = 1.0f - t;

            return subT * subT * subT * position1 +
                3 * subT * subT * t * direction1 +
                3 * subT * t * t * direction2 +
                t * t * t * position2;
        }

        public static float GetApproximateLength(RouteVertex vertex1, RouteVertex vertex2, int resolution)
        {
            float resolutionStep = 1.0f / resolution;
            var currentPoint = vertex1.Position;
            Vector3 distance = Vector3.zero;
            for (int index = 1; index <= resolution; index++)
            {
                var nextPoint = GetPoint(vertex1.Position, vertex1.Direction2, vertex2.Direction1, vertex2.Position, resolutionStep * index);
                distance += nextPoint - currentPoint;
                currentPoint = nextPoint;
            }
            return distance.magnitude;
        }
    }

    public class Line
    {
        public static Vector3 GetPoint(Vector3 vertex1, Vector3 vertex2, float t)
        {
            return vertex1 + ((vertex2 - vertex1) * t);
        }
    }

    public class QuadraticCurve
    {
        public static Vector3 GetVelocity(Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float subT = 1.0f - t;
            return 2 * subT * p1 +
                (2 - 4 * t) * p2 +
                2 * t * p3;
        }

        public static Vector3 GetPoint(Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float subT = 1.0f - t;

            return subT * subT * p1 +
                2 * subT * t * p2 +
                t * t * p3;
        }
    }
}
