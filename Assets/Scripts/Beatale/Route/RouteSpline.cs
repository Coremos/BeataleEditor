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
            var nextPoint = Vector3.zero;
            var resolutionStep = 1.0f / Resolution;
            for (int index = 1; index <= Resolution; index++)
            {
                nextPoint = GetPoint(vertex1, vertex2, resolutionStep * index);
                Gizmos.DrawLine(currentPoint, nextPoint);
                //Debug.Log("index" + index + " / " + currentPoint + " : " + nextPoint);
                //Handles.DrawBezier(vertex1.Position, vertex2.Position, vertex1.Direction2, vertex2.Direction1, RouteColor, )
                currentPoint = nextPoint;
            }
        }

        public Vector3 GetPoint(RouteVertex vertex1, RouteVertex vertex2, float t)
        {
            if (vertex1.Direction2 == Vector3.zero)
            {
                if (vertex2.Direction1 != Vector3.zero) return GetCubicCurvePoint(vertex1.Position, vertex1.GlobalDirection2, vertex2.GlobalDirection1, vertex2.Position, t);
                else return GetQuadraticCurvePoint(vertex1.Position, vertex1.GlobalDirection2, vertex2.Position, t);
            }
            else
            {
                if (vertex2.Direction1 != Vector3.zero) return GetQuadraticCurvePoint(vertex1.Position, vertex2.GlobalDirection1, vertex2.Position, t);
                else return GetLinearPoint(vertex1.Position, vertex2.Position, t);
            }
        }

        public Vector3 GetLinearPoint(Vector3 vertex1, Vector3 vertex2, float t)
        {
            return vertex1 + ((vertex2 - vertex1) * t);
        }

        public Vector3 GetCubicCurvePoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
        {
            t = Mathf.Clamp01(t);

            Vector3 part1 = Mathf.Pow(1 - t, 3) * p1;
            Vector3 part2 = 3 * Mathf.Pow(1 - t, 2) * t * p2;
            Vector3 part3 = 3 * (1 - t) * Mathf.Pow(t, 2) * p3;
            Vector3 part4 = Mathf.Pow(t, 3) * p4;

            return part1 + part2 + part3 + part4;
        }

        public Vector3 GetQuadraticCurvePoint(Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);

            Vector3 part1 = Mathf.Pow(1 - t, 2) * p1;
            Vector3 part2 = 2 * (1 - t) * t * p2;
            Vector3 part3 = Mathf.Pow(t, 2) * p3;

            return part1 + part2 + part3;
        }
    }
}
