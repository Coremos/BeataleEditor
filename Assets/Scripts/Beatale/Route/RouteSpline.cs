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
        public List<RouteSample> RouteSamples;

        private void OnDrawGizmos()
        {
            DrawVertices();
            DrawSpline();
            DrawSamples();
        }

        public void DrawSamples()
        {
            var routeSamples = GetRouteSamples(0.5f);
            for (int index = 0; index < routeSamples.Count; index++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(routeSamples[index].Position, VertexRadius * 0.5f); 
            }
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

        public List<RouteSample> GetRouteSamples(int resolution)
        {
            var routeSamples = new List<RouteSample>();
            for (int index = 0; index < RouteVertices.Count - 1; index++)
            {
            }
            return routeSamples;
        }

        public List<RouteSample> GetRouteSamples(float distance)
        {
            var routeSamples = new List<RouteSample>();
            var leftDistance = 0.0f;
            for (int index = 0; index < RouteVertices.Count - 1; index++)
            {
                var lut = CubicCurve.GenerateLUT(RouteVertices[index], RouteVertices[index + 1]);

                if (lut[lut.Length - 1] < leftDistance)
                {
                    leftDistance -= lut[lut.Length - 1];
                    continue;
                }

                float currentDistance = leftDistance;
                while (currentDistance < lut[lut.Length - 1])
                {
                    var t = CubicCurve.DistanceToTValue(RouteVertices[index], RouteVertices[index + 1], currentDistance, ref lut, out bool isBetween);
                    routeSamples.Add(CubicCurve.GetRouteSample(RouteVertices[index], RouteVertices[index + 1], t));
                    currentDistance += distance;
                }

                leftDistance = currentDistance - lut[lut.Length - 1];
            }
            return routeSamples;
        }
    }

    public class CubicCurve
    {
        private const int DEFAULT_RESOLUTION = 60;

        public static RouteSample GetRouteSample(RouteVertex vertex1, RouteVertex vertex2, float t)
        {
            return new RouteSample()
            {
                Position = GetPoint(vertex1, vertex2, t),
                Direction = GetVelocity(vertex1, vertex2, t)
            };
        }

        public static float[] GenerateLUT(RouteVertex vertex1, RouteVertex vertex2, int resolution = DEFAULT_RESOLUTION)
        {
            float resolutionStep = 1.0f / (resolution - 1);
            float[] lut = new float[resolution];
            lut[0] = 0;
            Vector3 currentPoint = vertex1.Position;

            for (int index = 1; index < resolution; index++)
            {
                var nextPoint = GetPoint(vertex1, vertex2, resolutionStep * index);
                lut[index] += lut[index - 1] + Vector3.Magnitude(nextPoint - currentPoint);
                currentPoint = nextPoint;
            }
            return lut;
        }

        public static float DistanceToTValue(RouteVertex vertex1, RouteVertex vertex2, float distance, ref float[] lut, out bool isBetween, int resolution = DEFAULT_RESOLUTION)
        {
            isBetween = false;

            if (distance > lut[lut.Length - 1]) return distance - lut[lut.Length - 1];

            isBetween = true;
            for (int index = 0; index < resolution - 1; index++)
            {
                if (distance > lut[index] && distance < lut[index + 1])
                {
                    return (index + (distance - lut[index]) / (lut[index + 1] - lut[index])) / (lut.Length - 1);
                }
            }
            return 0;
        }

        public static Vector3 GetVelocity(RouteVertex vertex1, RouteVertex vertex2, float t)
        {
            return GetVelocity(vertex1.Position, vertex1.GlobalDirection2, vertex2.GlobalDirection1, vertex2.Position, t);
        }

        public static Vector3 GetVelocity(Vector3 position1, Vector3 direction1, Vector3 direction2, Vector3 position2, float t)
        {
            float subT = 1.0f - t;

            return -subT * subT * position1 +
                (3 * subT * subT - 2 * subT) * direction1 +
                (-3 * t * t + 2 * t) * direction2 +
                t * t * position2;
        }

        public static Vector3 GetPoint(RouteVertex vertex1, RouteVertex vertex2, float t)
        {
            return GetPoint(vertex1.Position, vertex1.GlobalDirection2, vertex2.GlobalDirection1, vertex2.Position, t);
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
