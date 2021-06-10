using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourcesInfo", menuName = "Resources/Info", order = 1)]
[System.Serializable]
public class ServiceResourcesInfo : ScriptableObject
{

    public List<StringObjectPair> resourcesList;

    public void LoadResources(Dictionary<string, Object> dictresources)
    {
        if (resourcesList == null)
            resourcesList = new List<StringObjectPair>();
        foreach (KeyValuePair<string, Object> kp in dictresources)
        {
            StringObjectPair so = new StringObjectPair();
            so.key = kp.Key;
            so.value = kp.Value;
            resourcesList.Add(so);
        }
    }

    public void ClearResources()
    {
        if (resourcesList != null)
            resourcesList.Clear();
    }
}

[System.Serializable]
public class StringObjectPair
{
    public string key;
    public Object value;
}