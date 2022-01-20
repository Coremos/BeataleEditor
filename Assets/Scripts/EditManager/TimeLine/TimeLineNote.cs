using BeataleEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimeLineNote : RectTransformEvent
{
    private Vector2 mousePosition;
    private Vector2 lastPosition;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        mousePosition = Mouse.Position;
        lastPosition = rectTransform.anchoredPosition;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = lastPosition + new Vector2(Mouse.Position.x - mousePosition.x, 0);
    }

    //public override void OnMouseDrag()
    //{
    //    Vector2 mousePosition = GetMousePosition();
    //    rectTransform.anchoredPosition = new Vector2(mousePosition.x, rectTransform.anchoredPosition.y);
    //    Debug.Log("드래그중");
    //}
}