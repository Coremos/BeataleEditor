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
        public float Interval;

        public float TunnelSpeed;
        public Stack<NoteTask> NoteTasks;

        private float suddenOffset = 0.0f;
        private float[] radiusTable;

        public class NoteTask
        {
            public enum TaskType { Move, Delete };
            public TaskType Type;
            public Note Note;
            public Vector3 Position;
            public Vector3 Normal;

            public NoteTask(TaskType type, Note note)
            {
                Type = type;
                Note = note;
            }

            public NoteTask(TaskType type, Note note, Vector3 position)
            {
                Type = type;
                Note = note;
                Position = position;
            }
        }

        private bool isRun;
        private bool isNeedUpdate;
        private Dictionary<float, RouteSample> routeSamples;

        private void Awake()
        {
            //noteSchedul = new Stack<Note>();
            routeSamples = new Dictionary<float, RouteSample>();
            NoteTasks = new Stack<NoteTask>();
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

            TunnelMeshGenerator.GenerateTunnelMesh();
            radiusTable = TunnelMeshGenerator.RadiusTable;
            //Thread thread = new Thread(ProjectNote);
            //isRun = true;
            //thread.Start();
        }

        private void LateUpdate()
        {
            isNeedUpdate = true;
            //NoteArrange();
            Project();
        }

        private float GetRadius(float interval)
        {
            interval *= radiusTable.Length - 1;
            int index = (int)interval;
            if (index == interval) return radiusTable[index];
            return radiusTable[index] + (interval - index) * (radiusTable[index + 1] - radiusTable[index]);
        }

        private void ProjectLongNotes()
        {
            routeSamples.Clear();
            for (int index = 0; index < Chart.LongNotes.Count; index++)
            {

            }
        }

        public void ArrangeLongNote(LongNote longNote)
        {
            for (int index = 0; index < longNote.LongNoteVertices.Count; index++)
            {
                longNote.LongNoteVertices[index];
                
            }
        }

        private void Project()
        {
            routeSamples.Clear();
            for (int index = 0; index < Chart.Notes.Count; index++)
            {
                float timeDifference = Chart.Notes[index].Time - TunnelManager.PlayTime;
                float distance = timeDifference * TunnelSpeed * TunnelManager.BPM;

                if (distance > TunnelMeshBender.BentMesh.Length - suddenOffset) break;
                if (distance <= 0.0f)
                {
                    if (Chart.Notes[index].TunnelObject == null) continue;
                    var gameObject = Chart.Notes[index].TunnelObject;
                    Chart.Notes[index].TunnelObject = null;
                    NotePool.RestoreObject(gameObject);
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
        }

        private void ArrangeNote()
        {
            
        }

        private void ProjectNote()
        {
            while (isRun)
            {
                if (!isNeedUpdate) continue;
                //isNeedUpdate = false;
                routeSamples.Clear();
                for (int index = 0; index < Chart.Notes.Count; index++)
                {
                    float timeDifference = Chart.Notes[index].Time - TunnelManager.PlayTime;
                    float distance = timeDifference * TunnelSpeed * TunnelManager.BPM;

                    if (distance > TunnelMeshBender.BentMesh.Length - suddenOffset) break;
                    if (distance <= 0.0f)
                    {
                        var deleteTask = new NoteTask(NoteTask.TaskType.Delete, Chart.Notes[index]);
                        NoteTasks.Push(deleteTask);
                        continue;
                    }

                    distance += TunnelMeshBender.Distance;

                    RouteSample routeSample;
                    if (!routeSamples.TryGetValue(distance, out routeSample))
                    {
                        routeSample = RouteSpline.GetRouteSample(distance);
                        routeSamples[distance] = routeSample;
                    }

                    var direction = Quaternion.AngleAxis(Chart.Notes[index].Degree, routeSample.Direction) * routeSample.Up;
                    var position = MeshProjector.GetProjection(routeSample.Position, direction, ref TunnelMeshBender.BentMesh);
                    if (position.Equals(Vector3.zero)) continue;
                    if ((float.IsNaN(position.x) || float.IsNaN(position.y)) || float.IsNaN(position.z)) continue;
                    var task = new NoteTask(NoteTask.TaskType.Move, Chart.Notes[index], position);
                    NoteTasks.Push(task);
                }
            }
        }

        private void NoteArrange()
        {
            while (NoteTasks.Count > 0)
            {
                var task = NoteTasks.Pop();
                if (task == null) continue;
                if (task.Type.Equals(NoteTask.TaskType.Delete))
                {
                    if (task.Note.TunnelObject == null) continue;
                    var gameObject = task.Note.TunnelObject;
                    task.Note.TunnelObject = null;
                    NotePool.RestoreObject(gameObject);
                    continue;
                }
                if (task.Note.TunnelObject == null)
                {
                    task.Note.TunnelObject = NotePool.GetObject();
                }
                task.Note.TunnelObject.transform.position = task.Position;
                task.Note.TunnelObject.SetActive(true);
            }
        }
    }
}