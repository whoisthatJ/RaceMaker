using UnityEngine;
using UnityEditor;

public class AspectRatioCalculator : EditorWindow
{
    Vector2 xy = new Vector2(0, 0);
    string result = "Calculte";
    [MenuItem("Custom Editor/Aspect Ratio Calculator")]
    static void Init()
    {
        AspectRatioCalculator window = (AspectRatioCalculator)EditorWindow.GetWindow(typeof(AspectRatioCalculator));
    }

    void OnGUI()
    {
        xy = EditorGUI.Vector2Field(new Rect(3, 3, 500, 10), "Resolution", xy);
        xy = new Vector2(xy.x < 1 ? 1 : (int)xy.x, xy.y < 1 ? 1 : (int)xy.y);
        GUILayout.BeginVertical();
        if (GUI.Button(new Rect(3, 50, 300, 40), "Calculate Aspect Ratio\n" + result))
        {
            float aspectRatio = xy.x / xy.y;
            result = "Aspect Ratio = " + AspectRatio(aspectRatio) + " [" + aspectRatio + "] " + " (" + xy.x + "x" + xy.y + ")";
        }
        GUILayout.EndVertical();
        GUILayout.Space(100);
        GUILayout.BeginVertical();
        GUILayout.Label("FullHD - 1920x1080 [16x9]");
        GUILayout.Label("FullHD+ - 2220x1080 [18.5x9]");
        GUILayout.Label("FullHD+ - 1440x2960 [9x18.5]");
        GUILayout.Label("HD+ - 1480x720 [18.5x9]");
        GUILayout.Label("iPhone 6.5 - 1242x2688 [9x19.5]");
        GUILayout.Label("iPhone 5.5 - 1242x2208 [9x16]");
        GUILayout.Label("iPad 12.9 - 2048x2732 [3x4]");
        GUILayout.Label("Pixel C - 2560x1800 [1:√2]");
        GUILayout.Label("Nexus 7 - 2560x1800 [10x16]");
        GUILayout.EndVertical();
    }

    private string AspectRatio(float aspectRatio)
    {
        string _r = aspectRatio.ToString("F2");
        string ratio = _r.Substring(0, 4);
        string result = "Unknown";
        switch (ratio)
        {
            case "1.33":
                result = "4:3";
                break;
            case "0.75":
                result = "3:4";
                break;
            case "1.50":
                result = "3:2";
                break;
            case "0.67":
                result = "2:3";
                break;
            case "1.67":
                result = "5:3";
                break;
            case "0.60":
                result = "3:5";
                break;
            case "0.56":
                result = "9:16";
                break;
            case "1.78":
                result = "16:9";
                break;
            case "1.60":
                result = "16:10";
                break;
            case "0.63":
                result = "10:16";
                break;
            case "1.71":
                result = "17:10";
                break;
            case "0.59":
                result = "10:17";
                break;
            case "2.00":
                result = "18:9";
                break;
            case "0.50":
                result = "9:18";
                break;
            case "2.06":
                result = "18.5:9";
                break;
            case "0.49":
                result = "9:18.5";
                break;
            case "2.1":
                result = "19:9";
                break;
            case "0.47":
                result = "9:19";
                break;
            case "2.17":
                result = "19.5:9";
                break;
            case "0.46":
                result = "9:19.5";
                break;
            case "1.42":
                result = "1:√2";
                break;
            case "0.70":
                result = "√2:1";
                break;
        }
        return result;
    }
}