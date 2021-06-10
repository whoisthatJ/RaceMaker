using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trail
{
    public string name;
    public List<Points> transforms = new List<Points>();
    public List<Points> transformsPool = new List<Points>();
    public int side;
}
