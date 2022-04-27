using System.Collections.Generic;
using UnityEngine;

namespace Beatale.ChartSystem
{
    public class Chart
    {
        public string Name;
        public float BPM;
        public string Artist;
        public string Pattern;
        public List<Note> Notes;
        public List<LongNote> LongNotes;
        public List<BPMChange> BPMChanges;
        public double Offset;
        public double Length;

        public Chart()
        {
            Notes = new List<Note>();
            LongNotes = new List<LongNote>();
            BPMChanges = new List<BPMChange>();
        }
    }

    public enum NoteType { None = -1, Tap, Long }

    public class Note
    {
        public float Degree;
        public float Width;

        public NotePosition Position;

        public GameObject TimeLineObject;
        public NoteTestObject TunnelObject;
        public Vector3 TunnelPosition;
        public NoteObjectPool Pool;

        public Note()
        {
            Position = new NotePosition();
            TimeLineObject = null;
        }

        public Note CopyDeep(Note note)
        {
            Note newNote = new Note();
            return newNote;
        }

        public void SetNote(NoteObjectPool pool)
        {
            Pool = pool;
            TunnelObject = Pool.GetObject();
            TunnelObject.gameObject.SetActive(true);
        }

        public void ReleaseNote()
        {
            if (TunnelObject == null) return;
            var gameObject = TunnelObject;
            TunnelObject = null;
            Pool.ReleaseObject(gameObject);
        }
    }

    public class LongNoteVertex
    {
        public float Degree;
        public float Width;

        public NotePosition Position;
        public Vector2 Direction1;
        public Vector2 Direction2;
    }

    public class LongNote
    {
        public double TimeStart;
        public double TimeEnd;
        public float IntervalStart;
        public float IntervalEnd;

        public List<LongNoteVertex> LongNoteVertices;
        public List<LongNoteSample> LongNoteSamples;
        public LongNoteMesh LongNoteMesh;
        public LongNoteObject TunnelObject;
        public LongNoteObjectPool Pool;

        public LongNote()
        {
            LongNoteVertices = new List<LongNoteVertex>();
            LongNoteMesh = null;
            Pool = null;
        }

        public void SetNote(LongNoteObjectPool pool)
        {
            Pool = pool;
            TunnelObject = pool.GetObject();
            TunnelObject.transform.position = Vector3.zero;
            TunnelObject.gameObject.SetActive(true);
            TunnelObject.InitializeMesh(LongNoteMesh);
        }

        public void ReleaseNote()
        {
            if (TunnelObject == null) return;
            var gameObject = TunnelObject;
            TunnelObject = null;
            Pool.ReleaseObject(gameObject);
        }

        public void UpdateMesh()
        {
            TunnelObject.UpdateMesh(LongNoteMesh);
        }
    }

    public class BPMChange
    {
        public GameObject TunnelObject;
        public NotePosition Position;
        public double BPM;
    }
}