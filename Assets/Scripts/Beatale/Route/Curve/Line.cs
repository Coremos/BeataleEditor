using UnityEngine;

namespace Beatale.Route.Curve
{
    public class Line
    {
        public static Vector3 GetPoint(Vector3 vertex1, Vector3 vertex2, float t)
        {
            return vertex1 + ((vertex2 - vertex1) * t);
        }
    }
}
