using UnityEngine;

namespace BeataleEditor
{
    public class RectTransformEvent : MonoBehaviour
    {
        protected RectTransform rectTransform;
        private bool isRectangleContainsMouse;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            isRectangleContainsMouse = false;
        }

        protected virtual void Update()
        {
            var lastRectangleContainsMouse = isRectangleContainsMouse;
            isRectangleContainsMouse = IsRectangleContainsMouse();

            if (lastRectangleContainsMouse != isRectangleContainsMouse)
            {
                if (isRectangleContainsMouse) OnMouseEnter();
                else OnMouseExit();
            }

            if (isRectangleContainsMouse) OnMouseOver();
        }

        public virtual void OnMouseOver()
        {
            
        }

        public virtual void OnMouseEnter()
        {
        }

        public virtual void OnMouseExit()
        {
        }

        public Vector2 GetMousePosition()
        {
            return Mouse.Position - rectTransform.anchoredPosition;
        }

        public bool IsRectangleContainsMouse()
        {
            return IsRectangleContainsMouseHorizontal() && IsRectangleConatinsMouseVertical();
        }

        public bool IsRectangleContainsMouseHorizontal()
        {
            return (Mouse.Position.x >= rectTransform.anchoredPosition.x && 
                Mouse.Position.x <= rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x);
        }

        public bool IsRectangleConatinsMouseVertical()
        {
            return (Mouse.Position.y <= rectTransform.anchoredPosition.y &&
                Mouse.Position.y >= rectTransform.anchoredPosition.y - rectTransform.sizeDelta.y);
        }
    }
}
