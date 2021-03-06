using UnityEngine;

namespace Beatale.RouteSystem.Curve
{
    public class CubicCurve
    {
        private const int DEFAULT_RESOLUTION = 60;

        public static RouteSample GetRouteSample(RouteVertex vertex1, RouteVertex vertex2, float t)
        {
            return new RouteSample()
            {
                Position = GetPoint(vertex1, vertex2, t),
                Direction = GetVelocity(vertex1, vertex2, t),
            };
        }

        public static RouteSample GetRouteSample(RouteVertex vertex1, RouteVertex vertex2, Vector3 upVector, float t)
        {
            var direction = GetVelocity(vertex1, vertex2, t);
            upVector = GetUp(direction, upVector);
            return new RouteSample()
            {
                Position = GetPoint(vertex1, vertex2, t),
                Direction = direction,
                Up = upVector,
                Rotation = Quaternion.LookRotation(direction, upVector)
            };
        }

        public static float[] GenerateLengthTable(Vector3 position1, Vector3 direction1, Vector2 direction2, Vector3 position2, int resolution)
        {
            float resolutionStep = 1.0f / (resolution - 1);
            float[] lut = new float[resolution];
            Vector3 currentPoint = position1;
            lut[0] = currentPoint.y;

            for (int index = 1; index < resolution; index++)
            {
                var nextPoint = GetPoint(position1, direction1, direction2, position2, resolutionStep * index);
                lut[index] = lut[index - 1] + nextPoint.y - currentPoint.y;
                currentPoint = nextPoint;
            }
            return lut;
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
                lut[index] = lut[index - 1] + Vector3.Magnitude(nextPoint - currentPoint);
                currentPoint = nextPoint;
            }
            return lut;
        }
        
        public static float[] GenerateLUT(Vector3 position1, Vector3 direction1, Vector2 direction2, Vector3 position2, int resolution = DEFAULT_RESOLUTION)
        {
            float resolutionStep = 1.0f / (resolution - 1);
            float[] lut = new float[resolution];
            lut[0] = 0;
            Vector3 currentPoint = position1;

            for (int index = 1; index < resolution; index++)
            {
                var nextPoint = GetPoint(position1, direction1, direction2, position2, resolutionStep * index);
                lut[index] = lut[index - 1] + Vector3.Magnitude(nextPoint - currentPoint);
                currentPoint = nextPoint;
            }
            return lut;
        }

        public static float DistanceToTValue(float distance, float[] lut)
        {
            for (int index = 1; index < lut.Length; index++)
            {
                if (distance <= lut[index])
                {
                    return (index - 1 + (distance - lut[index - 1]) / (lut[index] - lut[index - 1])) / (lut.Length - 1);
                }
            }
            return 1;
        }

        public static Vector3 GetVelocity(RouteVertex vertex1, RouteVertex vertex2, float t)
        {
            return GetVelocity(vertex1.Position, vertex1.GlobalDirection2, vertex2.GlobalDirection1, vertex2.Position, t).normalized;
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

        public static Vector3 GetUp(Vector3 direction, Vector3 up)
        {
            return Vector3.Cross(direction, Vector3.Cross(up, direction));
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
}
