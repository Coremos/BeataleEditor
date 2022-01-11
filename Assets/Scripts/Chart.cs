using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beatale.Note;
using UnityEngine;

namespace Beatale
{
    namespace Chart
    {
        public class Chart
        {
            public string Name;
            public float BPM;
            public string Artist;
            public string Patterner;
            public List<Note> Notes;
            public List<LongNote> LongNotes;
        }

        public enum NoteType { None = -1, Tap, Long }

        public class Note
        {
            public float Time;
            public float Degree;
            public GameObject TimeLineObject;
            public GameObject TunerObject;

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
    }

    namespace Note
    {
        
    }

    
}
