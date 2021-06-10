using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Text.RegularExpressions;
[CustomEditor(typeof(Route))]
public class RouteEditor : Editor
{
    private bool isClick = true;
    private Route myTarget;
    private Vector2[,] pointsGrid =
    {
        {new Vector2(-3, 3), new Vector2(-2, 3), new Vector2(-1, 3), new Vector2(0, 3), new Vector2(1, 3), new Vector2(2, 3), new Vector2(3, 3)},
        {new Vector2(-3, 2), new Vector2(-2, 2), new Vector2(-1, 2), new Vector2(0, 2), new Vector2(1, 2), new Vector2(2, 2), new Vector2(3, 2)},
        {new Vector2(-3, 1), new Vector2(-2, 1), new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1), new Vector2(3, 1)},
        {new Vector2(-3, 0), new Vector2(-2, 0), new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0)},
        {new Vector2(-3, -1), new Vector2(-2, -1), new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1), new Vector2(2, -1), new Vector2(3, -1)},
        {new Vector2(-3, -2), new Vector2(-2, -2), new Vector2(-1, -2), new Vector2(0, -2), new Vector2(1, -2), new Vector2(2, -2), new Vector2(3, -2)},
        {new Vector2(-3, -3), new Vector2(-2, -3), new Vector2(-1, -3), new Vector2(0, -3), new Vector2(1, -3), new Vector2(2, -3), new Vector2(3, -3)}
    };
    private void OnEnable()
    {
        if (myTarget != null)
        {
            myTarget.listSelectedSide.Add("Right");
            myTarget.listSelectedSide.Add("Top");
            myTarget.listSelectedSide.Add("Left");
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        myTarget = (Route)target;
        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        GUILayout.Space(42);
        for (int i = 0; i < 7; i++)
        {
            GUILayout.Label(i.ToString(), GUILayout.Width(60), GUILayout.Height(30));
        }
        GUILayout.EndHorizontal();
        for (int i = 0; i < 7; i++)
        {
            GUILayout.Label(i.ToString());
            GUILayout.Space(-27);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            for (int j = 0; j < 7; j++)
            {
                var style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = myTarget.items[i, j].color;
                style.normal.background = myTarget.GetTextureBackground(myTarget.items[i, j].nameSpriteRoute);
                if (GUILayout.Button(myTarget.items[i, j].name, style, GUILayout.Width(60), GUILayout.Height(60)) == isClick)
                {
                    if (myTarget.mode == Route.Mode.ADD)
                    {
                        if (!myTarget.isReplaceMode)
                        {
                            myTarget.numberPieceTrail++;
                            if (myTarget.items[i, j].name == "0")
                            {
                                myTarget.items[i, j].color = Color.red;
                                myTarget.items[i, j].nameSpriteRoute = myTarget.currentSpriteTrail.name;
                                myTarget.items[i, j].name = myTarget.numberPieceTrail.ToString();
                            }
                            else
                            {
                                myTarget.items[i, j].color = Color.green;
                                myTarget.items[i, j].nameSpriteRoute = myTarget.currentSpriteTrail.name;
                                myTarget.items[i, j].name = myTarget.items[i, j].name + "/" + myTarget.numberPieceTrail;
                            }
                            Transform tr = Instantiate(myTarget.trail.transform, Vector2.zero, Quaternion.identity) as Transform;
                            tr.GetComponent<SpriteRenderer>().sprite = myTarget.currentSpriteTrail;
                            tr.SetParent(myTarget.transform);
                            tr.localPosition = pointsGrid[i, j];
                            tr.name = "Point";
                            myTarget.listUnsavedPoints.Add(tr);
                            myTarget.isCreateItem = true;
                            for (int k = 0; k < myTarget.listUnsavedPoints.Count - 1; k++)
                            {
                                if (myTarget.listUnsavedPoints[k].localPosition.Equals(pointsGrid[i, j]))
                                {
                                    myTarget.listUnsavedPoints[k].GetComponent<SpriteRenderer>().sprite = null;
                                }
                            }
                        }
                        else
                        {
                            int index = myTarget.items[i, j].name.LastIndexOf('/');
                            if (index > -1)
                            {
                                string name = myTarget.items[i, j].name.Substring(index + 1);
                                int indexEdit = int.Parse(name);
                                SpriteRenderer sp = myTarget.listUnsavedPoints[indexEdit - 1].GetComponent<SpriteRenderer>();
                                sp.sprite = myTarget.currentSpriteTrail;
                            }
                            else
                            {
                                int indexEdit = int.Parse(myTarget.items[i, j].name);
                                SpriteRenderer sp = myTarget.listUnsavedPoints[indexEdit - 1].GetComponent<SpriteRenderer>();
                                sp.sprite = myTarget.currentSpriteTrail;
                            }
                        }
                    }
                    else if (myTarget.mode == Route.Mode.EDIT)
                    {
                        if (!myTarget.isReplaceMode)
                        {
                            myTarget.numberPieceTrail++;
                            if (myTarget.items[i, j].name == "0")
                            {
                                myTarget.items[i, j].color = Color.red;
                                myTarget.items[i, j].name = myTarget.numberPieceTrail.ToString();
                                myTarget.items[i, j].nameSpriteRoute = myTarget.currentSpriteTrail.name;
                            }
                            else
                            {
                                myTarget.items[i, j].color = Color.green;
                                myTarget.items[i, j].name = myTarget.items[i, j].name + "/" + myTarget.numberPieceTrail;
                                myTarget.items[i, j].nameSpriteRoute = myTarget.currentSpriteTrail.name;
                            }
                            string namePreviewTrail = myTarget.options[myTarget.selected];
                            Trail trail = null;
                            Transform previewTrail = null;
                            Transform tr = Instantiate(myTarget.trail.transform, Vector2.zero, Quaternion.identity) as Transform;
                            tr.GetComponent<SpriteRenderer>().sprite = myTarget.currentSpriteTrail;
                            trail = myTarget.trueTrail.Find(x => x.name == namePreviewTrail);
                            if (trail != null)
                            {
                                previewTrail = myTarget.trueTrailContainer.FindTransformInactive(trail.name);
                                tr.SetParent(previewTrail);
                                Points points = new Points();
                                points.transform = tr;
                                points.sprite = tr.GetComponent<SpriteRenderer>().sprite;
                                trail.transforms.Add(points);
                                tr.localPosition = pointsGrid[i, j];
                                tr.name = "Point";
                                for (int k = 0; k < previewTrail.childCount - 1; k++)
                                {
                                    if (previewTrail.GetChild(k).localPosition.Equals(pointsGrid[i, j]))
                                    {
                                        previewTrail.GetChild(k).GetComponent<SpriteRenderer>().sprite = null;
                                    }
                                }
                            }

                            trail = myTarget.falseTrail.Find(x => x.name == namePreviewTrail);
                            if (trail != null)
                            {
                                previewTrail = myTarget.falseTrailContainer.FindTransformInactive(trail.name);
                                tr.SetParent(previewTrail);
                                Points points = new Points();
                                points.transform = tr;
                                points.sprite = tr.GetComponent<SpriteRenderer>().sprite;
                                trail.transforms.Add(points);
                                tr.localPosition = pointsGrid[i, j];
                                tr.name = "Point";
                                for (int k = 0; k < previewTrail.childCount - 1; k++)
                                {
                                    if (previewTrail.GetChild(k).localPosition.Equals(pointsGrid[i, j]))
                                    {
                                        previewTrail.GetChild(k).GetComponent<SpriteRenderer>().sprite = null;
                                    }
                                }
                            }
                            myTarget.isCreateItem = true;
                        }
                        else
                        {
                            string namePreviewTrail = myTarget.options[myTarget.selected];
                            Trail trail = null;
                            Transform previewTrail = null;
                            trail = myTarget.trueTrail.Find(x => x.name == namePreviewTrail);
                            if (trail != null)
                            {
                                previewTrail = myTarget.trueTrailContainer.FindTransformInactive(trail.name);
                                int index = myTarget.items[i, j].name.LastIndexOf('/');
                                if (index > -1)
                                {
                                    string name = myTarget.items[i, j].name.Substring(index + 1);
                                    int indexEdit = int.Parse(name);
                                    SpriteRenderer sp = previewTrail.GetChild(indexEdit - 1).GetComponent<SpriteRenderer>();
                                    sp.sprite = myTarget.currentSpriteTrail;
                                    trail.transforms[indexEdit - 1].sprite = myTarget.currentSpriteTrail;
                                }
                                else
                                {
                                    int indexEdit = int.Parse(myTarget.items[i, j].name);
                                    SpriteRenderer sp = previewTrail.GetChild(indexEdit - 1).GetComponent<SpriteRenderer>();
                                    sp.sprite = myTarget.currentSpriteTrail;
                                    trail.transforms[indexEdit - 1].sprite = myTarget.currentSpriteTrail;
                                }
                            }
                            trail = myTarget.falseTrail.Find(x => x.name == namePreviewTrail);
                            if (trail != null)
                            {
                                previewTrail = myTarget.falseTrailContainer.FindTransformInactive(trail.name);
                                int index = myTarget.items[i, j].name.LastIndexOf('/');
                                if (index > -1)
                                {
                                    string name = myTarget.items[i, j].name.Substring(index + 1);
                                    int indexEdit = int.Parse(name);
                                    SpriteRenderer sp = previewTrail.GetChild(indexEdit - 1).GetComponent<SpriteRenderer>();
                                    sp.sprite = myTarget.currentSpriteTrail;
                                    trail.transforms[indexEdit - 1].sprite = myTarget.currentSpriteTrail;
                                }
                                else
                                {
                                    int indexEdit = int.Parse(myTarget.items[i, j].name);
                                    SpriteRenderer sp = previewTrail.GetChild(indexEdit - 1).GetComponent<SpriteRenderer>();
                                    sp.sprite = myTarget.currentSpriteTrail;
                                    trail.transforms[indexEdit - 1].sprite = myTarget.currentSpriteTrail;
                                }
                            }
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("MODE: " + myTarget.mode.ToString(), GUILayout.Width(203), GUILayout.Height(30));
        SwitchModeType();
        GUILayout.EndHorizontal();
        myTarget.selectedSide = EditorGUILayout.Popup("Selected Side", myTarget.selectedSide, myTarget.listSelectedSide.ToArray());
        myTarget.selected = EditorGUILayout.Popup("Name", myTarget.selected, myTarget.options.ToArray());
        if (myTarget.mode == Route.Mode.ADD)
        {
            GUILayout.BeginHorizontal();
            SaveTrueTrail();
            SaveFalseTrail();
            GUILayout.EndHorizontal();
        }
        else if (myTarget.mode == Route.Mode.EDIT)
        {
            if (myTarget.options.Count > 0)
            {
                string namePreviewTrail = myTarget.options[myTarget.selected];
                Trail trail = null;
                trail = myTarget.trueTrail.Find(x => x.name == namePreviewTrail);
                if (trail != null) GUILayout.Label("Current Side: " + GetSideTrail(trail.side), GUILayout.Width(150), GUILayout.Height(30));
                trail = myTarget.falseTrail.Find(x => x.name == namePreviewTrail);
                if (trail != null) GUILayout.Label("Current Side: " + GetSideTrail(trail.side), GUILayout.Width(150), GUILayout.Height(30));
                GUILayout.BeginHorizontal();
                SaveModify();
                SwitchTypeTrail();
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (myTarget.options.Count > 0)
        {
            PreviewGridClick();
            ReplaceModeClick();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (myTarget.isCreateItem)
            UndoClick();
        ClearGridClick();
        if (myTarget.options.Count > 0)
        {
            DestoyTrailClick();
        }
        ClearAllData();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        SetImage();
        SetObstacles();
    }

    private void SetImage()
    {
        ImageButtons(0, 4);
        ImageButtons(7, 11);
        ImageButtons(5, 7);
        ImageButtons(4, 5);
    }

    private void ImageButtons(int current, int count)
    {
        GUILayout.BeginHorizontal();
        for (int i = current; i < count; i++)
        {
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = myTarget.colorsBtnTrails[i];
            if (GUILayout.Button(myTarget.sprites[i].texture, GUILayout.Width(40), GUILayout.Height(40)) == isClick)
            {
                for (int j = 0; j < myTarget.colorsBtnTrails.Length; j++)
                {
                    myTarget.colorsBtnTrails[j] = Color.white;
                }
                myTarget.colorsBtnTrails[i] = Color.red;
                myTarget.currentSpriteTrail = myTarget.sprites[i];
            }
            GUILayout.Space(5);
        }
        GUILayout.EndHorizontal();
    }

    private void ReplaceModeClick()
    {
        var style = new GUIStyle(GUI.skin.button);
        if (myTarget.isReplaceMode)
            style.normal.textColor = Color.green;
        else style.normal.textColor = new Color(.6235294f, .6235294f, .6235294f, 1);
        if (GUILayout.Button("Replace Mode", style, GUILayout.Width(100), GUILayout.Height(30)))
        {
            myTarget.isReplaceMode = !myTarget.isReplaceMode;
        }
    }

    private void SwitchModeType()
    {
        if (GUILayout.Button("Switch Mode Type", GUILayout.Width(120), GUILayout.Height(17)))
        {
            if (myTarget.options.Count > 0)
                myTarget.ModeType++;
            if (myTarget.mode == Route.Mode.EDIT)
            {
                PreviewGrid();
            }
            ClearGrid();
        }
    }

    private void PreviewGridClick()
    {
        if (GUILayout.Button("Preview Grid", GUILayout.Width(100), GUILayout.Height(30)))
        {
            PreviewGrid();
            myTarget.mode = Route.Mode.EDIT;
            myTarget.isCreateItem = true;
        }
    }

    private void PreviewGrid()
    {
        ClearGrid();
        string namePreviewTrail = myTarget.options[myTarget.selected];
        for (int i = 0; i < myTarget.trueTrailContainer.childCount; i++)
        {
            myTarget.trueTrailContainer.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < myTarget.falseTrailContainer.childCount; i++)
        {
            myTarget.falseTrailContainer.GetChild(i).gameObject.SetActive(false);
        }
        Trail trail = null;
        Transform previewTrail = null;
        trail = myTarget.trueTrail.Find(x => x.name == namePreviewTrail);
        if (trail != null)
        {
            previewTrail = myTarget.trueTrailContainer.FindTransformInactive(trail.name);
            previewTrail.gameObject.SetActive(true);
        }
        trail = myTarget.falseTrail.Find(x => x.name == namePreviewTrail);
        if (trail != null)
        {
            previewTrail = myTarget.falseTrailContainer.FindTransformInactive(trail.name);
            previewTrail.gameObject.SetActive(true);
        }

        for (int k = 0; k < previewTrail.childCount; k++)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (previewTrail.GetChild(k).transform.position.Equals(pointsGrid[i, j]))
                    {
                        myTarget.numberPieceTrail++;
                        if (myTarget.items[i, j].name == "0")
                        {
                            myTarget.items[i, j].color = Color.red;
                            myTarget.items[i, j].name = myTarget.numberPieceTrail.ToString();
                            if (previewTrail.GetChild(k).GetComponent<SpriteRenderer>() != null && previewTrail.GetChild(k).GetComponent<SpriteRenderer>().sprite != null)
                                myTarget.items[i, j].nameSpriteRoute = previewTrail.GetChild(k).GetComponent<SpriteRenderer>().sprite.name;
                        }
                        else
                        {
                            myTarget.items[i, j].color = Color.green;
                            myTarget.items[i, j].name = myTarget.items[i, j].name + "/" + myTarget.numberPieceTrail;
                            if (previewTrail.GetChild(k).GetComponent<SpriteRenderer>() != null && previewTrail.GetChild(k).GetComponent<SpriteRenderer>().sprite != null)
                                myTarget.items[i, j].nameSpriteRoute = previewTrail.GetChild(k).GetComponent<SpriteRenderer>().sprite.name;
                        }
                    }
                }
            }
        }
    }

    private void SaveTrueTrail()
    {
        if (GUILayout.Button("Save True Trail", GUILayout.Width(100), GUILayout.Height(30)) && myTarget.isCreateItem)
        {
            myTarget.numberNameContainer = 0;
            FindFreeNameContainerTrail();
            myTarget.options.Add("True Trail " + myTarget.numberNameContainer);
            int transformCount = myTarget.transform.childCount;
            Trail trail = new Trail();
            GameObject container = new GameObject("True Trail " + (myTarget.numberNameContainer));
            container.transform.SetParent(myTarget.trueTrailContainer);
            trail.name = "True Trail " + (myTarget.numberNameContainer);
            trail.side = myTarget.selectedSide;
            container.transform.SetSiblingIndex(myTarget.numberNameContainer);
            for (int i = 0; i < transformCount; i++)
            {
                if (myTarget.transform.GetChild(transformCount - i - 1).CompareTag("TrailPoint"))
                {
                    Transform child = myTarget.transform.GetChild(transformCount - i - 1);
                    Points points = new Points();
                    points.transform = child.transform;
                    points.sprite = child.GetComponent<SpriteRenderer>().sprite;
                    trail.transforms.Add(points);
                    child.name = "Point" + myTarget.numberName;
                    myTarget.numberName++;
                    child.SetParent(container.transform);
                    child.SetAsFirstSibling();
                }
            }
            trail.transforms.Reverse();
            myTarget.trueTrail.Add(trail);
            container.gameObject.SetActive(false);
            ClearGrid();
            myTarget.numberPieceTrail = 0;
            myTarget.isCreateItem = false;
        }
    }
    private void SaveFalseTrail()
    {
        if (GUILayout.Button("Save False Trail", GUILayout.Width(100), GUILayout.Height(30)) && myTarget.isCreateItem)
        {
            myTarget.numberNameContainer = 0;
            FindFreeNameContainerTrail();
            myTarget.options.Add("False Trail " + myTarget.numberNameContainer);
            int transformCount = myTarget.transform.childCount;
            Trail trail = new Trail();
            GameObject container = new GameObject("False Trail " + (myTarget.numberNameContainer));
            trail.name = "False Trail " + (myTarget.numberNameContainer);
            trail.side = myTarget.selectedSide;
            container.transform.SetParent(myTarget.falseTrailContainer);
            container.transform.SetSiblingIndex(myTarget.numberNameContainer);
            for (int i = 0; i < transformCount; i++)
            {
                if (myTarget.transform.GetChild(transformCount - i - 1).CompareTag("TrailPoint"))
                {
                    Transform child = myTarget.transform.GetChild(transformCount - i - 1);
                    Points points = new Points();
                    points.transform = child.transform;
                    points.sprite = child.GetComponent<SpriteRenderer>().sprite;
                    trail.transforms.Add(points);
                    child.name = "Point" + myTarget.numberName;
                    myTarget.numberName++;
                    child.SetParent(container.transform);
                    child.SetAsFirstSibling();
                }
            }
            trail.transforms.Reverse();
            myTarget.falseTrail.Add(trail);
            container.gameObject.SetActive(false);
            ClearGrid();
            myTarget.numberPieceTrail = 0;
            myTarget.isCreateItem = false;
        }
    }

    private void SaveModify()
    {
        if (GUILayout.Button("Save Changes", GUILayout.Width(100), GUILayout.Height(30)))
        {
            string namePreviewTrail = myTarget.options[myTarget.selected];
            Trail trail = myTarget.trueTrail.Find(x => x.name == namePreviewTrail);
            if (trail != null)
            {
                trail.side = myTarget.selectedSide;
            }
            trail = myTarget.falseTrail.Find(x => x.name == namePreviewTrail);
            if (trail != null)
            {
                trail.side = myTarget.selectedSide;
            }
        }
    }

    private void SwitchTypeTrail()
    {
        if (GUILayout.Button("Switch Type Trail", GUILayout.Width(100), GUILayout.Height(30)))
        {
            string namePreviewTrail = myTarget.options[myTarget.selected];
            if (namePreviewTrail.Contains("True"))
            {
                int number = 0;
                Trail trail = myTarget.trueTrail.Find(x => x.name == namePreviewTrail);
                Trail falseTrail = myTarget.falseTrail.Find(x => x.name == "False Trail " + number);
                while (falseTrail != null)
                {
                    number++;
                    falseTrail = myTarget.falseTrail.Find(x => x.name == "False Trail " + number);
                }
                GameObject trailObj = myTarget.trueTrailContainer.FindTransformInactive(namePreviewTrail).gameObject;
                trailObj.transform.SetParent(myTarget.falseTrailContainer);
                trailObj.name = "False Trail " + number;
                trail.name = "False Trail " + number;
                myTarget.options.Remove(namePreviewTrail);
                myTarget.options.Add("False Trail " + number);
                myTarget.falseTrail.Add(trail);
                myTarget.trueTrail.Remove(trail);
            }
            else
            {
                int number = 0;
                Trail trail = myTarget.falseTrail.Find(x => x.name == namePreviewTrail);
                Trail trueTrail = myTarget.trueTrail.Find(x => x.name == "True Trail " + number);
                while (trueTrail != null)
                {
                    number++;
                    trueTrail = myTarget.trueTrail.Find(x => x.name == "True Trail " + number);
                }
                GameObject trailObj = myTarget.falseTrailContainer.FindTransformInactive(namePreviewTrail).gameObject;
                trailObj.transform.SetParent(myTarget.trueTrailContainer);
                trailObj.name = "True Trail " + number;
                trail.name = "True Trail " + number;
                myTarget.options.Remove(namePreviewTrail);
                myTarget.options.Add("True Trail " + number);
                myTarget.trueTrail.Add(trail);
                myTarget.falseTrail.Remove(trail);
            }
        }
    }

    private void ClearGrid()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                myTarget.items[i, j].color = Color.black;
                myTarget.items[i, j].name = "0";
                myTarget.items[i, j].nameSpriteRoute = "default";
            }
        }
        myTarget.numberName = 0;
        int transformCount = myTarget.transform.childCount;

        for (int i = 0; i < transformCount; i++)
        {
            if (myTarget.transform.GetChild(transformCount - i - 1).CompareTag("TrailPoint"))
            {
                DestroyImmediate(myTarget.transform.GetChild(transformCount - i - 1).gameObject, true);
            }
        }
        for (int i = 0; i < myTarget.trueTrailContainer.childCount; i++)
        {
            myTarget.trueTrailContainer.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < myTarget.falseTrailContainer.childCount; i++)
        {
            myTarget.falseTrailContainer.GetChild(i).gameObject.SetActive(false);
        }
        myTarget.listUnsavedPoints.Clear();
        myTarget.numberPieceTrail = 0;
    }
    private void UndoClick()
    {
        if (GUILayout.Button("Undo Last Point", GUILayout.Width(100), GUILayout.Height(30)))
        {
            if (myTarget.mode == Route.Mode.ADD)
            {
                if (myTarget.listUnsavedPoints.Count > 0)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (myTarget.listUnsavedPoints[myTarget.listUnsavedPoints.Count - 1].position.Equals(pointsGrid[i, j]))
                            {
                                if (myTarget.items[i, j].name == myTarget.numberPieceTrail.ToString())
                                {
                                    myTarget.items[i, j].color = Color.black;
                                    myTarget.items[i, j].name = "0";
                                    myTarget.items[i, j].nameSpriteRoute = "default";
                                }
                                else
                                {
                                    int index = myTarget.items[i, j].name.LastIndexOf('/');
                                    if (index > -1)
                                    {
                                        myTarget.items[i, j].name = myTarget.items[i, j].name.Substring(0, index);
                                        index = myTarget.items[i, j].name.LastIndexOf('/');
                                        if (index > -1) myTarget.items[i, j].color = Color.green;
                                        else myTarget.items[i, j].color = Color.red;
                                    }
                                    else
                                    {
                                        myTarget.items[i, j].color = Color.black;
                                        myTarget.items[i, j].name = "0";
                                        myTarget.items[i, j].nameSpriteRoute = "default";
                                    }
                                }
                                myTarget.numberPieceTrail--;
                            }
                        }
                    }
                    DestroyImmediate(myTarget.listUnsavedPoints[myTarget.listUnsavedPoints.Count - 1].gameObject, true);
                    myTarget.listUnsavedPoints.RemoveAt(myTarget.listUnsavedPoints.Count - 1);
                }
                else
                {
                    myTarget.isCreateItem = false;
                }
            }
            else if (myTarget.mode == Route.Mode.EDIT)
            {
                string namePreviewTrail = myTarget.options[myTarget.selected];
                Trail trail = null;
                Transform previewTrail = null;
                trail = myTarget.trueTrail.Find(x => x.name == namePreviewTrail);
                if (trail != null)
                {
                    previewTrail = myTarget.trueTrailContainer.FindTransformInactive(trail.name);

                    if (previewTrail != null)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            for (int j = 0; j < 7; j++)
                            {
                                if (previewTrail.GetChild(previewTrail.transform.childCount - 1).position.Equals(pointsGrid[i, j]))
                                {
                                    if (myTarget.items[i, j].name == myTarget.numberPieceTrail.ToString())
                                    {
                                        myTarget.items[i, j].color = Color.black;
                                        myTarget.items[i, j].name = "0";
                                        myTarget.items[i, j].nameSpriteRoute = "default";
                                    }
                                    else
                                    {
                                        int index = myTarget.items[i, j].name.LastIndexOf('/');
                                        if (index > -1)
                                        {
                                            myTarget.items[i, j].name = myTarget.items[i, j].name.Substring(0, index);
                                            index = myTarget.items[i, j].name.LastIndexOf('/');
                                            if (index > -1) myTarget.items[i, j].color = Color.green;
                                            else myTarget.items[i, j].color = Color.red;
                                        }
                                        else
                                        {
                                            myTarget.items[i, j].color = Color.black;
                                            myTarget.items[i, j].name = "0";
                                            myTarget.items[i, j].nameSpriteRoute = "default";
                                        }
                                    }
                                    myTarget.numberPieceTrail--;
                                }
                            }
                        }
                        trail.transforms.RemoveAt(trail.transforms.Count - 1);
                        if (previewTrail.childCount > 0)
                            DestroyImmediate(previewTrail.GetChild(previewTrail.transform.childCount - 1).gameObject, true);
                        if (previewTrail.childCount == 0)
                        {
                            DestroyImmediate(previewTrail.gameObject, true);
                            myTarget.trueTrail.Remove(trail);
                            myTarget.options.RemoveAt(myTarget.selected);
                            myTarget.isCreateItem = false;
                            myTarget.mode = Route.Mode.ADD;
                            ClearGrid();
                            myTarget.numberPieceTrail = 0;
                        }
                    }
                }
                trail = myTarget.falseTrail.Find(x => x.name == namePreviewTrail);
                if (trail != null)
                {
                    previewTrail = myTarget.falseTrailContainer.FindTransformInactive(trail.name);
                    if (previewTrail != null)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            for (int j = 0; j < 7; j++)
                            {
                                if (previewTrail.GetChild(previewTrail.transform.childCount - 1).position.Equals(pointsGrid[i, j]))
                                {
                                    if (myTarget.items[i, j].name == myTarget.numberPieceTrail.ToString())
                                    {
                                        myTarget.items[i, j].color = Color.black;
                                        myTarget.items[i, j].name = "0";
                                        myTarget.items[i, j].nameSpriteRoute = "default";
                                    }
                                    else
                                    {
                                        int index = myTarget.items[i, j].name.LastIndexOf('/');
                                        if (index > -1)
                                        {
                                            myTarget.items[i, j].name = myTarget.items[i, j].name.Substring(0, index);
                                            index = myTarget.items[i, j].name.LastIndexOf('/');
                                            if (index > -1) myTarget.items[i, j].color = Color.green;
                                            else myTarget.items[i, j].color = Color.red;
                                        }
                                        else
                                        {
                                            myTarget.items[i, j].color = Color.black;
                                            myTarget.items[i, j].name = "0";
                                            myTarget.items[i, j].nameSpriteRoute = "default";
                                        }
                                    }
                                    myTarget.numberPieceTrail--;
                                }
                            }
                        }
                        trail.transforms.RemoveAt(trail.transforms.Count - 1);
                        if (previewTrail.childCount > 0)
                            DestroyImmediate(previewTrail.GetChild(previewTrail.transform.childCount - 1).gameObject, true);
                        if (previewTrail.childCount == 0)
                        {
                            DestroyImmediate(previewTrail.gameObject, true);
                            myTarget.falseTrail.Remove(trail);
                            myTarget.options.RemoveAt(myTarget.selected);
                            myTarget.isCreateItem = false;
                            myTarget.mode = Route.Mode.ADD;
                            ClearGrid();
                            myTarget.numberPieceTrail = 0;
                        }
                    }
                }
            }
        }
    }
    private void ClearGridClick()
    {
        if (GUILayout.Button("Clear Grid", GUILayout.Width(100), GUILayout.Height(30)))
        {
            myTarget.mode = Route.Mode.ADD;
            ClearGrid();
            myTarget.numberPieceTrail = 0;
            myTarget.isCreateItem = false;
        }
    }

    private void ClearAllData()
    {
        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.red;
        if (GUILayout.Button("Clear Data", style, GUILayout.Width(100), GUILayout.Height(30)))
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    myTarget.items[i, j].color = Color.black;
                    myTarget.items[i, j].name = "0";
                    myTarget.items[i, j].nameSpriteRoute = "default";
                }
            }
            int transformCount = myTarget.transform.childCount;
            for (int i = 0; i < transformCount; i++)
            {
                if (myTarget.transform.GetChild(transformCount - i - 1).CompareTag("TrailPoint"))
                {
                    DestroyImmediate(myTarget.transform.GetChild(transformCount - i - 1).gameObject, true);
                }
            }
            int countChild = myTarget.trueTrailContainer.childCount;
            for (int i = 0; i < countChild; i++)
            {
                DestroyImmediate(myTarget.trueTrailContainer.GetChild(countChild - i - 1).gameObject);
            }
            countChild = myTarget.falseTrailContainer.childCount;
            for (int i = 0; i < countChild; i++)
            {
                DestroyImmediate(myTarget.falseTrailContainer.GetChild(countChild - i - 1).gameObject);
            }
            myTarget.numberName = 0;
            myTarget.numberNameContainer = 0;
            myTarget.selected = 0;
            myTarget.selectedSide = 0;
            myTarget.numberPieceTrail = 0;
            myTarget.options.Clear();
            myTarget.trueTrail.Clear();
            myTarget.falseTrail.Clear();
            myTarget.listUnsavedPoints.Clear();
            myTarget.mode = Route.Mode.ADD;
            myTarget.isCreateItem = false;
            myTarget.isReplaceMode = false;
        }
    }

    private void DestoyTrailClick()
    {
        if (GUILayout.Button("Destroy Trail", GUILayout.Width(100), GUILayout.Height(30)))
        {
            if (myTarget.options.Count > 0)
            {
                ClearGrid();
                string namePreviewTrail = myTarget.options[myTarget.selected];
                Trail trail = null;
                trail = myTarget.trueTrail.Find(x => x.name == namePreviewTrail);
                if (trail != null)
                {
                    Transform previewTrail = myTarget.trueTrailContainer.FindTransformInactive(trail.name);
                    DestroyImmediate(previewTrail.gameObject, true);
                    myTarget.trueTrail.Remove(trail);
                }
                trail = myTarget.falseTrail.Find(x => x.name == namePreviewTrail);
                if (trail != null)
                {
                    Transform previewTrail = myTarget.falseTrailContainer.FindTransformInactive(trail.name);
                    DestroyImmediate(previewTrail.gameObject, true);
                    myTarget.falseTrail.Remove(trail);
                }
                myTarget.options.RemoveAt(myTarget.selected);
                if (myTarget.options.Count == 0)
                {
                    myTarget.mode = Route.Mode.ADD;
                }
                myTarget.selected = 0;
            }
            else
            {
                myTarget.isCreateItem = false;
                myTarget.mode = Route.Mode.ADD;
            }
        }
    }

    private void FindFreeNameContainerTrail()
    {
        Transform tr = null;
        tr = myTarget.trueTrailContainer.FindTransformInactive("True Trail " + myTarget.numberNameContainer);
        if (tr != null)
        {
            myTarget.numberNameContainer++;
            FindFreeNameContainerTrail();
        }
        tr = myTarget.falseTrailContainer.FindTransformInactive("False Trail " + myTarget.numberNameContainer);
        if (tr != null)
        {
            myTarget.numberNameContainer++;
            FindFreeNameContainerTrail();
        }
    }
    private void SetObstacles()
    {
        var style = new GUIStyle(GUI.skin.button);
        if (myTarget.isReplaceMode)
            style.normal.textColor = Color.green;
        else style.normal.textColor = new Color(.235294f, .6235294f, .235294f, 1);
        if (GUILayout.Button("Set Obstacles", style, GUILayout.Width(100), GUILayout.Height(30)))
        {
            Collider2D[] obstacles = myTarget.GetComponentsInChildren<Collider2D>();
            myTarget.obstacles = new List<SpriteRenderer>();
            foreach (Collider2D c in obstacles)
            {
                myTarget.obstacles.Add(c.gameObject.GetComponent<SpriteRenderer>());
            }
            myTarget.background = myTarget.GetComponent<SpriteRenderer>();
            {
                if (myTarget.transform.Find("ObstacleParent") == null)
                {
                    GameObject go = new GameObject();
                    go.name = "ObstacleParent";
                    go.transform.parent = myTarget.transform;
                    go.transform.localPosition = Vector3.zero;
                    foreach (SpriteRenderer sr in myTarget.obstacles)
                    {
                        sr.transform.parent = go.transform;
                    }
                }
            }
        }

    }
    private string GetSideTrail(int index)
    {
        string t = string.Empty;
        switch (index)
        {
            case 0:
                return t = "Right";
            case 156:
                return t = "Top";
            case 157:
                return t = "Left";
        }
        return t;
    }
}
