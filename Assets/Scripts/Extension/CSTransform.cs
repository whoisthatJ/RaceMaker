using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This Class Expands Methods Transfrom
public static class CSTransform
{
    #region Set transform position one axis
    public static void SetX(this Transform transform, float x)
    {
        Vector3 newPosition =
            new Vector3(x, transform.position.y, transform.position.z);

        transform.position = newPosition;
    }
    public static void SetY(this Transform transform, float y)
    {
        Vector3 newPosition =
            new Vector3(transform.position.x, y, transform.position.z);

        transform.position = newPosition;
    }
    public static void SetZ(this Transform transform, float z)
    {
        Vector3 newPosition =
            new Vector3(transform.position.x, transform.position.y, z);

        transform.position = newPosition;
    }
    #endregion

    #region Set transform position one local axis
    public static void SetLocalX(this Transform transform, float x)
    {
        Vector3 newPosition =
            new Vector3(x, transform.localPosition.y, transform.localPosition.z);

        transform.localPosition = newPosition;
    }
    public static void SetLocalY(this Transform transform, float y)
    {
        Vector3 newPosition =
            new Vector3(transform.localPosition.x, y, transform.localPosition.z);

        transform.localPosition = newPosition;
    }
    public static void SetLocalZ(this Transform transform, float z)
    {
        Vector3 newPosition =
            new Vector3(transform.localPosition.x, transform.localPosition.y, z);

        transform.localPosition = newPosition;
    }
    #endregion

    #region Set rotation one axis
    public static void SetRotationX(this Transform transform, float x)
    {
        Vector3 newRotation =
            new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);

        transform.eulerAngles = newRotation;
    }
    public static void SetRotationY(this Transform transform, float y)
    {
        Vector3 newRotation =
            new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);

        transform.eulerAngles = newRotation;
    }
    public static void SetRotationZ(this Transform transform, float z)
    {
        Vector3 newRotation =
            new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, z);

        transform.eulerAngles = newRotation;
    }
    #endregion

    #region Set local rotation one axis
    public static void SetLocalRotationX(this Transform transform, float x)
    {
        Vector3 newRotation =
            new Vector3(x, transform.localEulerAngles.y, transform.localEulerAngles.z);

        transform.localEulerAngles = newRotation;
    }
    public static void SetLocalRotationY(this Transform transform, float y)
    {
        Vector3 newRotation =
            new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);

        transform.localEulerAngles = newRotation;
    }
    public static void SetLocalRotationZ(this Transform transform, float z)
    {
        Vector3 newRotation =
            new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, z);

        transform.localEulerAngles = newRotation;
    }
    #endregion

    #region Set Local Scale one axis
    public static void SetLocalScaleX(this Transform transform, float x)
    {
        Vector3 newPosition =
            new Vector3(x, transform.localScale.y, transform.localScale.z);
        transform.localScale = newPosition;
    }
    public static void SetLocalScaleY(this Transform transform, float y)
    {
        Vector3 newPosition =
            new Vector3(transform.localScale.x, y, transform.localScale.z);
        transform.localScale = newPosition;
    }
    public static void SetLocalScaleZ(this Transform transform, float z)
    {
        Vector3 newPosition =
            new Vector3(transform.localScale.x, transform.localScale.y, z);
        transform.localScale = newPosition;
    }

    public static Transform FindTransformInactive(this Transform parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t;
            }
        }
        return null;
    }

    public static GameObject FindObjectInactive(this GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
    #endregion
}