using Beatale.RouteSystem;
using Beatale.RouteSystem.Curve;
using Beatale.TunnelSystem;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Beatale.ChartSystem
{
    public class TunnelNoteArranger : MonoBehaviour
    {
        public TunnelManager TunnelManager;
        public TunnelMeshGenerator TunnelMeshGenerator;
        public TunnelMeshBender TunnelMeshBender;
        public RouteSpline RouteSpline;
        public Chart Chart;
        public NoteObjectPool NotePool;
        public LongNoteObjectPool LongNotePool;
        public AudioSource SoundEffectTest;
        public float Interval;

        public float TunnelSpeed;

        public int BPMIndex;
        public float ChartTime;
        private bool isMove;
        
        private float timePerFrame = 0f;

        private float suddenOffset = 0.0f;
        private float[] radiusTable;

        private static float speed = 1.0f;
        private static double constant = 1.0 / 60.0 / 20.0;

        private Dictionary<float, RouteSample> routeSamples;


        private void Awake()
        {
            routeSamples = new Dictionary<float, RouteSample>();
            MakeChartTest();
            ChartIntervalCalculator.CalculateBPM(Chart);
            ChartIntervalCalculator.CalculateChart(Chart);

            ChartTimeCalculator.CalculateBPM(Chart);

            TunnelMeshGenerator.GenerateTunnelMesh();
            radiusTable = TunnelMeshGenerator.RadiusTable;

            LongNoteSampler.LongNoteSampling(Chart.LongNotes);
            LongNoteArranger.Project(Chart);
            //DebugChart(Chart);
            DebugLongNote(Chart);
            //LongNoteArranger.DebugLongNoteMeshes(Chart);
        }

        public void DebugLongNote(Chart chart)
        {
            for (int index = 0; index < chart.LongNotes.Count; index++)
            {
                var vertices = chart.LongNotes[index].LongNoteSamples;
                for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
                {
                    Debug.Log(index + " : " + vertexIndex + " / " + vertices[vertexIndex].Degree + "도 " + vertices[vertexIndex].Width + ", 시간" + vertices[vertexIndex].Time);
                }
            }
        }

        public void DebugChart(Chart chart)
        {
            Debug.Log(chart.BPMChanges[0].Position.Bar + ", " + chart.BPMChanges[0].Position.Numerator + " / " + chart.BPMChanges[0].Position.Denominator);
            for (int index = 0; index < chart.Notes.Count; index++)
            {
                Debug.Log(index + " : " + chart.Notes[index].Position.Bar + "번째 = " + chart.Notes[index].Position.Time);
            }
            for (int index = 0; index < chart.BPMChanges.Count; index++)
            {
                Debug.Log("BPM " + index + " : " + chart.BPMChanges[index].Position.Bar + "번째 = " + chart.BPMChanges[index].Position.Time);
            }
        }

        private void MakeChartTest()
        {
            Chart = new Chart();
            var startBPM = new BPMChange();
            startBPM.BPM = 140;

            Chart.BPMChanges.Add(startBPM);
            //var devide = 4;
            //for (int index = 0; index < 2000; index++)
            //{
            //    for (int quater = 0; quater < devide; quater++)
            //    {
            //        var note = new Note();
            //        note.Position.Bar = index;
            //        note.Position.Numerator = quater;
            //        note.Position.Denominator = devide;
            //        note.Degree = (index + quater) * 10;
            //        note.TunnelObject = null;
            //        note.TunnelPosition = Vector3.zero;
            //        Chart.Notes.Add(note);
            //    }
            //}

            //for (int index = 1; index < 9; index++)
            //{
            //    var bpmChange = new BPMChange();
            //    bpmChange.Position.Bar = (index * 8) - 1;
            //    bpmChange.BPM = startBPM.BPM * (1 - (1 & index) * 0.5);//(1 << index);
            //    //bpmChange.BPM = startBPM.BPM * (1 + (0.25 * index));
            //    Chart.BPMChanges.Add(bpmChange);
            //}

            AddLongNote3();
        }

        private void AddLongNote3()
        {
            for (int index = 0; index < 100; index += 1)
            {
                var longNote = new LongNote();
                var vertex1 = new LongNoteVertex();
                vertex1.Degree = (10 * index);
                vertex1.Width = 10;
                vertex1.Direction2 = new Vector2(-20 + (index & 1) * 40, 0);
                vertex1.Position.Bar = index;
                var vertex2 = new LongNoteVertex();
                vertex2.Degree = (10 * index);
                vertex2.Width = 10;
                vertex2.Position.Bar = index + 1;
                vertex2.Direction1 = new Vector2(-20 + (index & 1) * 40, 0);
                longNote.LongNoteVertices.Add(vertex1);
                longNote.LongNoteVertices.Add(vertex2);
                Chart.LongNotes.Add(longNote);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) isMove = !isMove; timePerFrame = (float)Chart.BPMChanges[BPMIndex].BPM * (float)Chart.BPMChanges[BPMIndex].BPM / 60.0f * (float)constant * speed;

            if (!isMove) return;
            if ((BPMIndex < Chart.BPMChanges.Count - 1) && (ChartTime > Chart.BPMChanges[BPMIndex + 1].Position.Time))
            {
                timePerFrame = (float)Chart.BPMChanges[BPMIndex].BPM * (float)Chart.BPMChanges[BPMIndex].BPM / 60.0f * (float)constant * speed;
                BPMIndex++;
            }
            ChartTime += timePerFrame * Time.deltaTime;
        }

        private void LateUpdate()
        {
            if (!isMove) return;
            Project();
        }

        private float GetRadius(float interval)
        {
            interval *= radiusTable.Length - 1;
            int index = (int)interval;
            if (index == interval) return radiusTable[index];
            return radiusTable[index] + (interval - index) * (radiusTable[index + 1] - radiusTable[index]);
        }

        private void Project()
        {
            var distanceMultiplier = 1.0f / TunnelMeshBender.BentMesh.Length;
            routeSamples.Clear();
            for (int index = 0; index < Chart.Notes.Count; index++)
            {
                float timeDifference = (float)Chart.Notes[index].Position.Time - ChartTime;
                float distance = timeDifference * TunnelSpeed;
                if (distance > TunnelMeshBender.BentMesh.Length - suddenOffset) break;
                if (distance <= 0.0f)
                {
                    if (Chart.Notes[index].TunnelObject == null) continue;
                    var gameObject = Chart.Notes[index].TunnelObject;
                    Chart.Notes[index].TunnelObject = null;
                    NotePool.RestoreObject(gameObject);
                    SoundEffectTest.PlayOneShot(SoundEffectTest.clip);
                    continue;
                }

                var noteInterval = distance * distanceMultiplier;
                distance += TunnelMeshBender.Distance;
                RouteSample routeSample;
                if (!routeSamples.TryGetValue(distance, out routeSample))
                {
                    routeSample = RouteSpline.GetRouteSample(distance);
                    routeSamples[distance] = routeSample;
                }

                var direction = Quaternion.AngleAxis(Chart.Notes[index].Degree, routeSample.Direction) * routeSample.Up;
                var position = routeSample.Position + direction * GetRadius(noteInterval);
                if (position.Equals(Vector3.zero)) continue;
                
                if (Chart.Notes[index].TunnelObject == null)
                {
                    Chart.Notes[index].TunnelObject = NotePool.GetObject();
                }
                Chart.Notes[index].TunnelObject.transform.position = position;
                Chart.Notes[index].TunnelObject.gameObject.SetActive(true);
            }

            for (int index = 0; index < Chart.LongNotes.Count; index++)
            {
                var longNote = Chart.LongNotes[index];
                float timeStartDifference = ((float)longNote.StartTime - ChartTime) * TunnelSpeed;
                float timeEndDifference = ((float)longNote.EndTime - ChartTime) * TunnelSpeed;
                if (timeStartDifference > TunnelMeshBender.BentMesh.Length - suddenOffset) continue;
                if (timeEndDifference <= 0.0f)
                {
                    if (longNote.TunnelObject == null) continue;
                    var gameObject = longNote.TunnelObject;
                    longNote.TunnelObject = null;
                    LongNotePool.RestoreObject(gameObject);
                    SoundEffectTest.PlayOneShot(SoundEffectTest.clip);
                    continue;
                }

                for (int vertexIndex = 0; vertexIndex < longNote.LongNoteMesh.AngleVertices.Length; vertexIndex++)
                {
                    var vertex = longNote.LongNoteMesh.AngleVertices[vertexIndex];
                    float distance = ((float)vertex.Time - ChartTime) * TunnelSpeed;
                   
                    if (distance > TunnelMeshBender.BentMesh.Length - suddenOffset) distance = TunnelMeshBender.BentMesh.Length - suddenOffset;
                    if (distance <= 0.0f) distance = 0.0f;

                    var noteInterval = distance * distanceMultiplier;
                    distance += TunnelMeshBender.Distance;

                    RouteSample routeSample;
                    if (!routeSamples.TryGetValue(distance, out routeSample))
                    {
                        routeSample = RouteSpline.GetRouteSample(distance);
                        routeSamples[distance] = routeSample;
                    }

                    var direction = Quaternion.AngleAxis(vertex.Degree, routeSample.Direction) * routeSample.Up;
                    var position = routeSample.Position + direction * GetRadius(noteInterval);
                    longNote.LongNoteMesh.Vertices[vertexIndex] = position;
                }

                if (longNote.TunnelObject == null)
                {
                    longNote.TunnelObject = LongNotePool.GetObject();
                    longNote.TunnelObject.transform.position = Vector3.zero;
                    longNote.TunnelObject.gameObject.SetActive(true);
                    longNote.TunnelObject.InitializeMesh(longNote.LongNoteMesh);
                }
                longNote.TunnelObject.UpdateMesh(longNote.LongNoteMesh);
            }
        }

        private void RestoreNote(GameObject noteObject)
        {

        }
    }
}