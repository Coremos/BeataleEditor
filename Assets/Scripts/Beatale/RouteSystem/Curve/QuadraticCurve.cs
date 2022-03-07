using UnityEngine;

namespace Beatale.RouteSystem.Curve
{
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
