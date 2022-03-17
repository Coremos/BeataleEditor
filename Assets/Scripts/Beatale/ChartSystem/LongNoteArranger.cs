using UnityEngine;
using System.Linq;

namespace Beatale.ChartSystem
{
    public class LongNoteArranger : MonoBehaviour
    {
        public Chart chart;

        public void PreLoad()
        {
            TimeCalculator.CaculateNotesTime(chart.Notes);

            var s = chart.Notes.Select(note=>note.Position);
        }

        public void Project()
        {
            for (int index = 0; index < chart.LongNotes.Count; index++)
            {
                
            }
        }

        
    }
}
