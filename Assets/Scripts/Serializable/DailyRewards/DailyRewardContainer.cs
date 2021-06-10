using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class DailyRewardContainer
{
    [XmlArray("rewards")]
    [XmlArrayItem("reward")]
    public List<DailyReward> rewards = new List<DailyReward>();

    public static DailyRewardContainer Load(string path)
    {
        var serialiser = new XmlSerializer(typeof(DailyRewardContainer));
        TextAsset file = (TextAsset)ServiceResources.Load(path);
        return serialiser.Deserialize(new StringReader(file.text)) as DailyRewardContainer;
    }
}
