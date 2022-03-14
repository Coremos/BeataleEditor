using System.Collections.Generic;
using System.Linq;
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
    }

    public enum NoteType { None = -1, Tap, Long }

    public struct NotePosition
    {
        public float Bar;
        public int Numerator;
        public int Denominator;
    }

    public class Note
    {
        public float Time;
        public float Degree;
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

    public class TapNote : Note
    {
        public TapNote()
        {

        }
    }

    public class LongNoteVertex
    {
        public float Degree;
        public float Width;
        public Vector2 Direction1;
        public Vector2 Direction2;
    }

    public class LongNoteSampler
    {
        private static int DEFAULT_RESOLUTION = 10;

        public List<LongNoteSample> GetLongNoteSamples(LongNoteVertex vertex1, LongNoteVertex vertex2, int resolution)
        {
            var samples = new List<LongNoteSample>();
            return samples;
        }

        public LongNoteSample GetLongNoteSample(LongNoteVertex vertex1, LongNoteVertex vertex2, float t)
        {
            var sample = new LongNoteSample();
        }
    }

    public class LongNote : Note
    {
        public float Length;
        public List<LongNoteVertex> LongNoteVertices;
        public LongNote()
        {
            LongNoteVertices = new List<LongNoteVertex>();
        }
    }

    public class BPMChange
    {
        public float Time;
        public GameObject TunerObject;
        public float BPM;
    }
}