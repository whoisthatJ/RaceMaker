using System.Xml.Serialization;

[System.Serializable]
public class DailyReward 
{
    [XmlAttribute]
    public int day;
    [XmlAttribute]
    public string rewardType;
    [XmlAttribute]
    public string reward;
}
