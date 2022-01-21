using UnityEngine;
using UnityEngine.EventSystems;

namespace BeataleEditor
{
    namespace TimeLine
    {
        public class TimeLineNote : RectTransformEvent
        {
            public TimeLineManager TimeLineManager;
            private Vector2 mousePosition;
            private Vector2 lastPosition;

            protected override void Awake()
            {
                base.Awake();
            }

            public override void OnPointerDown(PointerEventData eventData)
            {
                TimeLineManager.SelectNote(this);

                mousePosition = Mouse.Position;
                lastPosition = rectTransform.anchoredPosition;
            }

            public override void OnPointerClick(PointerEventData eventData)
            {
            }

            public override void OnDrag(PointerEventData eventData)
            {
                TimeLineManager.DragNote();
                rectTransform.anchoredPosition = lastPosition + new Vector2(Mouse.Position.x - mousePosition.x, 0);
            }
        }
    }
}
