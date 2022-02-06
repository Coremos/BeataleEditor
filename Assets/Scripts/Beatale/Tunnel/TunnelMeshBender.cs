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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) isMove = true;
            if (Input.GetKey(KeyCode.RightArrow)) Roll += 5f;
            if (Input.GetKey(KeyCode.LeftArrow)) Roll -= 5f;
            if (Input.GetKey(KeyCode.UpArrow)) Debug.Log(Quaternion.AngleAxis(Roll, Vector3.forward) * Vector3.up);
            if (!isMove) return;

            if (currentIndex < routeSamples.Count)
            {
                Tunnel.transform.position = Vector3.MoveTowards(Tunnel.transform.position, routeSamples[currentIndex].Position, Speed * Time.deltaTime);
                //Tunnel.transform.rotation = Quaternion.LookRotation(routeSamples[currentIndex].Direction);
                Tunnel.transform.rotation = Quaternion.Euler(Quaternion.AngleAxis(90.0f, routeSamples[currentIndex].Direction) * routeSamples[currentIndex].Direction);
                if (Vector3.Distance(Tunnel.transform.position, routeSamples[currentIndex].Position) == 0f)
                {
                    currentIndex++;
                }
            }
            else currentIndex = 0;
        }
    }

}