using System.Xml.Serialization;
using System.Collections.Generic;
[System.Serializable]
public class RaceLevel
{
    [XmlAttribute]
    public int level;
    [XmlAttribute]
    public float speed;
    [XmlAttribute]
    public string color;
    [XmlAttribute]
    public string shape;
    [XmlArray("prefabs")]
    [XmlArrayItem("prefab")]
    public List<string> prefabs = new List<string>();
}