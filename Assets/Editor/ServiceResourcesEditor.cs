using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(ServiceResourcesInfo))]
public class ServiceResourcesInfoEditor : Editor {

    ServiceResourcesInfo dataContainer;
    private void OnEnable()
    {
        dataContainer = (ServiceResourcesInfo)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("InitResources"))
        {
            dataContainer.ClearResources();
            InitResources();
        }
        if (GUILayout.Button("ClearResources"))
        {
            dataContainer.ClearResources();
        }
        base.OnInspectorGUI();

        EditorUtility.SetDirty(dataContainer);
    }
    private void InitResources()
    {
        InitAssets();
        InitTextAssets();
        InitPrefabs();
        InitSprites();
    }

    private void InitSprites()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/AlternativeResources");
        FileInfo[] files = dir.GetFiles("*.png", SearchOption.AllDirectories);
        Dictionary<string, Object> spriteDict = new Dictionary<string, Object>();
        for (int i = 0; i < files.Length; i++)
        {
            string path = files[i].FullName;
            path = path.Remove(0, Application.dataPath.Length);
            path = path.Insert(0, "Assets");
            Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);
            path = path.Replace(@"\", "/");
            path = path.Remove(0, "Assets/AlternativeResources/".Length);

            int lastIndex = path.LastIndexOf("/");

            if (lastIndex > 0)
                path = path.Remove(lastIndex);
            foreach (Object o in sprites)
            {
                string newName = path + "/" + o.name;
                spriteDict.Add(newName, o);
            }
        }
        dataContainer.LoadResources(spriteDict);
    }
    
    private void InitPrefabs()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/AlternativeResources");
        FileInfo[] files = dir.GetFiles("*.prefab", SearchOption.AllDirectories);
        Dictionary<string, Object> prefabDict = new Dictionary<string, Object>();
        for (int i = 0; i < files.Length; i++)
        {
            string path = files[i].FullName;
            path = path.Remove(0, Application.dataPath.Length);
            path = path.Insert(0, "Assets");
            GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            if (prefab != null)
            {
				path = path.Remove(0, "Assets/AlternativeResources/".Length);
                path = path.Replace(@"\", "/");
                int lastIndex = path.LastIndexOf("/");
				if (lastIndex > 0)                  
					path = path.Remove(lastIndex);              
				string newName = path + "/" + prefab.name;
                prefabDict.Add(newName, (Object)prefab);
            }
        }
        dataContainer.LoadResources(prefabDict);
    }

    private void InitAssets()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/AlternativeResources");
        FileInfo[] files = dir.GetFiles("*.asset", SearchOption.AllDirectories);
        Dictionary<string, Object> assetDict = new Dictionary<string, Object>();
        for (int i = 0; i < files.Length; i++)
        {
            string path = files[i].FullName;
            path = path.Remove(0, Application.dataPath.Length);
            path = path.Insert(0, "Assets");
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject));

            if (asset != null)
            {
                path = path.Remove(0, "Assets/AlternativeResources/".Length);
                int lastIndex = path.LastIndexOf(@"\");

                if (lastIndex > 0)
                    path = path.Remove(lastIndex);
                path = path.Replace(@"\", "/");
                string newName;
                if (path.Contains("/"))
                    newName = path + "/" + asset.name;
                else
                    newName = asset.name;
                assetDict.Add(newName, asset);
            }
        }
        dataContainer.LoadResources(assetDict);
    }

    private void InitTextAssets()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/AlternativeResources");
        FileInfo[] files = dir.GetFiles("*.xml", SearchOption.AllDirectories);
        Dictionary<string, Object> textAssetDict = new Dictionary<string, Object>();
        for (int i = 0; i < files.Length; i++)
        {
            string path = files[i].FullName;
            path = path.Remove(0, Application.dataPath.Length);
            path = path.Insert(0, "Assets");
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));

            if (asset != null)
            {
                string newName = "Xmls/"+asset.name;
                textAssetDict.Add(newName, asset);
            }
        }
        dataContainer.LoadResources(textAssetDict);
    }
}
