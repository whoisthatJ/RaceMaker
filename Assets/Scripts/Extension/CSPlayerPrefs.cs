using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSPlayerPrefs
{
    public static void SetInt(string name, int value)
    {
        if (!string.IsNullOrEmpty(name))
        {
            PlayerPrefs.SetInt(name, value);
            Save();
        }
        else
        {
            Debug.LogError("Key is Empty!");
        }
    }

    public static void SetFloat(string name, float value)
    {
        if (!string.IsNullOrEmpty(name))
        {
            PlayerPrefs.SetFloat(name, value);
            Save();
        }
        else
        {
            Debug.LogError("Key is Empty!");
        }
    }

    public static void SetString(string name, string value)
    {
        if (!string.IsNullOrEmpty(name))
        {
            PlayerPrefs.SetString(name, value);
            Save();
        }
        else
        {
            Debug.LogError("Key is Empty!");
        }
    }

    public static int GetInt(string name)
    {
        int result = 0;
        if (PlayerPrefs.HasKey(name))
        {
            result = PlayerPrefs.GetInt(name);
        }
        else
        {
            Debug.LogError("[" + name + "] Key is Empty!");
        }
        return result;
    }

    public static float GetFloat(string name)
    {
        float result = 0;
        if (PlayerPrefs.HasKey(name))
        {
            result = PlayerPrefs.GetFloat(name);
        }
        else
        {
            Debug.LogError("[" + name + "] Key is Empty!");
        }
        return result;
    }

    public static string GetString(string name)
    {
        string result = string.Empty;
        if (PlayerPrefs.HasKey(name))
        {
            result = PlayerPrefs.GetString(name);
        }
        else
        {
            Debug.LogError("[" + name + "] Key is Empty!");
        }
        return result;
    }

    public static void SetVector3(string name, Vector3 value)
    {
        if (!string.IsNullOrEmpty(name))
        {
            PlayerPrefs.SetFloat(name + "x", value.x);
            PlayerPrefs.SetFloat(name + "y", value.y);
            PlayerPrefs.SetFloat(name + "z", value.z);
            Save();
        }
        else
        {
            Debug.LogError("Key is Empty!");
        }
    }

    public static void SetVector2(string name, Vector2 value)
    {
        if (!string.IsNullOrEmpty(name))
        {
            PlayerPrefs.SetFloat(name + "x", value.x);
            PlayerPrefs.SetFloat(name + "y", value.y);
            Save();
        }
        else
        {
            Debug.LogError("Key is Empty!");
        }
    }

    public static Vector3 GetVector3(string name)
    {
        Vector3 vec = Vector3.zero;
        if (PlayerPrefs.HasKey(name + "x") && PlayerPrefs.HasKey(name + "y") && PlayerPrefs.HasKey(name + "z"))
        {
            vec.x = PlayerPrefs.GetFloat(name + "x");
            vec.y = PlayerPrefs.GetFloat(name + "y");
            vec.z = PlayerPrefs.GetFloat(name + "z");
        }
        else
        {
            Debug.LogError("[" + name + "] Key is Empty!");
        }
        return vec;
    }

    public static Vector2 GetVector2(string name)
    {
        Vector2 vec = Vector2.zero;
        if (PlayerPrefs.HasKey(name + "x") && PlayerPrefs.HasKey(name + "y"))
        {
            vec.x = PlayerPrefs.GetFloat(name + "x");
            vec.y = PlayerPrefs.GetFloat(name + "y");
        }
        else
        {
            Debug.LogError("[" + name + "] Key is Empty!");
        }
        return vec;
    }

    public static bool HasKey(string name)
    {
        if (PlayerPrefs.HasKey(name))
            return true;
        else return false;
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
}
