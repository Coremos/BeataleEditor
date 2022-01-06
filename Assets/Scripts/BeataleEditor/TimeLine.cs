using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BeataleEditor
{
    class TimeLine : RectTransformEvent
    {
        public GameObject TimeLineObject;
        private RectTransform timeLineRectTransform;

        protected override void Awake()
        {
            base.Awake();
            timeLineRectTransform = TimeLineObject.GetComponent<RectTransform>();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void OnMouseEnter()
        {
            TimeLineObject.SetActive(true);
        }
        public override void OnMouseOver()
        {
            timeLineRectTransform.anchoredPosition = Mouse.Position - ((RectTransform)transform).anchoredPosition;
        }
        public override void OnMouseExit()
        {
            TimeLineObject.SetActive(false);
        }
    }
}
