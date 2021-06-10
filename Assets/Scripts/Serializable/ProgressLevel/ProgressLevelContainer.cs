using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class ProgressLevelContainer
{
    [XmlArray("levels")]
    [XmlArrayItem("level")]
    public List<ProgressLevel> levels = new List<ProgressLevel>();

    public static ProgressLevelContainer Load(string path)
    {
        var serialiser = new XmlSerializer(typeof(ProgressLevelContainer));
        TextAsset file = (TextAsset)ServiceResources.Load(path);
        return serialiser.Deserialize(new StringReader(file.text)) as ProgressLevelContainer;
    }
}
