using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    public enum Mode
    {
        ADD,
        EDIT
    }

    public List<Trail> trueTrail = new List<Trail>();
    public List<Trail> falseTrail = new List<Trail>();
    public List<Sprite> sprites = new List<Sprite>();
    public List<SpriteRenderer> obstacles = new List<SpriteRenderer>();
    public List<SpriteRenderer> obstaclePools = new List<SpriteRenderer>();
    public SpriteRenderer background;
    [HideInInspector] public Sprite currentSpriteTrail;
    public Color[,] colors =
    {
       {Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black },
       {Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black },
       {Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black },
       {Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black },
       {Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black },
       {Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black },
       {Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black }
    };
    [HideInInspector]
    public Color[] colorsBtnTrails =
    {
        Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white
    };

    public RouteGridItem[,] items =
    {
        { new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default") },
        { new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default") },
        { new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default") },
        { new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default") },
        { new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default") },
        { new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default") },
        { new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default") },
        { new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default"), new RouteGridItem(Color.black, "0", "default") },
    };

    public Texture2D GetTextureBackground(string name)
    {
        Texture2D tex = sprites.Find(x => x.name == name).texture;
        return tex;
    }
    private int modeType;
    public int ModeType
    {
        set
        {
            modeType = value;
            if (modeType > 1)
            {
                modeType = 0;
                mode = Mode.ADD;
            }
            else
            {
                mode = Mode.EDIT;
            }
        }
        get { return modeType; }
    }
    [HideInInspector] public int selectedSide;
    [HideInInspector] public int selected;
    [HideInInspector] public int numberName;
    [HideInInspector] public int numberNameContainer;
    [HideInInspector] public int numberPieceTrail;
    [HideInInspector] public bool isCreateItem;
    [HideInInspector] public bool isReplaceMode;

    [HideInInspector] public List<string> options = new List<string>();
    [HideInInspector] public List<string> listSelectedSide = new List<string>();
    [HideInInspector] public List<Transform> listUnsavedPoints = new List<Transform>();
    [HideInInspector] public Transform trueTrailContainer;
    [HideInInspector] public Transform falseTrailContainer;
    [HideInInspector] public GameObject trail;

    [HideInInspector] public Mode mode = Mode.ADD;

    public Trail currentTrail;
    public Trail currentFalseTrail;
    public Vector2Int coordinate;
    [HideInInspector] public float speed;
    [HideInInspector] public List<Route> dummies;
    public void Init()
    {
        dummies = new List<Route>();
    }
}
