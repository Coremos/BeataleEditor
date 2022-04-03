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

    public struct NotePosition
    {
        public int Bar;
        public int Numerator;
        public int Denominator;
        public double Time;
    }

    public class Note
    {
        public float Time;
        public float Degree;
        public NotePosition Position;
        public GameObject TimeLineObject;
        public GameObject TunnelObject;
        public Vector3 TunnelPosition;

        public Note()
        {

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

    public class LongNote : Note
    {
        public float Length;
        public List<LongNoteVertex> LongNoteVertices;
        public List<LongNoteSample> LongNoteSamples;
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