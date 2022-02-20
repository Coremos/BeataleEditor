using Beatale.Route;
using System.Collections.Generic;
using UnityEngine;

namespace Beatale.TunnelSystem
{
    public class TunnelMeshBender : MonoBehaviour
    {
        public float Distance = 0.1f;
        public RouteSpline RouteSpline;
        public GameObject Tunnel;
        public float Speed;
        public Vector3 TestVector3;

        public float Roll;

        private List<RouteSample> routeSamples;
        private int currentIndex;
        private bool isMove;

        void Awake()
        {
            routeSamples = RouteSpline.GetRouteSamples(Distance);
            isMove = false;
            currentIndex = 0;
            Tunnel.transform.position = routeSamples[0].Position;
        }

        void Bend(Mesh mesh)
        {
            Mesh bentVertice = mesh.
            for (int index = 0; index < mesh.vertexCount; index++)
            {
                float distance = mesh.vertices[index].z;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) isMove = true;

            if (!isMove) return;

            if (currentIndex < routeSamples.Count)
            {
                Tunnel.transform.position = Vector3.MoveTowards(Tunnel.transform.position, routeSamples[currentIndex].Position, Speed * Time.deltaTime);
                Tunnel.transform.rotation = Quaternion.LookRotation(routeSamples[currentIndex].Direction, routeSamples[currentIndex].Up);
                if (Vector3.Distance(Tunnel.transform.position, routeSamples[currentIndex].Position) == 0f)
                {
                    currentIndex++;
                }
            }
            else currentIndex = 0;
        }
    }

}