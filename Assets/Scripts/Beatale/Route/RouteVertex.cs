using UnityEngine;

namespace Beatale.Route
{
    public enum VertexType
    {
        Connected,
        Broken,
        None
    }

    public class RouteVertex : MonoBehaviour
    {
        public VertexType VertexType;
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        private Vector3 direction1;
        public Vector3 Direction1
        {
            get { return direction1; }
            set
            {
                direction1 = value;
                if (VertexType == VertexType.None) VertexType = VertexType.Broken;
                else if (VertexType == VertexType.Connected) direction2 = -value;
            }
        }

        private Vector3 direction2;
        public Vector3 Direction2
        {
            get { return direction2; }
            set
            {
                direction2 = value;
                if (VertexType == VertexType.None) VertexType = VertexType.Broken;
                else if (VertexType == VertexType.Connected) direction1 = -value;
            }
        }

        public Vector3 GlobalDirection1
        {
            get { return transform.TransformPoint(direction1); }
            set { direction1 = transform.InverseTransformPoint(value); }
        }

        public Vector3 GlobalDirection2
        {
            get { return transform.TransformPoint(direction2); }
            set { direction2 = transform.InverseTransformPoint(value); }
        }
    }
}