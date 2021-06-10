// Creates a prefab from the selected GameObjects.
// If the prefab already exists it asks if you want to replace it.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ExampleClass : EditorWindow
{
    [MenuItem("Examples/Create Prefab From Selected")]
    static void CreatePrefab()
    {
        GameObject[] objs = Selection.gameObjects;
        Object[] spritesObject = AssetDatabase.LoadAllAssetsAtPath("Assets/Sprites/EnvAtlas.png");
        List<Sprite> sprites = new List<Sprite>();
        foreach (Object o in spritesObject)
        {
            if (o.GetType() == typeof(Sprite))
            {
                sprites.Add((Sprite)o);
                Debug.Log(o);
            }
        }
        
        foreach (GameObject go in objs)
        {

            Route r = go.GetComponent<Route>();
            foreach (Trail t in r.trueTrail)
            {
                foreach (Points p in t.transforms)
                {
                    SpriteRenderer sr = p.transform.GetComponent<SpriteRenderer>();
                    try
                    {
                        sr.sprite = sprites.Find(s => s.name == sr.sprite.name);
                        p.sprite = sprites.Find(s => s.name == sr.sprite.name);
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }
            foreach (Trail t in r.falseTrail)
            {
                foreach (Points p in t.transforms)
                {
                    SpriteRenderer sr = p.transform.GetComponent<SpriteRenderer>();
                    try
                    {
                        sr.sprite = sprites.Find(s => s.name == sr.sprite.name);
                        p.sprite = sprites.Find(s => s.name == sr.sprite.name);
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }
            try
            {
                /*SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                Sprite dotSprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/BackgroundDots.png", typeof(Sprite));
                sr.sprite = dotSprite;
                sr.drawMode = SpriteDrawMode.Tiled;
                sr.size = Vector2.one * 7;
                */
                //DestroyImmediate(r.GetComponent<SpriteRenderer>());
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            string localPath = "Assets/AlternativeResources/Prefabs/Routes/" + go.name + ".prefab";
            /*if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
            {
                if (EditorUtility.DisplayDialog("Are you sure?",
                        "The prefab already exists. Do you want to overwrite it?",
                        "Yes",
                        "No"))
                {
                    CreateNew(go, localPath);
                }
            }
            else*/
            {
                CreateNew(go, localPath);
            }
        }
    }

    // Disable the menu item if no selection is in place
    [MenuItem("Examples/Create Prefab From Selected", true)]
    static bool ValidateCreatePrefab()
    {
        return Selection.activeGameObject != null;
    }

    static void CreateNew(GameObject obj, string localPath)
    {
        Object prefab = PrefabUtility.CreateEmptyPrefab(localPath);
        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }
}