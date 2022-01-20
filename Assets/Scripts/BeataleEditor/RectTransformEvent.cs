using UnityEngine;
using UnityEngine.EventSystems;

namespace BeataleEditor
{
    public class RectTransformEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        protected RectTransform rectTransform;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        #region VirtualMethods
        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {

        }
        #endregion

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
