using UnityEngine;

namespace Beatale.Route
{
    public struct RouteSample
    {
        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 Up;

        public RouteSample(Vector3 position, Vector3 direction, Vector3 up)
        {
            Position = position;
            Direction = direction;
            Up = up;
        }

        public static RouteSample Lerp(RouteSample routeSample1, RouteSample routeSample2, float t)
        {
            return new RouteSample(
                Vector3.Lerp(routeSample1.Position, routeSample2.Position, t),
                Vector3.Lerp(routeSample1.Direction, routeSample2.Direction, t).normalized,
                Vector3.Lerp(routeSample1.Up, routeSample2.Up, t)
                );
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var routeSample = (RouteSample)obj;
            return Position.Equals(routeSample.Position) &&
                Direction.Equals(routeSample.Direction) &&
                Up.Equals(routeSample.Up);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(RouteSample routeSample1, RouteSample routeSample2)
        {
            return routeSample1.Equals(routeSample2);
        }

        public static bool operator !=(RouteSample routeSample1, RouteSample routeSample2)
        {
            return !routeSample1.Equals(routeSample2);
        }
    }
}