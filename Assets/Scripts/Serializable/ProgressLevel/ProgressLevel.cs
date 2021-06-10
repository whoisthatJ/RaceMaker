using System.Xml.Serialization;
[System.Serializable]
public class ProgressLevel
{
    [XmlAttribute]
    public int level;
    [XmlAttribute]
    public int exp;
    [XmlAttribute]
    public int gold;
}