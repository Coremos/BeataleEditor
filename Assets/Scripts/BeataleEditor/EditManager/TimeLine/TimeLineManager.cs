using Beatale.ChartSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BeataleEditor
{
    namespace TimeLine
    {
        public class TimeLineManager : RectTransformEvent
        {
            public TunnelManager TunerManager;
            public GameObject NotePrefab;
            public RectTransform ContentRect;
            public GameObject TimeLineMark;

            private List<TimeLineNote> SelectedNotes;

            public float areaStart;
            public float areaEnd;
            public float ViewStart;
            public float ViewEnd;
            public float ViewLength;
            public float Factor;

            protected override void Awake()
            {
                base.Awake();
                SelectedNotes = new List<TimeLineNote>();
                Factor = 100.0f;
            }

            private void Update()
            {
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

            public void SelectNote(TimeLineNote note)
            {
                if (SelectedNotes.Contains(note))
                {
                    SelectedNotes.Remove(note);
                }
                else
                {
                    SelectedNotes.Add(note);
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
                var noteObject = Instantiate(NotePrefab, ContentRect);
                noteObject.GetComponent<TimeLineNote>().TimeLineManager = this;
                ((RectTransform)noteObject.transform).anchoredPosition = new Vector2(GetMousePosition().x - ContentRect.anchoredPosition.x, 0);


                var note = new Note();
                note.TimeLineObject = noteObject;

                //TunerManager.Chart.Notes.Add(note);
            }

            public void DragNote()
            {

            }

            public void InitializeNotes()
            {

            }

            public override void OnPointerUp(PointerEventData eventData)
            {
            }

            public override void OnPointerDown(PointerEventData eventData)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    AddNote();
                }
                TimeLineMark.SetActive(true);
                ((RectTransform)TimeLineMark.transform).anchoredPosition = new Vector2(GetMousePosition().x, ((RectTransform)TimeLineMark.transform).anchoredPosition.y);
            }

            public override void OnPointerExit(PointerEventData eventData)
            {
                //TimeLineMark.SetActive(false);
            }
        }
    }
}