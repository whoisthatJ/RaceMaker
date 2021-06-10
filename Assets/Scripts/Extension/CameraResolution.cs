using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    [SerializeField] private List<ResolutionValue> listAspectRatio = new List<ResolutionValue>();
    private Camera cam;
    // Use this for initialization
    private void Start()
    {
        cam = GetComponent<Camera>();
        Init();
    }
    private void Init()
    {
        string aspect = CalcAspect();
        ResolutionValue rValue = listAspectRatio.Find(x => x.aspectRatioTitle == aspect);
        if (rValue != null)
        {
            if (!cam.orthographic)
                cam.fieldOfView = rValue.fieldOfView;
            else cam.orthographicSize = rValue.fieldOfView;
        }
    }

    private string CalcAspect()
    {
        float r = (float)Screen.width / (float)Screen.height;
        string _r = r.ToString("F2");
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
            case "2.10":
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
