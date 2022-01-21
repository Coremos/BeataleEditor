using UnityEngine;
using UnityEngine.EventSystems;

namespace BeataleEditor
{
    namespace TimeLine
    {
        public class TimeLineContent : RectTransformEvent
        {
            public TimeLineManager TimeLineManager;

            protected override void Awake()
            {
                base.Awake();
            }

            public override void OnPointerDown(PointerEventData eventData)
            {
                TimeLineManager.AddNote();
            }
        }
    }
}
