using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class SkinContainer {
    [XmlArray("skins")]
    [XmlArrayItem("skin")]
    public List<Skin> skins = new List<Skin>();

    public static SkinContainer Load(string path)
    {
        var serialiser = new XmlSerializer(typeof(SkinContainer));
        TextAsset file = (TextAsset)ServiceResources.Load(path);
        return serialiser.Deserialize(new StringReader(file.text)) as SkinContainer;
    }
}
