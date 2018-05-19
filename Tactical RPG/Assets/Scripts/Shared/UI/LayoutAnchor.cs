using System.Collections;
using UnityEngine;

/// <summary>
/// Will provide an easy way to move a RectTransform in relation to its parent
/// RectTransform.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class LayoutAnchor : MonoBehaviour {

    RectTransform rectTransform;
    RectTransform parentRectTransform;

    /// <summary>
    /// Set up RectTransform references
    /// </summary>
    private void Awake() {
        rectTransform = transform as RectTransform;
        parentRectTransform = transform.parent as RectTransform;
        if (parentRectTransform == null)
            Debug.LogError("ERROR: This component requires a RectTransform.");
    }


    /// <summary>
    /// Gets the general offset to se based on the location of the anchor and the size
    /// of the RectTransforms rect
    /// </summary>
    Vector2 GetPosition (RectTransform rectTransform, TextAnchor anchor) {
        Vector2 result = Vector2.zero;

        switch (anchor) {
            case TextAnchor.LowerCenter:
            case TextAnchor.MiddleCenter:
            case TextAnchor.UpperCenter:
                result.x += rectTransform.rect.width * 0.5f;
                break;
            case TextAnchor.LowerRight:
            case TextAnchor.MiddleRight:
            case TextAnchor.UpperRight:
                result.x += rectTransform.rect.width;
                break;
        }

        switch(anchor) {
            case TextAnchor.MiddleLeft:
            case TextAnchor.MiddleCenter:
            case TextAnchor.MiddleRight:
                result.y += rectTransform.rect.height * 0.5f;
                break;
            case TextAnchor.UpperLeft:
            case TextAnchor.UpperCenter:
            case TextAnchor.UpperRight:
                result.y += rectTransform.rect.height;
                break;
        }

        return result;
    }


    /// <summary>
    /// Find the value to use to make a RectTransform appear in the correct position based
    /// on the specified anchor points
    /// </summary>
    public Vector2 AnchorPosition (TextAnchor anchor, TextAnchor parentAnchor, Vector2 offset) {
        Vector2 childOffset = GetPosition(rectTransform, anchor);
        Vector2 parentOffset = GetPosition(parentRectTransform, parentAnchor);
        Vector2 anchorCenter = new Vector2(Mathf.Lerp(rectTransform.anchorMin.x,
            rectTransform.anchorMax.x, rectTransform.pivot.x), Mathf.Lerp(rectTransform.anchorMin.y,
            rectTransform.anchorMax.y, rectTransform.pivot.y));
        Vector2 anchorOffset = new Vector2(parentRectTransform.rect.width * anchorCenter.x,
            parentRectTransform.rect.height * anchorCenter.y);
        Vector2 pivotOffset = new Vector2(rectTransform.rect.width * rectTransform.pivot.x,
            rectTransform.rect.height * rectTransform.pivot.y);
        Vector2 pos = parentOffset - anchorOffset - childOffset + pivotOffset + offset;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);
        return pos;
    }


    /// <summary>
    /// Positions the anchors
    /// </summary>
    public void SnapToAnchorPosition(TextAnchor anchor, TextAnchor parentAnchor, Vector2 offset) {
        rectTransform.anchoredPosition = AnchorPosition(anchor, parentAnchor, offset);
    }


    /// <summary>
    /// Animates the RectTransform moving into position
    /// </summary>
    public Tweener MoveToAnchorPosition(TextAnchor anchor, TextAnchor parentAnchor, Vector2 offset) {
        return rectTransform.AnchorTo(AnchorPosition(anchor, parentAnchor, offset));
    }


}
