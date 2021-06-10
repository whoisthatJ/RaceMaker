using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public class SceneSelection : EditorWindow
{
	[MenuItem ("Custom Editor/Selection Scenes")]
	public static void ShowWindow ()
	{
		GetWindow<SceneSelection> ("Scene Selection");
	}
	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	void OnGUI ()
	{
		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			var scene = EditorBuildSettings.scenes[i];
			var sceneName = Path.GetFileNameWithoutExtension (scene.path);
			if (GUILayout.Button (i + ": " + sceneName, new GUIStyle (GUI.skin.GetStyle ("Button")) { alignment = TextAnchor.MiddleLeft }, GUILayout.Width (300), GUILayout.Height (20)))
			{
				OpenScene (EditorBuildSettings.scenes[i].path);
			}
		}
	}
	static void OpenScene (string pathScene)
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();
		EditorSceneManager.OpenScene (pathScene);
	}
}