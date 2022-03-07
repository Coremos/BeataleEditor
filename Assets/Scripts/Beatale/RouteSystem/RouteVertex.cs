using UnityEngine;

namespace Beatale.RouteSystem
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

        [SerializeField]
        private Vector3 position;
        public Vector3 Position
        {
            get
            {
                if (position != transform.position) position = transform.position;
                return position;
            }
            set { position = transform.position = value; }
        }

        [SerializeField]
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

        [SerializeField]
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

        public Vector3 Up;

        private float roll;
        public float Roll;
    }
}