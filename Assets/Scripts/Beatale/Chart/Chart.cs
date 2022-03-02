using System.Collections.Generic;
using UnityEngine;

namespace Beatale
{
    namespace ChartSystem
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

        public class NoteVertex
        {

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

        public class LongNote : Note
        {
            public float Length;
            public LongNote()
            {

            }
        }

        public class BPMChange
        {
            public float Time;
            public GameObject TunerObject;
            public float BPM;
        }
    }
}
