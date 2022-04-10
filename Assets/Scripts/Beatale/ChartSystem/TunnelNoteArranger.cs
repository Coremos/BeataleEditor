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
        public ObjectPool NotePool;
        public AudioSource SoundEffectTest;
        public float Interval;

        public float TunnelSpeed;

        public int BPMIndex;
        public float ChartTime;
        private bool isMove;
        private static float speed = 1.0f;
        private static double constant = 1.0 / 60.0 / 20.0;
        private float timePerFrame = 0f;

        private float suddenOffset = 0.0f;
        private float[] radiusTable;

        private Dictionary<float, RouteSample> routeSamples;

        public static void GetTime(Chart chart)
        {
            var bpmChanges = chart.BPMChanges;
            var lastBPM = bpmChanges[0];

            var notes = chart.Notes;
            var longNotes = chart.LongNotes;
            var nextBPMIndex = 1;
            var oneBeatTime = 60.0 / lastBPM.BPM;
            //var oneBeatTime = lastBPM.BPM / 60.0;

            for (int index = 0; index < notes.Count; index++)
            {
                while (nextBPMIndex < bpmChanges.Count)
                {
                    if (notes[index].Position > bpmChanges[nextBPMIndex].Position)
                    {
                        lastBPM = bpmChanges[nextBPMIndex++];
                        oneBeatTime = 60.0 / lastBPM.BPM;
                        //oneBeatTime = lastBPM.BPM / 60.0;
                        continue;
                    }
                    break;
                }

                var fractionValue = 0.0;
                var position = notes[index].Position - lastBPM.Position;

                if (position.Denominator != 0) fractionValue = position.Numerator / (double)position.Denominator;

                notes[index].Position.Time = lastBPM.Position.Time +
                    oneBeatTime * (position.Bar + fractionValue);
            }

            for (int index = 0; index < longNotes.Count; index++)
            {

            }
        }

        public static void GetTime2(Chart chart)
        {
            var bpmChanges = chart.BPMChanges;
            var lastBPM = bpmChanges[0];

            var notes = chart.Notes;
            var longNotes = chart.LongNotes;
            var nextBPMIndex = 1;
            var oneBeatTime = lastBPM.BPM / 60.0;

            for (int index = 0; index < notes.Count; index++)
            {
                while (nextBPMIndex < bpmChanges.Count)
                {
                    if (notes[index].Position > bpmChanges[nextBPMIndex].Position)
                    {
                        lastBPM = bpmChanges[nextBPMIndex++];
                        oneBeatTime = lastBPM.BPM / 60.0;
                        continue;
                    }
                    break;
                }

                var fractionValue = 0.0;
                var position = notes[index].Position - lastBPM.Position;

                if (position.Denominator != 0) fractionValue = position.Numerator / (double)position.Denominator;

                notes[index].Position.Time = lastBPM.Position.Time +
                    oneBeatTime * (position.Bar + fractionValue);
            }

            for (int index = 0; index < longNotes.Count; index++)
            {

            }
        }

        public static void GetTime3(Chart chart)
        {
            var bpmChanges = chart.BPMChanges;
            var lastBPM = bpmChanges[0];

            var notes = chart.Notes;
            var longNotes = chart.LongNotes;
            var nextBPMIndex = 1;
            var oneBeatTime = lastBPM.BPM * constant * speed;

            for (int index = 0; index < notes.Count; index++)
            {
                while (nextBPMIndex < bpmChanges.Count)
                {
                    if (notes[index].Position > bpmChanges[nextBPMIndex].Position)
                    {
                        lastBPM = bpmChanges[nextBPMIndex++];
                        oneBeatTime = lastBPM.BPM * constant * speed;
                        continue;
                    }
                    break;
                }

                var fractionValue = 0.0;
                var position = notes[index].Position - lastBPM.Position;

                if (position.Denominator != 0) fractionValue = position.Numerator / (double)position.Denominator;

                notes[index].Position.Time = lastBPM.Position.Time +
                    oneBeatTime * (position.Bar + fractionValue);
            }

            lastBPM = bpmChanges[0];
            nextBPMIndex = 1;
            oneBeatTime = lastBPM.BPM * constant * speed;

            for (int longNoteIndex = 0; longNoteIndex < longNotes.Count; longNoteIndex++)
            {
                var vertices = longNotes[longNoteIndex].LongNoteVertices;
                for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
                {
                    while (nextBPMIndex < bpmChanges.Count)
                    {
                        if (vertices[vertexIndex].Position > bpmChanges[nextBPMIndex].Position)
                        {
                            lastBPM = bpmChanges[nextBPMIndex++];
                            oneBeatTime = lastBPM.BPM * constant * speed;
                            continue;
                        }
                        break;
                    }

                    var fractionValue = 0.0;
                    var position = vertices[vertexIndex].Position - lastBPM.Position;

                    if (position.Denominator != 0) fractionValue = position.Numerator / (double)position.Denominator;

                    vertices[vertexIndex].Position.Time = lastBPM.Position.Time +
                        oneBeatTime * (position.Bar + fractionValue);
                }
            }
        }

        private void Awake()
        {
            routeSamples = new Dictionary<float, RouteSample>();
            MakeChartTest();
            TimeCalculator.CalculateBPMTime3(Chart);
            GetTime3(Chart);

            TunnelMeshGenerator.GenerateTunnelMesh();
            radiusTable = TunnelMeshGenerator.RadiusTable;

            LongNoteSampler.LongNoteSampling(Chart.LongNotes);
            LongNoteArranger.Project(Chart);
            //DebugChart(Chart);
            //DebugLongNote(Chart);
            LongNoteArranger.DebugLongNoteMeshes(Chart);
        }

        public void DebugLongNote(Chart chart)
        {
            for (int index = 0; index < chart.LongNotes.Count; index++)
            {
                var vertices = chart.LongNotes[index].LongNoteSamples;
                for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
                {
                    Debug.Log(index + " : " + vertexIndex + " / " + vertices[vertexIndex].Degree + "도 " + vertices[vertexIndex].Width);
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

        private void MakeChartTest1()
        {
            Chart = new Chart();
            Chart.Notes = new List<Note>();
            for (int index = 0; index < 5000; index++)
            {
                var note = new Note();
                note.Time = index * 0.1f;
                note.Degree = index * 10;
                note.TunnelObject = null;
                note.TunnelPosition = Vector3.zero;
                Chart.Notes.Add(note);
            }
        }

        private void MakeChartTest()
        {
            Chart = new Chart();
            var startBPM = new BPMChange();
            startBPM.BPM = 174;
            //startBPM.BPM = 153;
            //startBPM.BPM = 240;
            //startBPM.BPM = 60;

            Chart.BPMChanges.Add(startBPM);
            var devide = 4;
            for (int index = 0; index < 2000; index++)
            {
                for (int quater = 0; quater < devide; quater++)
                {
                    var note = new Note();
                    note.Position.Bar = index;
                    note.Position.Numerator = quater;
                    note.Position.Denominator = devide;
                    note.Degree = (index + quater) * 10;
                    note.TunnelObject = null;
                    note.TunnelPosition = Vector3.zero;
                    Chart.Notes.Add(note);
                }
            }

            var longNote = new LongNote();
            var vertex1 = new LongNoteVertex();
            vertex1.Degree = 0;
            vertex1.Width = 20;
            vertex1.Direction2 = new Vector2(-1, 0);
            var vertex2 = new LongNoteVertex();
            vertex2.Degree = 0;
            vertex2.Width = 180;
            vertex2.Direction1 = new Vector2(-1, 0);
            longNote.LongNoteVertices.Add(vertex1);
            longNote.LongNoteVertices.Add(vertex2);
            Chart.LongNotes.Add(longNote);

            for (int index = 1; index < 9; index++)
            {
                var bpmChange = new BPMChange();
                bpmChange.Position.Bar = (index * 8) - 1;
                bpmChange.BPM = startBPM.BPM * (1 - (1 & index) * 0.5);//(1 << index);
                Chart.BPMChanges.Add(bpmChange);
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
            //ChartTime += (60.0f / (float)Chart.BPMChanges[BPMIndex].BPM) * Time.deltaTime;
            //ChartTime += ((float)Chart.BPMChanges[BPMIndex].BPM) * Time.deltaTime;
            //ChartTime += Time.deltaTime;
            //ChartTime += (float)Chart.BPMChanges[BPMIndex].BPM / 60.0f * Time.deltaTime;
            //ChartTime += (float)Chart.BPMChanges[BPMIndex].BPM / 60.0f * (float)Chart.BPMChanges[BPMIndex].BPM / 60.0f / (float)constant * speed * Time.deltaTime;
            //ChartTime += (float)Chart.BPMChanges[BPMIndex].BPM / 60.0f * (float)Chart.BPMChanges[BPMIndex].BPM * (float)constant * speed * Time.deltaTime;
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
            routeSamples.Clear();
            for (int index = 0; index < Chart.Notes.Count; index++)
            {
                //float timeDifference = (float)Chart.Notes[index].Position.Time - TunnelManager.PlayTime;
                float timeDifference = (float)Chart.Notes[index].Position.Time - ChartTime;
                float distance = timeDifference * TunnelSpeed * TunnelManager.BPM;
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

                var noteInterval = distance / TunnelMeshBender.BentMesh.Length;
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
                Chart.Notes[index].TunnelObject.SetActive(true);
            }

            //for (int index = 0; index < Chart.LongNotes.Count; index++)
            //{
            //    float timeStartDifference = (float)Chart.LongNotes[index].StartTime - ChartTime;
            //    float 
            //}
        }
    }
}