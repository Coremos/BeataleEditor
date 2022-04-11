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

    
    public class ChartObject
    {
        NotePosition Position;
    }

    public class Note
    {
        public float Time;
        public float Degree;
        public NotePosition Position;
        public GameObject TimeLineObject;
        public NoteTestObject TunnelObject;
        public Vector3 TunnelPosition;

        public Note()
        {
            Position = new NotePosition();
        }

        public Note CopyDeep(Note note)
        {
            Note newNote = new Note();
            return newNote;
        }
    }

    public class LongNoteVertex
    {
        public float Degree;
        public float Width;
        public float Time;
        public NotePosition Position;
        public Vector2 Direction1;
        public Vector2 Direction2;
    }

    public class LongNote
    {
        public double StartTime;
        public double EndTime;
        public float Length;

        public List<LongNoteVertex> LongNoteVertices;
        public List<LongNoteSample> LongNoteSamples;
        public LongNoteMesh LongNoteMesh;
        public LongNoteObject TunnelObject;

        public LongNote()
        {
            LongNoteVertices = new List<LongNoteVertex>();
        }
    }

    public class BPMChange
    {
        public GameObject TunnelObject;
        public NotePosition Position;
        public double BPM;
    }
}