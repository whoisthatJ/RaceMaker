using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DisplayVariablesEditor : EditorWindow
{
    private int countChildVariables;
    private GameObject displayPrefab = null;
    //Instance prefab
    private GameObject cloneDisplayPrefab = null;
    // Reference Content Panel for Scroll
    private GameObject contentPanelVar = null;
    // Names Variables in Display Editor
    private List<string> namesBnts = new List<string>();
    private Vector2 scrollPos;

    [MenuItem("Custom Editor/Phone Variables")]
    public static void ShowWindow()
    {
        DisplayVariablesEditor test = (DisplayVariablesEditor)EditorWindow.GetWindowWithRect(typeof(DisplayVariablesEditor), new Rect(0, 0, 650, 600));
    }
    bool isPresed = true;
    private void OnGUI()
    {
        GUILayout.Label("Display Variables", EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(640), GUILayout.Height(500));
        GUILayout.BeginVertical();
        for (int i = 0; i < countChildVariables; i++)
        {
            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            ModifyTextField(i);
            SaveVariable(i);
            DeleteVariable(i);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
        AddVariable();
        AddVector();
        DeleteAll();
        GUILayout.EndHorizontal();
    }

    private void OnEnable()
    {
        GetPrefabs();
    }

    private void OnDisable()
    {
        PrefabUtility.ReplacePrefab(cloneDisplayPrefab, displayPrefab, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
        DestroyImmediate(GameObject.Find("DisplayVariablesWidget"));
    }

    private void GetPrefabs()
    {
        string[] search_results = System.IO.Directory.GetFiles("Assets/PlayFlock_Utils/HUDDebug/Widgets", "DisplayVariablesWidget.prefab", System.IO.SearchOption.AllDirectories);
        for (int i = 0; i < search_results.Length; i++)
        {
            displayPrefab = AssetDatabase.LoadMainAssetAtPath(search_results[i]) as GameObject;
            cloneDisplayPrefab = PrefabUtility.InstantiatePrefab(displayPrefab) as GameObject;
            cloneDisplayPrefab.hideFlags = HideFlags.HideInHierarchy;
            contentPanelVar = GameObject.FindGameObjectWithTag("CPVariables");
            countChildVariables = contentPanelVar.transform.childCount;
            for (int j = 0; j < countChildVariables; j++)
            {
                namesBnts.Add(contentPanelVar.transform.GetChild(j).GetComponent<Variable>().nameVariable);
            }
        }
    }
    // Add Variable Prefab to Content Panel
    private void AddVariable()
    {
        if (GUILayout.Button("Add Variable", GUILayout.Width(150), GUILayout.Height(40)))
        {
            contentPanelVar = GameObject.FindGameObjectWithTag("CPVariables");
            GameObject variable = displayPrefab.GetComponent<Playflock.Log.Widget.DisplayVariablesWidget>().variable;
            GameObject cloneVariable = PrefabUtility.InstantiatePrefab(variable) as GameObject;
            cloneVariable.transform.parent = contentPanelVar.transform;
            countChildVariables = contentPanelVar.transform.childCount;
            namesBnts.Add(cloneDisplayPrefab.name);
            PrefabUtility.ReplacePrefab(cloneDisplayPrefab, displayPrefab, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
            cloneDisplayPrefab.hideFlags = HideFlags.HideInHierarchy;
        }
    }

    // Add Vector Prefab to Content Panel
    private void AddVector()
    {
        if (GUILayout.Button("Add Vector", GUILayout.Width(150), GUILayout.Height(40)))
        {
            contentPanelVar = GameObject.FindGameObjectWithTag("CPVariables");
            GameObject vector = displayPrefab.GetComponent<Playflock.Log.Widget.DisplayVariablesWidget>().vector;
            GameObject cloneVariable = PrefabUtility.InstantiatePrefab(vector) as GameObject;
            cloneVariable.transform.parent = contentPanelVar.transform;
            countChildVariables = contentPanelVar.transform.childCount;
            namesBnts.Add(cloneDisplayPrefab.name);
            PrefabUtility.ReplacePrefab(cloneDisplayPrefab, displayPrefab, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
            cloneDisplayPrefab.hideFlags = HideFlags.HideInHierarchy;
        }
    }

    private void ModifyTextField(int current)
    {
        namesBnts[current] = EditorGUILayout.TextField(namesBnts[current], GUILayout.Width(300), GUILayout.Height(40));
        if (EditorGUI.EndChangeCheck())
        {
            namesBnts[current] = EditorGUILayout.TextField(namesBnts[current], GUILayout.Width(300), GUILayout.Height(40));
        }
    }

    private void SaveVariable(int current)
    {
        if (isPresed == GUILayout.Button("Save", GUILayout.Width(150), GUILayout.Height(40)))
        {
            contentPanelVar.transform.GetChild(current).GetComponent<Variable>().nameVariable = namesBnts[current];
        }
    }

    private void DeleteVariable(int current)
    {
        if (isPresed == GUILayout.Button("Delete", GUILayout.Width(150), GUILayout.Height(40)))
        {
            GameObject obj = contentPanelVar.transform.GetChild(current).gameObject;
            string name = contentPanelVar.transform.GetChild(current).GetComponent<Variable>().nameVariable;
            Object GO = PrefabUtility.GetCorrespondingObjectFromSource(obj.gameObject);
            if (GO != null)
            {
                countChildVariables--;
                CSPlayerPrefs.DeleteKey(name);
                namesBnts.RemoveAt(current);
                DestroyImmediate(GO, true);
            }
            cloneDisplayPrefab.hideFlags = HideFlags.HideInHierarchy;
        }
    }

    private void DeleteAll()
    {
        if (isPresed == GUILayout.Button("Delete All", GUILayout.Width(150), GUILayout.Height(40)) && countChildVariables > 0)
        {
            for (int i = 0; i < contentPanelVar.transform.childCount; i++)
            {
                GameObject obj = contentPanelVar.transform.GetChild(i).gameObject;
                string name = contentPanelVar.transform.GetChild(i).GetComponent<Variable>().nameVariable;
                Object GO = PrefabUtility.GetCorrespondingObjectFromSource(obj.gameObject);
                CSPlayerPrefs.DeleteKey(name);
                countChildVariables--;
                DestroyImmediate(GO, true);
            }
        }
        cloneDisplayPrefab.hideFlags = HideFlags.HideInHierarchy;
    }
}
