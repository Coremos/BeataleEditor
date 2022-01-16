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
        public TunerManager TunerManager;
        public GameObject TimeLineMarkGameObject;
        public GameObject NotePrefab;
        private RectTransform timeLineMarkRectTransform;

        public float areaStart;
        public float areaEnd;
        public float ViewStart;
        public float ViewEnd;
        public float ViewLength;
        public float Factor;

        protected override void Awake()
        {
            base.Awake();
            timeLineMarkRectTransform = TimeLineMarkGameObject.GetComponent<RectTransform>();
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

        private float GetPercentage()
        {
            var position = GetMousePosition();
            return (position.x / rectTransform.sizeDelta.x);
        }

        private void UpdateScroll()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel") * 10.0f;
            if (scroll != 0)
            {

            }
        }

        public void UpdateNote()
        {
            for (int index = 0; index < TunerManager.Chart.Notes.Count; index++)
            {
                if (TunerManager.Chart.Notes[index] == null) continue;

            }
        }

        public void AddNote()
        {
            var noteObject = Instantiate(NotePrefab);
            var note = new Note();
            
            TunerManager.Chart.Notes.Add(note);
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
