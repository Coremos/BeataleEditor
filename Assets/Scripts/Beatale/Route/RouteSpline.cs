using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Beatale.Route
{
    public class RouteSpline : MonoBehaviour
    {
        public List<RouteVertex> RouteVertices;
        public Color RouteColor;
        public int Resolution;
        public bool IsLoop;

        private void OnDrawGizmos()
        {
            Gizmos.color = RouteColor;
            Resolution = 1;
            DrawSpline();
        }

        public void DrawSpline()
        {
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
                //Handles.DrawBezier(vertex1.Position, vertex2.Position, vertex1.Direction2, vertex2.Direction1, RouteColor, )
                currentPoint = nextPoint;
            }
        }

        public Vector3 GetPoint(RouteVertex vertex1, RouteVertex vertex2, float t)
        {
            //if (vertex1.Direction1 == )
            if (vertex1.Direction2 == Vector3.zero)
            {

            }
            else
            {

            }
            Vector3 point = Vector3.zero;
            Debug.Log(vertex1.Position +" / " + vertex2.Position);
            return GetLinearPoint(vertex1.Position, vertex2.Position, t);
        }

        public Vector3 GetLinearPoint(Vector3 vertex1, Vector3 vertex2, float t)
        {
            return vertex1 + ((vertex2 - vertex1) * t);
        }
    }
}
