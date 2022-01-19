using Beatale.Chart;
using UnityEngine;

namespace BeataleEditor
{
    class TimeLine : RectTransformEvent
    {
        public TunerManager TunerManager;
        public GameObject TimeLineMarkGameObject;
        public GameObject NotePrefab;
        public RectTransform RectT;
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
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Factor += 1f;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Factor -= 1f;
            }
            //Debug.Log(Time.deltaTime + " : " + RectT.anchoredPosition);
        }

        private void CreateNote()
        {
            var note = Instantiate(NotePrefab, RectT);
            //Debug.Log(GetMousePosition() + " / " + RectT.anchoredPosition);
            ((RectTransform)note.transform).anchoredPosition = new Vector2(GetMousePosition().x - RectT.anchoredPosition.x, 0);
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
            if (Input.GetMouseButtonDown(0))
            {
                CreateNote();
            }
            timeLineMarkRectTransform.anchoredPosition = new Vector2(GetMousePosition().x, timeLineMarkRectTransform.anchoredPosition.y);
        }

        public override void OnMouseExit()
        {
            TimeLineMarkGameObject.SetActive(false);
        }
    }
}
