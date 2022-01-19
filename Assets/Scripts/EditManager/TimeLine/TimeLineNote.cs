using BeataleEditor;
using UnityEngine;

public class TimeLineNote : RectTransformEvent
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnMouseDrag()
    {
        Vector2 mousePosition = GetMousePosition();
        rectTransform.anchoredPosition = new Vector2(mousePosition.x, rectTransform.anchoredPosition.y);
        Debug.Log("드래그중");
    }
}