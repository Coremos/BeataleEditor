using UnityEngine;

namespace Beatale.ChartSystem
{
    public class LongNoteArranger : MonoBehaviour
    {
        public Chart chart;

        public void Project()
        {
            for (int index = 0; index < chart.LongNotes.Count; index++)
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
    }
}
