using Beatale.ChartSystem;
using Beatale.Route;
using System.Collections.Generic;
using UnityEngine;

namespace Beatale.TunnelSystem
{
    public class TunnelNoteProjector : MonoBehaviour
    {
        public TunnelManager TunnelManager;
        public TunnelMeshBender TunnelMeshBender;
        public RouteSpline RouteSpline;
        public Chart Chart;
        public float Interval;

        public float TunnelSpeed;

        private Dictionary<float, RouteSample> routeSamples;

        private void Awake()
        {
            routeSamples = new Dictionary<float, RouteSample>();
            Chart = new Chart();
            Chart.Notes = new List<Note>();
            for (int index = 0; index < 100; index++)
            {
                var note = new Note();
                note.Time = index * 1;
                note.Degree = index * 5;
                Chart.Notes.Add(note);
            }
        }

        private void Update()
        {
            GetProjection();
        }

        public void GetProjection()
        {
            routeSamples.Clear();
            for (int index = 0; index < Chart.Notes.Count; index++)
            {
                float timeDifference = Chart.Notes[index].Time - TunnelManager.PlayTime;
                float distance = timeDifference * TunnelManager.BPM * TunnelSpeed;

                if (distance > TunnelMeshBender.BentMesh.Length || distance < 0.0f) continue;
                distance += TunnelMeshBender.Distance;

                RouteSample routeSample;
                if (!routeSamples.TryGetValue(distance, out routeSample))
                {
                    routeSample = RouteSpline.GetRouteSample(distance);
                    routeSamples[distance] = routeSample;
                }

                var direction = Quaternion.AngleAxis(Chart.Notes[index].Degree, routeSample.Direction) * routeSample.Up;
                var position = MeshProjector.GetProjection(routeSample.Position, direction, ref TunnelMeshBender.BentMesh);
                if (!position.Equals(Vector3.zero)) Debug.DrawLine(routeSample.Position, position, Color.green);
            }
        }
    }
}
