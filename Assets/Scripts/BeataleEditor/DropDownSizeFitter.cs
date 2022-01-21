using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BeataleEditor
{
    [RequireComponent(typeof(RectTransform))]
    public class DropDownSizeFitter : MonoBehaviour
    {
        public float HorizontalPadding;
        public float VerticalPadding;
        public float TextLeftMargin;

        private RectTransform rectTransform;
        private RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.delayCall += _OnValidate;
        }
#endif

        private void _OnValidate()
        {
            if (this == null) return;
            UpdateFitting();
        }

        private void UpdateFitting()
        {
            float maxWidth = float.MinValue;
            float height = 0;

            for (int index = 0; index < transform.childCount; index++)
            {
                var child = transform.GetChild(index);
                var rect = child.GetComponent<RectTransform>();
                var text = child.GetComponentInChildren<Text>();
                var textRect = text.GetComponent<RectTransform>();

                var width = text.preferredWidth;
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -height);
                textRect.anchoredPosition = new Vector2(TextLeftMargin, textRect.anchoredPosition.y);
                height += rect.sizeDelta.y;
                if (width > maxWidth) maxWidth = width;
            }

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Ceil(maxWidth + HorizontalPadding));
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Ceil(height));
        }
    }

}
