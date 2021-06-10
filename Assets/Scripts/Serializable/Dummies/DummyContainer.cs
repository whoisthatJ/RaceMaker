using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class DummyContainer
{
    [XmlArray("dummies")]
    [XmlArrayItem("dummy")]
    public List<Dummy> dummies = new List<Dummy>();

    public static DummyContainer Load(string path)
    {
        var serialiser = new XmlSerializer(typeof(DummyContainer));
        TextAsset file = (TextAsset)ServiceResources.Load(path);
        return serialiser.Deserialize(new StringReader(file.text)) as DummyContainer;
    }
}