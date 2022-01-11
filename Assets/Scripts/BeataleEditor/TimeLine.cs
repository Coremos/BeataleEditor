using Beatale.Chart;
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
        public GameObject TimeLineMarkGameObject;
        public GameObject NotePrefab;
        private RectTransform timeLineMarkRectTransform;
        private Chart chart;

        public float areaStart;
        public float areaEnd;
        public float ViewStart;
        public float ViewEnd;
        public float ViewLength;
        public float Factor;

        protected override void Awake()
        {
            base.Awake();
            timeLineMarkRectTransform = GetComponent<RectTransform>();
            Factor = 100.0f;
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetMouseButton(1))
            {

            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Factor += 1f;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Factor -= 1f;
            }
        }

        public float GetPercentage()
        {
            var position = GetMousePosition();
            return (position.x / rectTransform.sizeDelta.x);
        }

        public void UpdateNote()
        {
            for (int index = 0; index < chart.Notes.Count; index++)
            {
                if (chart.Notes[index] == null) continue;

            }
        }

        public void AddNote()
        {
            var noteObject = Instantiate(NotePrefab);
            var note = new Note();
            
            chart.Notes.Add(note);
        }

        public void InitializeNote()
        {

        }

        public override void OnMouseEnter()
        {
            TimeLineMarkGameObject.SetActive(true);
        }

        public override void OnMouseOver()
        {
            timeLineMarkRectTransform.anchoredPosition = new Vector2(GetMousePosition().x, timeLineMarkRectTransform.anchoredPosition.y);
        }

        public override void OnMouseExit()
        {
            TimeLineMarkGameObject.SetActive(false);
        }
    }
}
