using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class ButtonSizeFitter : EditorBehaviour
{
    public enum FitMode { Unconstrained, PrefferedSize };
    public FitMode HorizontalFit;
    public FitMode VerticalFit;
    public float HorizontalPadding;
    public float VerticalPadding;

    private RectTransform rectTransform;
    private RectTransform RectTransform
    {
        get
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            return rectTransform;
        }
    }

    private Text text;
    private Text Text
    {
        get
        {
            if (text == null) text = GetComponentInChildren<Text>();
            return text;
        }
    }

    private RectTransform textRect;
    private RectTransform TextRect
    {
        get
        {
            if (textRect == null) textRect = Text.GetComponent<RectTransform>();
            return textRect;
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
        if (HorizontalFit == FitMode.PrefferedSize) UpdateFitting(RectTransform.Axis.Horizontal);
    }

    private void UpdateFitting(RectTransform.Axis axis)
    {
        //RectTransform.sizeDelta = new Vector2(Text.preferredWidth, RectTransform.sizeDelta.y);
        RectTransform.SetSizeWithCurrentAnchors(axis, Mathf.Ceil(LayoutUtility.GetPreferredSize(TextRect, (int)axis) + HorizontalPadding));
    }
}
