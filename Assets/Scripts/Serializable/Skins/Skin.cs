using System.Xml.Serialization;

[System.Serializable]
public class Skin{
    [XmlAttribute]
    public string id;
    public string name;

    [XmlAttribute]
    public string priceType;
    [XmlAttribute]
    public int price;
}
