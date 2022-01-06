using UnityEngine;

namespace BeataleEditor
{
    class Mouse : MonoBehaviour
    {
        public static Vector2 Position;
        public static Vector2 DeltaPosition;

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            var lastPosition = Position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out Position);
            DeltaPosition = Position - DeltaPosition;
        }
    }

}
