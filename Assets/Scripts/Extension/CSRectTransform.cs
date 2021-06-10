using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This Class Expands Methods RectTransfrom
public static class CSRectTransform
{
    #region Set anchored position one axis
    public static void SetAnchoredX(this RectTransform transform, float x)
    {
        Vector2 newPosition =
            new Vector2(x, transform.anchoredPosition.y);

        transform.anchoredPosition = newPosition;
    }
    public static void SetAnchoredY(this RectTransform transform, float y)
    {
        Vector2 newPosition =
            new Vector2(transform.anchoredPosition.x, y);

        transform.anchoredPosition = newPosition;
    }
    #endregion

    #region Set anchored position3D one axis
    public static void SetAnchored3DX(this RectTransform transform, float x)
    {
        Vector2 newPosition =
            new Vector2(x, transform.anchoredPosition3D.y);

        transform.anchoredPosition3D = newPosition;
    }
    public static void SetAnchored3DY(this RectTransform transform, float y)
    {
        Vector2 newPosition =
            new Vector2(transform.anchoredPosition3D.x, y);

        transform.anchoredPosition3D = newPosition;
    }
    #endregion

    #region Set rotation one axis
    public static void SetRotationX(this RectTransform transform, float x)
    {
        Vector2 newRotation =
            new Vector2(x, transform.eulerAngles.y);

        transform.eulerAngles = newRotation;
    }
    public static void SetRotationY(this RectTransform transform, float y)
    {
        Vector2 newRotation =
            new Vector2(transform.eulerAngles.x, y);

        transform.eulerAngles = newRotation;
    }
    #endregion
    #region Set local rotation one axis
    public static void SetLocalRotationX(this RectTransform transform, float x)
    {
        Vector2 newRotation =
            new Vector2(x, transform.localEulerAngles.y);

        transform.eulerAngles = newRotation;
    }
    public static void SetLocalRotationY(this RectTransform transform, float y)
    {
        Vector2 newRotation =
            new Vector2(transform.localEulerAngles.x, y);

        transform.eulerAngles = newRotation;
    }
    #endregion
    #region Set local scale one axis
    public static void SetLocalScaleX(this RectTransform transform, float x)
    {
        Vector2 newScale =
            new Vector2(x, transform.localScale.y);

        transform.localScale = newScale;
    }
    public static void SetLocalScaleY(this RectTransform transform, float y)
    {
        Vector2 newScale =
            new Vector2(transform.localScale.x, y);

        transform.localScale = newScale;
    }
    #endregion
}