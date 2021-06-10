using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class CanvasOrientation : EditorWindow 
{    
	[MenuItem("Custom Editor/Horizontal Orientation Canvas")]
	public static void HorizonatalCanvasAllScenes()
	{
		foreach (EditorBuildSettingsScene S in EditorBuildSettings.scenes)
        {

			EditorSceneManager.OpenScene(S.path);
			Object[] obj = GameObject.FindSceneObjectsOfType(typeof(CanvasScaler));
			foreach (object o in obj)
			{
				
				if (o != null)
				{					
					CanvasScaler g = (CanvasScaler)o;
					g.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
					g.referenceResolution = new Vector2(1920, 1080);
					g.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
					g.referencePixelsPerUnit = 100;
					Object foundPrefab = PrefabUtility.GetCorrespondingObjectFromSource((Object)o);
                    if (g != null && PrefabUtility.GetPrefabObject(g.gameObject) != null)
                    {
                        GameObject gameObject = PrefabUtility.FindValidUploadPrefabInstanceRoot(g.gameObject);
                        PrefabUtility.ReplacePrefab(gameObject, foundPrefab, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
                    }
				}

			}
			EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
	}

	[MenuItem("Custom Editor/Vertical Orientation Canvas")]
	public static void VerticalCanvasAllScenes()
	{
		foreach (EditorBuildSettingsScene S in EditorBuildSettings.scenes)
        {
            EditorSceneManager.OpenScene(S.path);
            Object[] obj = GameObject.FindSceneObjectsOfType(typeof(CanvasScaler));
            foreach (object o in obj)
            {
                if (o != null)
                {
                    CanvasScaler g = (CanvasScaler)o;
                    g.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    g.referenceResolution = new Vector2(1080, 1920);
					g.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
					g.matchWidthOrHeight = 0;
                    g.referencePixelsPerUnit = 100;
					Object foundPrefab = PrefabUtility.GetCorrespondingObjectFromSource((Object)o);
                    if (g != null && PrefabUtility.GetPrefabObject(g.gameObject) != null)
                    {
                        GameObject gameObject = PrefabUtility.FindValidUploadPrefabInstanceRoot(g.gameObject);
						PrefabUtility.ReplacePrefab(gameObject, foundPrefab, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
                    }
                }

            }
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
	}
}
