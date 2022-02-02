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

        private List<RouteSample> routeSamples;
        private int currentIndex;
        private bool isMove;

        void Awake()
        {
            routeSamples = RouteSpline.GetRouteSamples(Distance);
            isMove = false;
            currentIndex = 0;
            Debug.Log(routeSamples.Count);

            Tunnel.transform.position = routeSamples[0].Position;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) isMove = true;

            if (!isMove) return;


            if (currentIndex < routeSamples.Count)
            {
                Tunnel.transform.position = Vector3.MoveTowards(Tunnel.transform.position, routeSamples[currentIndex].Position, Speed * Time.deltaTime);
                Tunnel.transform.rotation = Quaternion.LookRotation(routeSamples[currentIndex].Direction);
                if (Vector3.Distance(Tunnel.transform.position, routeSamples[currentIndex].Position) == 0f)
                {
                    currentIndex++;
                    Debug.Log(currentIndex);
                }
            }
            else currentIndex = 0;
        }
    }

}