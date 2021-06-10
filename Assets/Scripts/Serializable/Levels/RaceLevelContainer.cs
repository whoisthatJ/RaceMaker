using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class RaceLevelContainer
{
    [XmlArray("levels")]
    [XmlArrayItem("level")]
    public List<RaceLevel> levels = new List<RaceLevel>();

    public static RaceLevelContainer Load(string path)
    {
        var serialiser = new XmlSerializer(typeof(RaceLevelContainer));
        TextAsset file = (TextAsset)ServiceResources.Load(path);
        return serialiser.Deserialize(new StringReader(file.text)) as RaceLevelContainer;
    }
}
