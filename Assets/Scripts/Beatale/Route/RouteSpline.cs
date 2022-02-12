using Beatale.Route.Curve;
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


                Handles.color = Color.blue;
                Handles.DrawLine(routeSamples[index].Position, routeSamples[index].Position + routeSamples[index].Up);

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
            var upVector = Vector3.up;
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
                var lastUpVector = upVector;
                while (currentDistance < lut[lut.Length - 1])
                {
                    var t = CubicCurve.DistanceToTValue(RouteVertices[index], RouteVertices[index + 1], currentDistance, ref lut, out bool isBetween);
                    routeSamples.Add(CubicCurve.GetRouteSample(RouteVertices[index], RouteVertices[index + 1], ref upVector, t));
                    currentDistance += distance;
                }
                var angleDifference = Vector3.SignedAngle(lastUpVector, upVector, RouteVertices[index].Direction1);
                leftDistance = currentDistance - lut[lut.Length - 1];
            }
            return routeSamples;
        }
    }
}
