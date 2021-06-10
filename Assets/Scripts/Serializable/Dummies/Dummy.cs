using System.Xml.Serialization;
using System.Collections.Generic;
[System.Serializable]
public class Dummy
{
    [XmlArray("prefabs")]
    [XmlArrayItem("prefab")]
    public List<string> prefabs = new List<string>();
}