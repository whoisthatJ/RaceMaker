using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RouteGridItem
{
    public Color color;
    public string name;
    public string nameSpriteRoute;
    public RouteGridItem()
    {

    }

    public RouteGridItem(Color color, string name, string nameSpriteRoute)
    {
        this.color = color;
        this.name = name;
        this.nameSpriteRoute = nameSpriteRoute;
    }
}
