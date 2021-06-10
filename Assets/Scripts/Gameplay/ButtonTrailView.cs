using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTrailView : MonoBehaviour {

    public List<Image> points;

    public void SetRoad(List<Points> trail)
    {
        foreach (Image i in points)
        {
            //i.color = new Color(1f, 1f, 1f, 0f);
            i.gameObject.SetActive(false);
        }
        foreach (Points p in trail)
        {
            //if (p.transform.gameObject.activeSelf)
            {
                int index = (int)(p.transform.localPosition.y + 3) * 7 + (int)(p.transform.localPosition.x + 3);
                points[index].sprite = p.sprite;
                //points[index].color = Color.white;
                points[index].gameObject.SetActive(true);
            }
        }
    }
}
