using Beatale.Route.Curve;
using System;
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

        private static readonly float DEFAULT_SAMPLING_DISTANCE = 0.5f;
        private List<CurveSample> curveSamples;
        private float routeLength;

        private void OnDrawGizmos()
        {
            DrawVertices();
            DrawSpline();
            DrawCurveSamples();
        }

        private void DrawCurveSample(CurveSample sample)
        {
            for (int index = 0; index < sample.Samples.Count; index++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(sample.Samples[index].Position, VertexRadius * 0.5f);
                
                Gizmos.color = Color.white;
                Gizmos.DrawLine(sample.Samples[index].Position, sample.Samples[index].Position + sample.Samples[index].Up);
            }
        }

        private void DrawCurveSamples()
        {
            GetRouteSamples(DEFAULT_SAMPLING_DISTANCE);
            for (int index = 0; index < curveSamples.Count; index++)
            {
                DrawCurveSample(curveSamples[index]);
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

        public RouteSample GetRouteSample(float distance)
        {
            if (curveSamples == null)
            {
                GetRouteSamples(DEFAULT_SAMPLING_DISTANCE);
            }

            for (int index = 0; index < curveSamples.Count; index++)
            {
                if (distance < curveSamples[index].Length)
                {
                    return curveSamples[index].GetRouteSample(distance);
                }
                else
                {
                    distance -= curveSamples[index].Length;
                }
            }

            throw new Exception("Can't find RouteSample");
        }

        public List<RouteSample> GetRouteSamples(int resolution)
        {
            var routeSamples = new List<RouteSample>();
            for (int index = 0; index < RouteVertices.Count - 1; index++)
            {
            }
            return routeSamples;
        }

        public void GetRouteSamples(float distance)
        {
            curveSamples = new List<CurveSample>();
            float totalDistance = 0.0f;
            float leftDistance = 0.0f;
            Vector3 upVector = Quaternion.AngleAxis(RouteVertices[0].Roll, RouteVertices[0].Direction2) * Vector3.up;

            for (int index = 0; index < RouteVertices.Count - 1; index++)
            {
                float[] lut = CubicCurve.GenerateLUT(RouteVertices[index], RouteVertices[index + 1]);
                if (lut[lut.Length - 1] < leftDistance)
                {
                    leftDistance -= lut[lut.Length - 1];
                    continue;
                }

                var curveSample = new CurveSample();
                curveSample.Length = lut[lut.Length - 1];

                float currentDistance = leftDistance;
                float rollDifference = RouteVertices[index + 1].Roll - RouteVertices[index].Roll;

                var firstVertexSample = CubicCurve.GetRouteSample(RouteVertices[index], RouteVertices[index + 1], upVector, 0);
                firstVertexSample.Distance = 0.0f;
                curveSample.Samples.Add(firstVertexSample);

                while (currentDistance < lut[lut.Length - 1])
                {
                    float roll = rollDifference * currentDistance / lut[lut.Length - 1];
                    float t = CubicCurve.DistanceToTValue(RouteVertices[index], RouteVertices[index + 1], currentDistance, lut);
                    var routeSample = CubicCurve.GetRouteSample(RouteVertices[index], RouteVertices[index + 1], upVector, t);
                    upVector = routeSample.Up;

                    routeSample.Distance = currentDistance;
                    curveSample.Samples.Add(routeSample);

                    totalDistance += distance;
                    currentDistance += distance;
                }
                leftDistance = currentDistance - lut[lut.Length - 1];

                var lastVertexSample = CubicCurve.GetRouteSample(RouteVertices[index], RouteVertices[index + 1], upVector, 1);
                lastVertexSample.Distance = lut[lut.Length - 1];
                curveSample.Samples.Add(lastVertexSample);
                curveSamples.Add(curveSample);
            }
            routeLength = totalDistance;
        }
    }
}
