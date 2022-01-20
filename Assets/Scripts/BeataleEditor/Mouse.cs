using System.Collections.Generic;
using UnityEngine;

namespace BeataleEditor
{
    class Mouse : MonoBehaviour
    {
        public static Dictionary<CursorType, Texture2D> CursorTextures;
        public enum CursorType { Normal = 0, Work, Horizontal, Vertical, Count }
        public static Vector2 Position;
        public static Vector2 DeltaPosition;

        private static readonly string cursorTexturePath = "Textures/Cursor/";
        private RectTransform canvasRectTransform;

        private void Awake()
        {
            canvasRectTransform = GetComponent<RectTransform>();
            InitializeCursorTextures();
        }

        private void InitializeCursorTextures()
        {
            CursorTextures = new Dictionary<CursorType, Texture2D>();
            for (int i = 0; i < (int)CursorType.Count; i++)
            {
                CursorTextures.Add((CursorType)i, Resources.Load<Texture2D>(cursorTexturePath + (CursorType)i));
            }
        }

        private void Update()
        {
            var lastPosition = Position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, null, out Position);
            DeltaPosition = Position - lastPosition;
        }

        public static void SetCursor(CursorType type)
        {
            Cursor.SetCursor(CursorTextures[type], Vector2.zero, CursorMode.ForceSoftware);
        }
    }

}
