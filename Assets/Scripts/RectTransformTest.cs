using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BeataleEditor
{
    public class RectTransformTest : MonoBehaviour
    {
        private RectTransform rectTransform;
        private bool isInside = false;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            Debug.Log(IsInsideWidth() + " " + IsInsideHeight() + " " + IsInside() + "" + rectTransform.anchoredPosition.y + "  " + rectTransform.anchoredPosition.y + rectTransform.sizeDelta.y);
        }

        public bool IsInsideEnter()
        {
            
            return IsInside();
        }

        public bool IsInside()
        {
            return IsInsideWidth() && IsInsideHeight();
        }

        public bool IsInsideWidth()
        {
            return (Mouse.Position.x >= rectTransform.anchoredPosition.x && 
                Mouse.Position.x <= rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x);
        }

        public bool IsInsideHeight()
        {
            return (Mouse.Position.y <= rectTransform.anchoredPosition.y &&
                Mouse.Position.y >= rectTransform.anchoredPosition.y - rectTransform.sizeDelta.y);
        }
    }
}
