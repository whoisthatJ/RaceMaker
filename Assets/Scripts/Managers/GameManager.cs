using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this) Destroy(gameObject);
    }
    public CarController car;
    public CameraFollow cameraFollow;
    public GameObject firstRoad;
    public GameObject newLevelFlag;
    public GameObject arrows;
    public GameObject highScoreFlag;
    public Route highScoreRoute;
    public GameObject squaresParent;

    public GameObject pathParent;
    public List<Route> squares;
    public List<Route> currentLevelSquares;
    public List<Route> pathSquares = new List<Route>();
    public List<Route> trashSquares = new List<Route>();
    public List<Route> poolSquares;
    public List<Route> tutorialDummies;
    public List<Route> generalDummies;
    public RaceLevel currentLevel;
    public RaceLevel spriteLevel;
    public Button roadRightBtn;
    public Button roadLeftBtn;

    public Image rightWrongImage;
    public Image leftWrongImage;

    public ButtonTrailView rightButtonTrailView;
    public ButtonTrailView leftButtonTrailView;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI comboTxt;
    public TextMeshProUGUI levelUpTxt;
    public TextMeshProUGUI recordTxt;

    public TextMeshProUGUI tutorialTxt;
    public TextMeshProUGUI tutorialWrongTxt;
    public TextMeshProUGUI tutorialEndTxt;
    public TextMeshProUGUI loseTxt;

    public RectTransform tutorialAnimation;
    public Image timeBar;
    public Image timeBarBG;
    public int score;
    public int generatedSquares;
    public int combo;
    public Sprite obstacleSprite;
    public Sprite obstacleShineSprite;
    public Sprite gridSprite;
    public Sprite[] obstacleSprites;
    public Sprite[] obstacleShineSprites;
    public Sprite[] gridSprites;
    Dictionary<string, Sprite> obstacleDict;
    Dictionary<string, Sprite> obstacleShineDict;
    Dictionary<string, Sprite> gridDict;
    int prevHighScore;
    List<string> colors;
    public bool gameStarted;
    List<Tween> tutorialPathTweens;
    Route Z001;
    Route Z000;
    Route Z002;
    private void Start()
    {
        InitData();

        InitSquaresPool();
        InitAudio();
        InitShineObstacles();
        roadRightBtn.onClick.AddListener(RightPressed);
        roadLeftBtn.onClick.AddListener(LeftPressed);
        InitGame();
    }
    public void InitGame()
    {
        ClearPath();
        score = 0;
        generatedSquares = 0;
        combo = 0;
        toTheLeft = 0;
        toTheRight = 0;
        highScoreFlagged = false;
        SetCurrentLevelSquares();
        pathSquares = new List<Route>();
        trashSquares = new List<Route>();
        GeneratePath();
        SetCurrentSprites();
        SetSpritesToWorldInstant();

        cameraFollow.rot = pathSquares[0].transform.eulerAngles.z;
        cameraFollow.SetOffset();
        cameraFollow.SetTarget(pathSquares[0].gameObject, true);
        car.InitCar();
        SetButtonTrailView();
        EnablePathButton(true);
        score = 0;
        scoreTxt.text = score.ToString();
        scoreTxt.gameObject.SetActive(true);
        SetShineObstacles();
        prevHighScore = MainRoot.Instance.userConfig.highScore;
        cameraFollow.SetTarget(pathSquares[0].gameObject, true);
        DOTween.Play("1003");
        gameStarted = false;
        tutorialAnimation.gameObject.SetActive(false);
        tutorialTxt.gameObject.SetActive(false);
        tutorialWrongTxt.gameObject.SetActive(false);
        loseTxt.gameObject.SetActive(false);
    }
    public void ClearPath()
    {
        newLevelFlag.SetActive(false);
        foreach (Route r in pathSquares)
        {
            r.gameObject.SetActive(false);
            poolSquares.Add(r);
            r.coordinate = Vector2Int.zero;
            foreach (Route rd in r.dummies)
            {
                rd.transform.parent = squaresParent.transform;
                rd.gameObject.SetActive(false);
                poolSquares.Add(rd);
                rd.coordinate = Vector2Int.zero;
            }
        }
        foreach (Route r in trashSquares)
        {
            r.gameObject.SetActive(false);
            poolSquares.Add(r);
            r.coordinate = Vector2Int.zero;
            foreach (Route rd in r.dummies)
            {
                rd.transform.parent = squaresParent.transform;
                rd.gameObject.SetActive(false);
                poolSquares.Add(rd);
                rd.coordinate = Vector2Int.zero;
            }
        }
        sides = new List<Vector2Int>();
        sides.Add(Vector2Int.up);
        sides.Add(Vector2Int.left);
        sides.Add(Vector2Int.down);
        sides.Add(Vector2Int.right);
        cameraFollow.rot = 0f;
        cameraFollow.SetOffset();
    }
    public void StartGame()
    {
        car.pointsCount = 7;
        car.distance = 6.5f;
        car.UpdateSpeed();
        lost = false;
        SetTimeBar();
        DOTween.Play("1004");
        gameStarted = true;
        cameraFollow.rot = pathSquares[0].transform.eulerAngles.z;
        cameraFollow.SetOffset();
        cameraFollow.SetTarget(pathSquares[0].gameObject, true);
        highScored = false;
        if (!MainRoot.Instance.userConfig.isTutorialPassed)
        {
            tutorialAnimation.gameObject.SetActive(true);
            pathSquares[0].currentTrail.transforms[0].transform.parent.gameObject.SetActive(true);
            foreach (Tween t in tutorialPathTweens)
            {
                t.Rewind();
                t.Kill();
            }
            tutorialPathTweens = new List<Tween>();
            foreach (Points p in pathSquares[0].currentTrail.transforms)
            {
                tutorialPathTweens.Add(p.spriteRenderer.DOFade(0.6f, 0.6f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).ChangeStartValue(new Color(1, 1, 1, 0)));
            }
            tutorialTxt.gameObject.SetActive(false);
            tutorialTxt.gameObject.SetActive(true);
            
            //DOTween.Play("1101");
            //tutorialTxt.transform.DOPunchScale(Vector3.one * 1.2f, 1f).ChangeStartValue(Vector3.one);
            tutorialTxt.text = "Choose the correct path!";
        }
    }
    private void InitSquaresPool()
    {
        poolSquares = new List<Route>();
        for (int i = 0; i < 100; i++)
        {
            GameObject g = Instantiate(squares.Find(ss => ss.gameObject.name == "RoutePool").gameObject, squaresParent.transform);
            Route s = g.GetComponent<Route>();
            g.transform.localScale = Vector3.one;
            s.currentTrail = s.trueTrail[0];
            s.currentFalseTrail = s.falseTrail[0];
            s.obstaclePools = new List<SpriteRenderer>();
            foreach (Points p in s.currentTrail.transforms)
            {
                s.currentTrail.transformsPool.Add(p);
                p.spriteRenderer = p.transform.GetComponent<SpriteRenderer>();
            }
            foreach (Points p in s.currentFalseTrail.transforms)
            {
                s.currentFalseTrail.transformsPool.Add(p);
                p.spriteRenderer = p.transform.GetComponent<SpriteRenderer>();
            }
            foreach (SpriteRenderer sr in s.obstacles)
            {
                s.obstaclePools.Add(sr);
            }
            poolSquares.Add(s);
            g.SetActive(false);
        }
        squares.Remove(squares.Find(ss => ss.gameObject.name == "RoutePool"));
        tutorialDummies = new List<Route>();
        generalDummies = new List<Route>();
        foreach (string s in ServiceXML.Instance.GetDummies()[0].prefabs)
        {
            Route r = squares.Find(rr => rr.gameObject.name == s);
            if (r != null)
            {
                tutorialDummies.Add(r);
            }
        }
        foreach (string s in ServiceXML.Instance.GetDummies()[1].prefabs)
        {
            Route r = squares.Find(rr => rr.gameObject.name == s);
            if (r != null)
            {
                generalDummies.Add(r);
            }
        }
    }
    List<SpriteRenderer> shineSprites;
    private void InitShineObstacles()
    {
        shineSprites = new List<SpriteRenderer>();
        for (int i = 0; i < 20; i++)
        {
            GameObject g = new GameObject();
            SpriteRenderer s = g.AddComponent<SpriteRenderer>();
            s.sortingLayerName = "GamePlay";
            s.sortingOrder = 1000;
            shineSprites.Add(s);
            s.DOFade(0f, 0.83f).SetEase(Ease.OutCubic).SetLoops(-1, LoopType.Yoyo).SetDelay(0.2f * i);
        }
    }
    private void SetCurrentSprites()
    {
        if (generatedSquares < 20 && PlayerPrefs.GetInt("TutorialXML", 0) == 0)
            spriteLevel = ServiceXML.Instance.GetTutorialRaceLevels().Find(l => l.level > score);
        else
            spriteLevel = ServiceXML.Instance.GetRaceLevels().Find(l => l.level > score);
        /*if (ServiceXML.Instance.GetRaceLevels().IndexOf(spriteLevel) == ServiceXML.Instance.GetRaceLevels().Count - 1)
        {
            spriteLevel.level = -1;
        }*/
        if (spriteLevel == null)
        {
            spriteLevel = currentLevel;
        }
        //Sprite[] sprites = ServiceResources.LoadAll<Sprite>("Sprites/Environment/" + spriteLevel.color + "/");
        if (spriteLevel.level > 0)
        {
            //ServiceGameAnalytics.Instance.LogLevelStart(spriteLevel.level);
            //ServiceFirebaseAnalytics.Instance.LogLevelStart(spriteLevel.level);
        }
        gridSprite = gridSprites[0];
        if (spriteLevel.shape == null || spriteLevel.shape == "square")
        {
            obstacleDict["Blue"] = obstacleSprites[0];
            obstacleDict["Brown"] = obstacleSprites[1];
            obstacleDict["Green"] = obstacleSprites[2];
            obstacleDict["Purple"] = obstacleSprites[3];
            obstacleDict["Red"] = obstacleSprites[4];
            obstacleShineDict["Blue"] = obstacleShineSprites[0];
            obstacleShineDict["Brown"] = obstacleShineSprites[1];
            obstacleShineDict["Green"] = obstacleShineSprites[2];
            obstacleShineDict["Purple"] = obstacleShineSprites[3];
            obstacleShineDict["Red"] = obstacleShineSprites[4];
        }
        else
        {
            obstacleDict["Blue"] = obstacleSprites[5];
            obstacleDict["Brown"] = obstacleSprites[6];
            obstacleDict["Green"] = obstacleSprites[7];
            obstacleDict["Purple"] = obstacleSprites[8];
            obstacleDict["Red"] = obstacleSprites[9];
            obstacleShineDict["Blue"] = obstacleShineSprites[5];
            obstacleShineDict["Brown"] = obstacleShineSprites[6];
            obstacleShineDict["Green"] = obstacleShineSprites[7];
            obstacleShineDict["Purple"] = obstacleShineSprites[8];
            obstacleShineDict["Red"] = obstacleShineSprites[9];
        }
        obstacleSprite = obstacleDict[spriteLevel.color];
        obstacleShineSprite = obstacleShineDict[spriteLevel.color];
    }
    List<SpriteRenderer> toUpdateColor;
    private void SetSpritesToSquare(Route r, bool update = false)
    {
        //if (updatingColors)
          //  return;
        //r.background.sprite = gridSprite;
        foreach (SpriteRenderer s in r.obstacles)
        {
            s.sprite = obstacleSprite;
        }
        foreach (Route dr in r.dummies)
        {
            //dr.background.sprite = gridSprite;
            foreach (SpriteRenderer s in dr.obstacles)
            {
                s.sprite = obstacleSprite;
            }
        }
    }
    private void SetSpritesLevelUp()
    {
        for(int i = 2; i < pathSquares.Count; i++)
        {
            SetSpritesToSquare(pathSquares[i]);
        }        
    }
    private void SetSpritesToWorldInstant()
    {
        foreach (Route r in pathSquares)
        {
            SetSpritesToSquare(r);
        }
        foreach (Route r in trashSquares)
        {
            SetSpritesToSquare(r);
        }
        foreach(SpriteRenderer r in shineSprites)
        {
            r.sprite = obstacleShineSprite;
        }
    }
    private void SetSpritesToWorld()
    {
        toUpdateColor = new List<SpriteRenderer>();

        foreach (Route r in pathSquares)
        {
            //r.background.sprite = gridSprite;
            foreach (SpriteRenderer s in r.obstacles)
            {
                toUpdateColor.Add(s);
            }
            foreach (Route dr in r.dummies)
            {
                //dr.background.sprite = gridSprite;
                foreach (SpriteRenderer s in dr.obstacles)
                {
                    toUpdateColor.Add(s);
                }
            }
        }
        foreach (Route r in trashSquares)
        {
            //r.background.sprite = gridSprite;
            foreach (SpriteRenderer s in r.obstacles)
            {
                toUpdateColor.Add(s);
            }
            foreach (Route dr in r.dummies)
            {
                //dr.background.sprite = gridSprite;
                foreach (SpriteRenderer s in dr.obstacles)
                {
                    toUpdateColor.Add(s);
                }
            }
        }
        //StartCoroutine(UpdateColorSprites());
    }
    bool updatingColors;
    IEnumerator UpdateColorSprites()
    {
        updatingColors = true;
        toUpdateColor = toUpdateColor.OrderBy(s => s.transform.position.y).ToList();
        int minY = (int) Mathf.Round(toUpdateColor[0].transform.position.y);
        int maxY = (int) Mathf.Round(toUpdateColor[toUpdateColor.Count - 1].transform.position.y);
        int sIndex = 0;
        for (int i = minY; i <= maxY; i++)
        {
            while (toUpdateColor.Count > sIndex)
            {
                if (toUpdateColor[sIndex] == null)
                {
                    sIndex++;
                    continue;
                }
                else if ((int)Mathf.Round(toUpdateColor[sIndex].transform.position.y) == i)
                {
                    toUpdateColor[sIndex].sprite = obstacleSprite;
                    //toUpdateColor[sIndex].transform.parent.parent.GetComponent<SpriteRenderer>().sprite = gridSprite;
                    if (toUpdateColor[sIndex].transform.childCount > 0)
                    {
                        toUpdateColor[sIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = obstacleShineSprite;
                    }
                    sIndex++;
                }
                else
                {
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        updatingColors = false;
    }
    bool lastLevel;
    private void SetCurrentLevelSquares()
    {
        RaceLevel nextLevel;
        if(generatedSquares < 20 && PlayerPrefs.GetInt("TutorialXML", 0) == 0)
            nextLevel = ServiceXML.Instance.GetTutorialRaceLevels().Find(l => l.level > generatedSquares);
        else
            nextLevel = ServiceXML.Instance.GetRaceLevels().Find(l => l.level > generatedSquares);
        /*if (ServiceXML.Instance.GetRaceLevels().IndexOf(currentLevel) == ServiceXML.Instance.GetRaceLevels().Count - 1)
        {
            lastLevel = true;
        }*/
        if (nextLevel == null)
        {
            RaceLevel newLevel = new RaceLevel();
            newLevel.level = currentLevel.level + 10;
            newLevel.speed = currentLevel.speed + 0.2f;
            newLevel.prefabs = currentLevel.prefabs;
            string a = colors[0];
            colors.RemoveAt(0);
            colors.Add(a);
            newLevel.color = a;
            currentLevel = newLevel;
        }
        else
        {
            currentLevel = nextLevel;
        }
        currentLevelSquares = new List<Route>();
        foreach (string s in currentLevel.prefabs)
        {
            Route r = squares.Find(l => l.gameObject.name == s);
            r.speed = currentLevel.speed;
            currentLevelSquares.Add(r);
        }
    }
    private void SetRoute(Route newRoute, List<SpriteRenderer> obstacles, Trail trueTrail, Trail falseTrail)
    {
        newRoute.obstacles = new List<SpriteRenderer>();
        for (int i = 0; i < newRoute.obstaclePools.Count; i++)
        {
            if (i < obstacles.Count)
            {
                newRoute.obstaclePools[i].gameObject.SetActive(true);
                newRoute.obstaclePools[i].transform.localPosition = obstacles[i].transform.localPosition;
                newRoute.obstacles.Add(newRoute.obstaclePools[i]);
            }
            else
            {
                newRoute.obstaclePools[i].gameObject.SetActive(false);
            }
        }
        //int pointIndex = 0;
        newRoute.currentFalseTrail.transforms = new List<Points>();
        newRoute.currentTrail.transforms = new List<Points>();

        for (int i=0; i<newRoute.currentTrail.transformsPool.Count; i++)
        {
            if (i < trueTrail.transforms.Count)
            {
                newRoute.currentTrail.transforms.Add(newRoute.currentTrail.transformsPool[i]);
                newRoute.currentTrail.transforms[i].transform.localPosition = trueTrail.transforms[i].transform.localPosition;
                newRoute.currentTrail.transforms[i].transform.gameObject.SetActive(true);
                newRoute.currentTrail.transforms[i].spriteRenderer.sprite = trueTrail.transforms[i].sprite;
                newRoute.currentTrail.transforms[i].sprite = trueTrail.transforms[i].sprite;
            }
            else
            {
                newRoute.currentTrail.transformsPool[i].transform.gameObject.SetActive(false);
            }
            if (i < falseTrail.transforms.Count)
            {
                newRoute.currentFalseTrail.transforms.Add(newRoute.currentFalseTrail.transformsPool[i]);
                newRoute.currentFalseTrail.transforms[i].transform.localPosition = falseTrail.transforms[i].transform.localPosition;
                newRoute.currentFalseTrail.transforms[i].transform.gameObject.SetActive(true);
                newRoute.currentFalseTrail.transforms[i].spriteRenderer.sprite = falseTrail.transforms[i].sprite;
                newRoute.currentFalseTrail.transforms[i].sprite = falseTrail.transforms[i].sprite;
            }
            else
            {
                newRoute.currentFalseTrail.transformsPool[i].transform.gameObject.SetActive(false);
            }
        }
        foreach (Points p in newRoute.currentFalseTrail.transforms)
        {
            p.spriteRenderer.color = Color.white;
        }
        /*foreach (Points p in trueTrail.transforms)
        {
            newRoute.currentTrail.transforms[pointIndex].transform.localPosition = p.transform.localPosition;
            newRoute.currentTrail.transforms[pointIndex].transform.gameObject.SetActive(true);
            newRoute.currentTrail.transforms[pointIndex].transform.GetComponent<SpriteRenderer>().sprite = p.sprite;
            newRoute.currentTrail.transforms[pointIndex].sprite = p.sprite;
            pointIndex++;
        }
        pointIndex = 0;
        foreach (Points p in falseTrail.transforms)
        {
            newRoute.currentFalseTrail.transforms[pointIndex].transform.localPosition = p.transform.localPosition;
            newRoute.currentFalseTrail.transforms[pointIndex].transform.gameObject.SetActive(true);
            newRoute.currentFalseTrail.transforms[pointIndex].transform.GetComponent<SpriteRenderer>().sprite = p.sprite;
            newRoute.currentFalseTrail.transforms[pointIndex].sprite = p.sprite;
            pointIndex++;
        }*/
        newRoute.currentTrail.side = trueTrail.side;
        newRoute.dummies = new List<Route>();
        newRoute.currentFalseTrail.transforms[0].transform.parent.gameObject.SetActive(false);
        newRoute.currentTrail.transforms[0].transform.parent.gameObject.SetActive(false);
        newRoute.gameObject.SetActive(true);
    }
    private void SetDummyRoute(Route newRoute, List<SpriteRenderer> obstacles)
    {
        newRoute.obstacles = new List<SpriteRenderer>();
        for (int i = 0; i < newRoute.obstaclePools.Count; i++)
        {
            if (i < obstacles.Count)
            {
                newRoute.obstaclePools[i].gameObject.SetActive(true);
                newRoute.obstaclePools[i].transform.localPosition = obstacles[i].transform.localPosition;
                newRoute.obstacles.Add(newRoute.obstaclePools[i]);
            }
            else
            {
                newRoute.obstaclePools[i].gameObject.SetActive(false);
            }
        }
        newRoute.currentFalseTrail.transforms[0].transform.parent.gameObject.SetActive(false);
        newRoute.currentTrail.transforms[0].transform.parent.gameObject.SetActive(false);
        newRoute.gameObject.SetActive(true);
    }
    Trail lastTrail;
    Route GetTopSquare()
    {
        List<Route> topRoutes = currentLevelSquares.FindAll(r => r.trueTrail.Find(t => t.side == 156) != null);
        Route s = topRoutes[Random.Range(0, topRoutes.Count)];
        Route returnSquare = poolSquares[0];
        poolSquares.RemoveAt(0);
        returnSquare.speed = s.speed;
        List<Trail> topTrails = s.trueTrail.FindAll(t => t.side == 156);
        if (topTrails.Count > 1 && topTrails.Contains(lastTrail))
        {
            topTrails.Remove(lastTrail);
        }
        Trail currentTrueTrail = topTrails[Random.Range(0, topTrails.Count)];
        lastTrail = currentTrueTrail;
        SetRoute(returnSquare, s.obstacles, currentTrueTrail, s.falseTrail[Random.Range(0, s.falseTrail.Count)]);
        if (pathSquares.Count > 0)
            returnSquare.transform.SetParent(pathSquares[pathSquares.Count - 1].transform);
        return returnSquare;
    }
    Route GetLeftSquare()
    {
        List<Route> leftRoutes = currentLevelSquares.FindAll(r => r.trueTrail.Find(t => t.side == 157) != null);
        Route s = leftRoutes[Random.Range(0, leftRoutes.Count)];
        Route returnSquare = poolSquares[0];
        poolSquares.RemoveAt(0);
        returnSquare.speed = s.speed;
        List<Trail> leftTrails = s.trueTrail.FindAll(t => t.side == 157);
        if (leftTrails.Count > 1 && leftTrails.Contains(lastTrail))
        {
            leftTrails.Remove(lastTrail);
        }
        Trail currentTrueTrail = leftTrails[Random.Range(0, leftTrails.Count)];
        lastTrail = currentTrueTrail;
        SetRoute(returnSquare, s.obstacles, currentTrueTrail, s.falseTrail[Random.Range(0, s.falseTrail.Count)]);
        if (pathSquares.Count > 0)
            returnSquare.transform.SetParent(pathSquares[pathSquares.Count - 1].transform);
        return returnSquare;
    }
    Route GetRightSquare()
    {
        List<Route> rightRoutes = currentLevelSquares.FindAll(r => r.trueTrail.Find(t => t.side == 0) != null);
        Route s = rightRoutes[Random.Range(0, rightRoutes.Count)];
        Route returnSquare = poolSquares[0];
        poolSquares.RemoveAt(0);
        returnSquare.speed = s.speed;
        List<Trail> rightTrails = s.trueTrail.FindAll(t => t.side == 0);
        if (rightTrails.Count > 1 && rightTrails.Contains(lastTrail))
        {
            rightTrails.Remove(lastTrail);
        }
        Trail currentTrueTrail = rightTrails[Random.Range(0, rightTrails.Count)];
        lastTrail = currentTrueTrail;
        SetRoute(returnSquare, s.obstacles, currentTrueTrail, s.falseTrail[Random.Range(0, s.falseTrail.Count)]);
        if (pathSquares.Count > 0)
            returnSquare.transform.SetParent(pathSquares[pathSquares.Count - 1].transform);
        return returnSquare;
    }
    int[] types = { 156, 157, 0 };
    float[] rotations = { 0f, 90f, 180f, -90f };
    int toTheRight;
    int toTheLeft;
    void GeneratePath()
    {
        int c = 5 - pathSquares.Count;
        for (int i = 0; i < c; i++)
        {
            if (pathSquares.Count > 0)
            {
                if (pathSquares[pathSquares.Count - 1].currentTrail.side == 156)
                {
                    //int side = types[Random.Range(0, types.Length)]; to uncomment
                    int side = 0;//from this
                    if (toTheRight >= 2)
                    {
                        side = types[Random.Range(0, 2)];
                    }
                    else if (toTheLeft >= 2)
                    {
                        side = types[Random.Range(0, 2) == 0 ? 0 : 2];
                    }
                    else
                    { 
                        side = types[Random.Range(0, types.Length)];
                    }//till this
                    Route s;
                    if (side == 156)
                    {
                        s = GetTopSquare();
                    }
                    else if (side == 157)
                    {
                        s = GetLeftSquare();
                        toTheLeft++;
                        toTheRight = 0;
                    }
                    else
                    {
                        s = GetRightSquare();
                        toTheLeft = 0;
                        toTheRight++;
                    }
                    s.coordinate = pathSquares[pathSquares.Count - 1].coordinate + sides[0];
                    SetRotation(s);
                    s.Init();
                    s.transform.position = new Vector3(s.coordinate.x * 7f, s.coordinate.y * 7f, 0f);
                    SetTweenToSquare(s);
                    pathSquares.Add(s);
                    s.gameObject.transform.parent = pathParent.transform;
                    s.gameObject.name = "Path" + generatedSquares;
                }
                else if (pathSquares[pathSquares.Count - 1].currentTrail.side == 157)
                {
                    int side = types[Random.Range(0, 2) == 0 ? 0 : 2];
                    Route s;
                    if (side == 156)
                        s = GetTopSquare();
                    else if (side == 157)
                        s = GetLeftSquare();
                    else
                        s = GetRightSquare();

                    ShiftLeft();
                    SetRotation(s);
                    s.coordinate = pathSquares[pathSquares.Count - 1].coordinate + sides[0];
                    s.transform.position = new Vector3(s.coordinate.x * 7f, s.coordinate.y * 7f, 0f);
                    s.Init();
                    SetTweenToSquare(s);
                    pathSquares.Add(s);
                    s.gameObject.transform.parent = pathParent.transform;
                    s.gameObject.name = "Path" + generatedSquares;
                }
                else if (pathSquares[pathSquares.Count - 1].currentTrail.side == 0)
                {
                    int side = types[Random.Range(0, 2)];
                    Route s;
                    if (side == 156)
                        s = GetTopSquare();
                    else if (side == 157)
                        s = GetLeftSquare();
                    else
                        s = GetRightSquare();

                    ShiftRight();
                    SetRotation(s);
                    s.coordinate = pathSquares[pathSquares.Count - 1].coordinate + sides[0];
                    s.transform.position = new Vector3(s.coordinate.x * 7f, s.coordinate.y * 7f, 0f);
                    s.Init();
                    SetTweenToSquare(s);
                    pathSquares.Add(s);
                    s.gameObject.transform.parent = pathParent.transform;
                    s.gameObject.name = "Path" + generatedSquares;
                }
                SetDummies();
                SetSpritesToSquare(pathSquares[pathSquares.Count - 1]);
                pathSquares[pathSquares.Count - 1].transform.position = new Vector3(Mathf.Round(pathSquares[pathSquares.Count - 1].transform.position.x), Mathf.Round(pathSquares[pathSquares.Count - 1].transform.position.y), Mathf.Round(pathSquares[pathSquares.Count - 1].transform.position.z));
            }
            else
            {
                int side = types[Random.Range(0, types.Length)];
                Route s;
                if (side == 156)
                    s = GetTopSquare();
                else if (side == 157)
                    s = GetLeftSquare();
                else
                    s = GetRightSquare();
                s.transform.position = new Vector3(0f, 0f, 0f);
                SetRotation(s);
                SetTweenToSquare(s);
                s.Init();
                s.coordinate = new Vector2Int(0, 0);
                pathSquares.Add(s);
                SetDummies();
                SetSpritesToSquare(pathSquares[pathSquares.Count - 1]);

                pathSquares[pathSquares.Count - 1].transform.position = new Vector3(Mathf.Round(pathSquares[pathSquares.Count - 1].transform.position.x), Mathf.Round(pathSquares[pathSquares.Count - 1].transform.position.y), Mathf.Round(pathSquares[pathSquares.Count - 1].transform.position.z));
                s.gameObject.name = "Path" + generatedSquares;
                s.transform.parent = pathParent.transform;
            }
            generatedSquares++;
            if (generatedSquares > currentLevel.level)
            {
                SetCurrentLevelSquares();
                GenerateNewLevelPath();
            }
            if (MainRoot.Instance.userConfig.highScore > 0 && generatedSquares == MainRoot.Instance.userConfig.highScore + 1 && !highScoreFlagged)
            {
                highScoreFlag.transform.position = pathSquares[pathSquares.Count - 1].currentTrail.transforms[0].transform.position;
                highScoreFlag.transform.SetLocalRotationZ(pathSquares[pathSquares.Count - 1].transform.eulerAngles.z);
                highScoreFlag.SetActive(true);
                highScoreFlagged = true;
                highScoreRoute = pathSquares[pathSquares.Count - 1];
            }
        }
    }
    bool highScoreFlagged;
    void GenerateNewLevelPath()
    {
        
        newLevelFlag.SetActive(true);
        newLevelFlag.transform.position = pathSquares[pathSquares.Count - 1].currentTrail.transforms[0].transform.position;
        newLevelFlag.transform.SetLocalRotationZ(pathSquares[pathSquares.Count - 1].transform.eulerAngles.z);
        for (int i = 0; i < 4; i++)
        {
            Route r = i==0 ? Z000 : (i==3 ? Z002 : Z001);
            Route s = poolSquares[0];
            poolSquares.RemoveAt(0);

            //s.speed = r.speed;
            //s.transform.SetParent(pathSquares[pathSquares.Count - 1].transform);
            /*if (pathSquares[pathSquares.Count - 1].currentTrail.side == 156)
            {
                s.transform.localPosition = new Vector3(0f, 7f, 0f);
            }
            else */
            if (pathSquares[pathSquares.Count - 1].currentTrail.side == 157)
            {
                //s.transform.localPosition = new Vector3(-7f, 0f, 0f);
                //s.transform.SetLocalRotationZ(90);
                ShiftLeft();
            }
            else if(pathSquares[pathSquares.Count - 1].currentTrail.side == 0)
            {
                //s.transform.localPosition = new Vector3(7f, 0f, 0f);
                //s.transform.SetLocalRotationZ(-90);
                ShiftRight();
            }
            s.coordinate = pathSquares[pathSquares.Count - 1].coordinate + sides[0];
            s.transform.position = new Vector3(7f * s.coordinate.x, 7f * s.coordinate.y, 0f);
            SetRotation(s);
            s.Init();
            SetTweenToSquare(s);
            pathSquares.Add(s);
            s.gameObject.transform.parent = pathParent.transform;
            s.gameObject.name = "LevelUp";
            //s.currentTrail = s.trueTrail[0];
            //s.currentTrail.transforms[0].transform.parent.gameObject.SetActive(true);
            SetRoute(s, r.obstacles, r.trueTrail[0], r.falseTrail[0]);
            //if(i < 3)
            SetDummies(true);
            SetSpritesToSquare(pathSquares[pathSquares.Count - 1]);
        }
        GeneratePath();
    }
    void SetDummies(bool levelUp = false)
    {
        List<Vector2Int> leftForDummies = new List<Vector2Int>(arounds);
        Route last = pathSquares[pathSquares.Count - 1];
        if (pathSquares.Count > 1)
        {
            Route preLast = pathSquares[pathSquares.Count - 2];

            Route toRemove = preLast.dummies.Find(d => d.coordinate == last.coordinate);

            if (toRemove != null)
            {
                preLast.dummies.Remove(toRemove);
                toRemove.gameObject.SetActive(false);
                poolSquares.Add(toRemove);
                toRemove.transform.parent = squaresParent.transform;
            }
            //Destroy(toRemove.gameObject);
            foreach (Vector2Int v in arounds)
            {
                Route s = preLast.dummies.Find(d => d.coordinate == last.coordinate + v);
                if (s != null)
                {
                    preLast.dummies.Remove(s);
                    last.dummies.Add(s);
                    leftForDummies.Remove(v);
                    s.transform.parent = last.transform;
                }
                Route p = pathSquares.Find(d => d.coordinate == last.coordinate + v);
                if (p != null)
                {
                    leftForDummies.Remove(v);
                }
            }
            foreach (Vector2Int v in leftForDummies)
            {
                Route r = levelUp ? Z001 : (PlayerPrefs.GetInt("TutorialXML", 0) == 0 && generatedSquares < 20 ? tutorialDummies[Random.Range(0, tutorialDummies.Count)] : generalDummies[Random.Range(0, generalDummies.Count)]);
                Route s = poolSquares[0];
                poolSquares.RemoveAt(0);
                SetDummyRoute(s, r.obstacles);
                s.coordinate = last.coordinate + v;
                s.transform.position = new Vector3(7f * s.coordinate.x, 7f * s.coordinate.y, 0f);
                s.transform.SetLocalRotationZ(rotations[Random.Range(0, rotations.Length)]);
                s.Init();
                SetTweenToSquare(s);
                last.dummies.Add(s);
                s.transform.parent = last.transform;
            }
        }
        else
        {
            foreach (Vector2Int v in leftForDummies)
            {
                Route r;
                if (v == Vector2Int.down)
                    r = r = squares.Find(ss => ss.gameObject.name == "RouteDummyEmpty");
                else
                    r = PlayerPrefs.GetInt("TutorialXML", 0) == 0 && generatedSquares < 20 ? tutorialDummies[Random.Range(0, tutorialDummies.Count)] : generalDummies[Random.Range(0, generalDummies.Count)];
                Route s = poolSquares[0];
                poolSquares.RemoveAt(0);
                SetDummyRoute(s, r.obstacles);
                s.coordinate = last.coordinate + v;
                s.transform.position = new Vector3(7f * s.coordinate.x, 7f * s.coordinate.y, 0f);
                s.transform.SetLocalRotationZ(0f);
                last.dummies.Add(s);
                s.transform.parent = last.transform;
                SetTweenToSquare(s);
                if (v == Vector2Int.down)
                {
                    firstRoad.transform.parent = s.gameObject.transform;
                    firstRoad.transform.localPosition = new Vector3(0f, 7f, 0f);
                    firstRoad.transform.eulerAngles = Vector3.zero;
                    firstRoad.SetActive(true);
                }
            }
            for (int i = -1; i < 2; i++)
            {
                Vector2Int v = new Vector2Int(i, -2);
                Route r = PlayerPrefs.GetInt("TutorialXML", 0) == 0 && generatedSquares < 20 ? tutorialDummies[Random.Range(0, tutorialDummies.Count)] : generalDummies[Random.Range(0, generalDummies.Count)];
                Route s = poolSquares[0];
                poolSquares.RemoveAt(0);
                SetDummyRoute(s, r.obstacles);
                s.coordinate = last.coordinate + v;
                s.transform.position = new Vector3(7f * s.coordinate.x, 7f * s.coordinate.y, 0f);
                s.transform.SetLocalRotationZ(0f);
                last.dummies.Add(s);
                s.transform.parent = last.transform;
            }
        }
    }
    void SetTweenToSquare(Route s)
    {
        //s.obstacles[0].transform.parent.position = s.obstacles[0].transform.parent.position - new Vector3(0f, 0.05f, 0f);
        //s.obstacles[0].transform.parent.DOMoveY(s.obstacles[0].transform.parent.position.y + 0.05f, 3f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
    }

    public bool lost;
    void RightPressed()
    {
        if (rightBtnTrail)
        {
            SetRoadButton();
            CSSoundManager.instance.PlaySound(4);
        }
        else
        {
            WrongButton();
            if (buttonColorTween != null)
                buttonColorTween.Kill();
            //buttonColorTween = roadRightBtn.image.DOColor(Color.red, 0.3f).SetLoops(6, LoopType.Yoyo);
            buttonColorTween = rightWrongImage.DOFade(0.75f, 0.3f).SetLoops(6, LoopType.Yoyo);
            CSSoundManager.instance.PlaySound(5);
        }
    }
    Tween buttonColorTween;
    void LeftPressed()
    {
        if (leftBtnTrail)
        {
            SetRoadButton();
            CSSoundManager.instance.PlaySound(4);
        }
        else
        {
            WrongButton();
            if (buttonColorTween != null)
                buttonColorTween.Kill();
            //buttonColorTween = roadLeftBtn.image.DOColor(Color.red, 0.3f).SetLoops(6, LoopType.Yoyo);
            buttonColorTween = leftWrongImage.DOFade(0.75f, 0.3f).SetLoops(6, LoopType.Yoyo);
            CSSoundManager.instance.PlaySound(5);
        }
    }
    void WrongButton()
    {
        if (tutorialAnimation.gameObject.activeSelf)
        {
            tutorialWrongTxt.gameObject.SetActive(false);
            tutorialWrongTxt.gameObject.SetActive(true);
            return;
        }
        loseTxt.gameObject.SetActive(false);
        loseTxt.gameObject.SetActive(true);
        lost = true;
        EnablePathButton(false);
        SetWrongRoad();
        car.speed = 5f;
        car.currentDest = Vector3.back;

    }
    void SetShineObstacles()
    {
        int shineCount = pathSquares[0].obstacles.Count > 20 ? 20 : pathSquares[0].obstacles.Count;
        List<SpriteRenderer> obsList = new List<SpriteRenderer>(pathSquares[0].obstacles);
        foreach (Route r in pathSquares[1].dummies)
        {
            obsList.AddRange(r.obstacles);
        }
        for (int i = 0; i < shineCount; i++)
        {
            SpriteRenderer r = obsList[Random.Range(0, obsList.Count)];
            shineSprites[i].transform.parent = r.transform;
            shineSprites[i].transform.localPosition = Vector3.zero;
            shineSprites[i].gameObject.SetActive(true);
            //if(!updatingColors)
                shineSprites[i].sprite = obstacleShineSprite;
            obsList.Remove(r);
        }
        for (int i = shineCount; i < 20; i++)
        {
            shineSprites[i].transform.parent = null;
            shineSprites[i].gameObject.SetActive(false);
        }
    }
    public void SetRoadAfterLevelUp()
    {
        for (int i = 0; i < 3; i++)
        {
            trashSquares.Add(pathSquares[0]);
            pathSquares.RemoveAt(0);
            CheckTrash();
            GeneratePath();
        }
        SetShineObstacles();
        cameraFollow.rot = pathSquares[0].transform.eulerAngles.z;
        cameraFollow.SetOffset();
        cameraFollow.SetTarget(pathSquares[0].gameObject);
        SetButtonTrailView();
        arrows.SetActive(false);
        DOTween.Play("1004");
    }
    void SetRoadButton()
    {
        foreach (Tween t in tutorialPathTweens)
        {
            t.Rewind();
            t.Kill();
        }
        if (buttonColorTween != null)
            buttonColorTween.Kill();
        //roadRightBtn.image.color = Color.white;
        //roadLeftBtn.image.color = Color.white;
        rightWrongImage.color = new Color(1, 1, 1, 0);
        leftWrongImage.color = new Color(1, 1, 1, 0);
        tutorialWrongTxt.gameObject.SetActive(false);
        if (trashSquares.Count > 0)
            car.SpeedUpCarToDest();//car.currentDest = (trashSquares[trashSquares.Count - 1].currentTrail.transforms[trashSquares[trashSquares.Count - 1].currentTrail.transforms.Count - 1].transform.position + pathSquares[0].currentTrail.transforms[0].transform.position) * 0.5f;
        else
            car.currentDest = pathSquares[0].currentTrail.transforms[0].transform.position + Vector3.down * 0.5f; 
        SetCarPoints(pathSquares[0].currentTrail.transforms, pathSquares[1].currentTrail.transforms[0].transform.position);
        pathSquares[0].currentTrail.transforms[0].transform.parent.gameObject.SetActive(true);
        foreach (Points p in pathSquares[0].currentTrail.transforms)
        {
            p.spriteRenderer.color = Color.white;
        }
        trashSquares.Add(pathSquares[0]);
        car.speedLevel = pathSquares[0].speed;
        pathSquares.RemoveAt(0);
        CheckTrash();
        GeneratePath();
        cameraFollow.rot = pathSquares[0].transform.eulerAngles.z;
        cameraFollow.SetOffset();
        cameraFollow.SetTarget(pathSquares[0].gameObject);
        score++;
        scoreTxt.text = score.ToString();
        SetButtonTrailView();

        SetShineObstacles();
        CheckHighScore();
        timeBarTween.Kill();
        if (timeBar.fillAmount > 0.7f)
        {
            combo++;
        }
        else
        {
            combo = 0;
        }
        comboTxt.gameObject.SetActive(false);
        comboTxt.text = "x" + combo;
        timeBar.fillAmount = 1f;
        if (combo > 1)
        {
            comboTxt.gameObject.SetActive(true);
            MainRoot.Instance.userConfig.softCurrency += combo;
        }
        if (spriteLevel.level > 0 && score > spriteLevel.level)
        {
            SetCurrentSprites();
            SetSpritesToWorld();
            SetNextLevelPoints();
            levelUpTxt.gameObject.SetActive(false);
            levelUpTxt.gameObject.SetActive(true);
            //ServiceGameAnalytics.Instance.LogLevelUp(spriteLevel.level);
            //ServiceFirebaseAnalytics.Instance.LogLevelUp(spriteLevel.level);
            //ServiceFacebook.Instance.LogLevelUp(spriteLevel.level);
        }
        if (!MainRoot.Instance.userConfig.isTutorialPassed && score <= 3)
        {
            if (PlayerPrefs.GetInt("XMLIDSet", 0) == 0)
            {
                int xmlId = Random.Range(0, 2);
                MainRoot.Instance.userConfig.levelXmlId = xmlId;
                Firebase.Analytics.FirebaseAnalytics.SetUserProperty("XML", xmlId.ToString());
                BuildInfo.instance.SetText();
                PlayerPrefs.SetInt("XMLIDSet", 1);
                Firebase.Analytics.FirebaseAnalytics.SetUserProperty("First20", "false");
            }
            if (score == 3)
            {
                //MainRoot.Instance.userConfig.isTutorialPassed = true;
                tutorialAnimation.gameObject.SetActive(false);
                tutorialEndTxt.gameObject.SetActive(true);
                tutorialEndTxt.text = "You are on your own now!";
                tutorialTxt.gameObject.SetActive(false);
            }
            if (score < 3)
            {
                pathSquares[0].currentTrail.transforms[0].transform.parent.gameObject.SetActive(true);
                tutorialPathTweens = new List<Tween>();
                foreach (Points p in pathSquares[0].currentTrail.transforms)
                {
                    tutorialPathTweens.Add(p.spriteRenderer.DOFade(0.6f, 0.6f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).ChangeStartValue(new Color(1, 1, 1, 0)));
                }
            }
        }
        if (PlayerPrefs.GetInt("TutorialXML", 0) == 0 && score == 20)
        {
            PlayerPrefs.SetInt("TutorialXML", 1);
            Firebase.Analytics.FirebaseAnalytics.SetUserProperty("First20", "true");
            MainRoot.Instance.userConfig.isTutorialPassed = true;
        }
    }
    void SetNextLevelPoints()
    {
        //StartCoroutine(SetFollowPlayer());
        for (int i = 0; i < 4; i++)
        {
            pathSquares[i].currentTrail.transforms[0].transform.parent.gameObject.SetActive(true);
            SetCarPoints(pathSquares[i].currentTrail.transforms, pathSquares[i+1].currentTrail.transforms[0].transform.position);
            if (i == 2)
            {
                car.NextLevel((pathSquares[i].currentTrail.transforms[pathSquares[i].currentTrail.transforms.Count - 1].transform.position + pathSquares[i + 1].currentTrail.transforms[0].transform.position) * 0.5f);
            }
        }
        car.levelUp = true;
        car.playerTarget = trashSquares[trashSquares.Count - 1].currentTrail.transforms[trashSquares[trashSquares.Count - 1].currentTrail.transforms.Count - 1].transform.position;//pathSquares[0].currentTrail.transforms[0].transform.position;
        arrows.SetActive(true);
        arrows.transform.position = pathSquares[1].transform.position;
        arrows.transform.SetLocalRotationZ(pathSquares[1].transform.eulerAngles.z);
        DOTween.Play("1003");
        //for (int i = 0; i < 2; i++)
        {
            trashSquares.Add(pathSquares[0]);
            pathSquares.RemoveAt(0);
            CheckTrash();
            GeneratePath();
        }
        SetSpritesLevelUp();
    }
    IEnumerator SetFollowPlayer()
    {
        yield return new WaitForSeconds(2f);
        cameraFollow.SetFollowPlayer(true);
    }
    void SetWrongRoad()
    {
        SetCarPoints(pathSquares[0].currentFalseTrail.transforms, pathSquares[1].currentTrail.transforms[0].transform.position);
        pathSquares[0].currentFalseTrail.transforms[0].transform.parent.gameObject.SetActive(true);
        timeBarTween.Kill();
        timeBar.fillAmount = 0f;
        combo = 0;
        comboTxt.gameObject.SetActive(false);
        bool found = false;
        foreach (Points p in pathSquares[0].currentFalseTrail.transforms)
        {
            if (!found)
            {
                foreach (SpriteRenderer s in pathSquares[0].obstacles)
                {
                    if (s.gameObject.activeSelf && RoundedVector3(s.transform.position) == RoundedVector3(p.transform.position))
                    {
                        found = true;
                        p.spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                }
            }
            else
            {
                p.spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
        //car.Lose();
    }
    void SetCarPoints(List<Points> transformPoints, Vector3 nextSquare)
    {
        List<Transform> points = new List<Transform>();
        foreach (Points p in transformPoints)
        {
            //if(p.transform.gameObject.activeSelf)
            points.Add(p.transform);
        }
        car.BezierPoints(points, nextSquare);        
    }
    public void Continue()
    {
        DOTween.Play("1004");
        pathSquares[0].currentFalseTrail.transforms[0].transform.parent.gameObject.SetActive(false);
        car.bezierPoints = new List<Vector3>();
        SetCarPoints(trashSquares[trashSquares.Count - 1].currentTrail.transforms, pathSquares[0].currentTrail.transforms[0].transform.position);
        car.transform.position = car.bezierPoints[0];

        lost = false;
        //car.currentDest = trashSquares[trashSquares.Count - 1].currentTrail.transforms[trashSquares[trashSquares.Count - 1].currentTrail.transforms.Count - 1].transform;
        EnablePathButton(true);
        //car.speed = 0.5f;
        car.UpdateSpeed();
        car.blowParticle.SetActive(false);
        car.EnableParticle();
        //SetTimeBar();
        gameStarted = true;
        loseTxt.gameObject.SetActive(false);
    }
    bool rightBtnTrail;
    bool leftBtnTrail;
    void SetButtonTrailView()
    {
        if (!MainRoot.Instance.userConfig.isTutorialPassed)
        {
                rightBtnTrail = score % 2 == 0;
        }
        else
        {
            rightBtnTrail = Random.Range(0, 2) == 1;
        }
        leftBtnTrail = !rightBtnTrail;
        if (rightBtnTrail)
        {
            rightButtonTrailView.SetRoad(pathSquares[0].currentTrail.transforms);
            leftButtonTrailView.SetRoad(pathSquares[0].currentFalseTrail.transforms);
            if (score < 3 && !MainRoot.Instance.userConfig.isTutorialPassed)
            {
                //tutorialAnimation.transform.position = roadRightBtn.transform.position + new Vector3(0f, 100f, 0f);
                tutorialAnimation.anchoredPosition = new Vector2(200f, 220f);
                tutorialAnimation.transform.localScale = Vector3.one;
                tutorialTxt.gameObject.SetActive(false);
                tutorialTxt.gameObject.SetActive(true);
                //DOTween.Play("1101");
                //tutorialTxt.transform.DOPunchScale(Vector3.one * 1.2f, 1f).ChangeStartValue(Vector3.one);
                tutorialTxt.text = "Choose the correct path!";
            }
        }
        else
        {
            rightButtonTrailView.SetRoad(pathSquares[0].currentFalseTrail.transforms);
            leftButtonTrailView.SetRoad(pathSquares[0].currentTrail.transforms);
            if (score < 3 && !MainRoot.Instance.userConfig.isTutorialPassed)
            {
                //tutorialAnimation.transform.position = roadLeftBtn.transform.position + new Vector3(0f, 100f, 0f);
                tutorialAnimation.anchoredPosition = new Vector2(-200f, 220f);
                tutorialAnimation.transform.localScale = new Vector3(-1f, 1f, 1f);
                tutorialTxt.gameObject.SetActive(false);
                tutorialTxt.gameObject.SetActive(true);
                //DOTween.Play("1101");
                //tutorialTxt.transform.DOPunchScale(Vector3.one * 1.2f, 1f).ChangeStartValue(Vector3.one);
                tutorialTxt.text = "Choose the correct path!";
            }
        }
    }
    void CheckTrash()
    {
        if (trashSquares.Count > 4)
        {
            Route s = trashSquares[0];
            trashSquares.RemoveAt(0);
            foreach (Route d in s.dummies)
            {
                d.gameObject.SetActive(false);
                poolSquares.Add(d);
                d.transform.parent = squaresParent.transform;
                //Destroy(d.gameObject);
            }
            s.gameObject.SetActive(false);
            poolSquares.Add(s);
            s.transform.parent = squaresParent.transform;
            if (highScoreRoute == s)
            {
                highScoreFlag.SetActive(false);
            }
                //Destroy(s.gameObject);
                if (firstRoad.activeSelf)
                firstRoad.SetActive(false);
        }
    }
    public void EnablePathButton(bool active)
    {
        roadRightBtn.interactable = active;
        roadLeftBtn.interactable = active;
        if (active)
        {
            timeBar.color = Color.white;
            timeBarBG.color = Color.white;
        }
        else
        {
            timeBar.color = new Color(200f/255f, 200f / 255f, 200f / 255f, 128f/255f);
            timeBarBG.color = new Color(200f / 255f, 200f / 255f, 200f / 255f, 128f / 255f);
        }
    }
    public void GameOver(string how = "")
    {
        DOTween.Play("1003");
        CheckHighScore();
        FinishPopup.instance.OpenPanel("GAME OVER", 2f, false, MainRoot.Instance.userConfig.highScore > prevHighScore);
        EnablePathButton(false);
        car.ClearParticle();
        //ServiceGameAnalytics.Instance.LogDefeat(score);
        //ServiceFirebaseAnalytics.Instance.LogDefeat(score);
        ServiceGameAnalytics.Instance.LogLevelFail(spriteLevel.level, how);
        ServiceFirebaseAnalytics.Instance.LogLevelFail(score, how);
        gameStarted = false;
    }
    bool highScored;
    public void CheckHighScore()
    {
        if (score > MainRoot.Instance.userConfig.highScore)
        {
            MainRoot.Instance.userConfig.highScore = score;
            if (highScoreFlag.activeSelf && !highScored)
            {
                highScored = true;
                recordTxt.gameObject.SetActive(true);
                //highScoreFlag.SetActive(false);
                CSSoundManager.instance.PlaySound(2);
            }
        }
    }
    void SetRotation(Route r)
    {
        if (sides[0] == Vector2Int.up)
            r.transform.SetRotationZ(0);
        else if (sides[0] == Vector2Int.left)
            r.transform.SetRotationZ(90);
        else if (sides[0] == Vector2Int.down)
            r.transform.SetRotationZ(180);
        else if (sides[0] == Vector2Int.right)
            r.transform.SetRotationZ(-90);
    }
    public List<Vector2Int> sides;
    public List<Vector2Int> arounds;
    void InitData()
    {
        sides = new List<Vector2Int>();
        sides.Add(Vector2Int.up);
        sides.Add(Vector2Int.left);
        sides.Add(Vector2Int.down);
        sides.Add(Vector2Int.right);
        arounds = new List<Vector2Int>();
        arounds.Add(Vector2Int.up);
        arounds.Add(Vector2Int.left);
        arounds.Add(Vector2Int.down);
        arounds.Add(Vector2Int.right);
        arounds.Add(new Vector2Int(1, 1));
        arounds.Add(new Vector2Int(-1, 1));
        arounds.Add(new Vector2Int(1, -1));
        arounds.Add(new Vector2Int(-1, -1));

        colors = new List<string>();
        colors.Add("Blue");
        colors.Add("Brown");
        colors.Add("Green");
        colors.Add("Purple");
        colors.Add("Red");

        obstacleDict = new Dictionary<string, Sprite>();
        obstacleShineDict = new Dictionary<string, Sprite>();
        obstacleDict.Add("Blue", obstacleSprites[0]);
        obstacleDict.Add("Brown", obstacleSprites[1]);
        obstacleDict.Add("Green", obstacleSprites[2]);
        obstacleDict.Add("Purple", obstacleSprites[3]);
        obstacleDict.Add("Red", obstacleSprites[4]);
        obstacleShineDict.Add("Blue", obstacleShineSprites[0]);
        obstacleShineDict.Add("Brown", obstacleShineSprites[1]);
        obstacleShineDict.Add("Green", obstacleShineSprites[2]);
        obstacleShineDict.Add("Purple", obstacleShineSprites[3]);
        obstacleShineDict.Add("Red", obstacleShineSprites[4]);
        ServiceIronSource.Instance.CallDestroyBanner();
        squares = new List<Route>();
        GameObject[] gs = ServiceResources.LoadAll<GameObject>("Prefabs/Routes/");
        foreach (GameObject g in gs)
        {
            squares.Add(g.GetComponent<Route>());
        }
        Z001 = squares.Find(ss => ss.gameObject.name == "RouteZ001");
        squares.Remove(Z001);
        Z000 = squares.Find(ss => ss.gameObject.name == "RouteZ000");
        squares.Remove(Z000);
        Z002 = squares.Find(ss => ss.gameObject.name == "RouteZ002");
        squares.Remove(Z002);
        tutorialPathTweens = new List<Tween>();
    }
    void ShiftLeft()
    {
        Vector2Int a = sides[0];
        sides.RemoveAt(0);
        sides.Add(a);
    }
    void ShiftRight()
    {
        Vector2Int a = sides[sides.Count - 1];
        sides.RemoveAt(sides.Count - 1);
        sides.Insert(0, a);
    }
    Tweener timeBarTween;
    public void SetTimeBar()
    {   if(timeBarTween != null)
            timeBarTween.Kill();
        timeBar.fillAmount = 1f;
        //Debug.Log((score > 0 ? 14f : 13f) / car.speedLevel);
        timeBarTween = timeBar.DOFillAmount(0f, (score > 0 ? 14f : 13f) / car.speedLevel).SetEase(Ease.Linear);
    }
    Vector3 RoundedVector3(Vector3 v)
    {
        Vector3 r = new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        return r;
    }
    private void OnApplicationFocus(bool isFocus)
    {
        if (!isFocus)
        {
            ServiceFirebaseAnalytics.Instance.LogEnableMusic(System.Convert.ToInt32((MainRoot.Instance.userConfig.isMusic)));
            ServiceFirebaseAnalytics.Instance.LogEnableSound(System.Convert.ToInt32((MainRoot.Instance.userConfig.isSound)));
            ServiceGameAnalytics.Instance.LogEnableMusic(System.Convert.ToInt32((MainRoot.Instance.userConfig.isMusic)));
            ServiceGameAnalytics.Instance.LogEnableMusic(System.Convert.ToInt32((MainRoot.Instance.userConfig.isSound)));
#if !UNITY_EDITOR
            if(Time.timeScale > 0f)
			    TopBar.Pause?.Invoke();
#endif
        }
    }

    #region Audio
    private void InitAudio()
    {
        //CSSoundManager.instance.PlayLoopingMusic(1);
    }
    #endregion

    #region Preview Methods
    private void MethodsTest()
    {
        //---Finish---
        //FinishPopup --> CompletePopup
        //            --> DefeatPopup

        //Call Victory
        //FinishPopup.instance.OpenPanel("Win", 0, true, 2);
        //Call Defeat
        //FinishPopup.instance.OpenPanel("Defeat", 0, false, 0);

        //---Audio---
        //Volume Music Range (0, 1)
        CSSoundManager.instance.SetVolumeMusic(0);
        //Volume Sound Range(0, 1)
        CSSoundManager.instance.SetVolumeSound(0);
        //Play Sound --> SoundManager is in Preloader
        CSSoundManager.instance.PlaySound(0);
        //Play Music --> SoundManager is in Preloader
        CSSoundManager.instance.PlayMusic(0);
        //Play Looping Sound --> SoundManager is in Preloader add Sound in SoundSources
        CSSoundManager.instance.PlayLoopingSound(0);
        //Play Looping Music --> SoundManager is in Preloader add Music in MusicSources
        CSSoundManager.instance.PlayLoopingMusic(0);
        //Stop All Audio
        CSSoundManager.instance.StopAll();
        //Stop Sound 
        CSSoundManager.instance.StopSound(0);
        //Stop Music
        CSSoundManager.instance.StopMusic(0);

        //---GamePlay---
        //Current Level
        MainRoot.Instance.userConfig.currentLevel = 1;
        //Next Level
        MainRoot.Instance.userConfig.currentLevel++;
        //Count Levels in Game
        MainRoot.Instance.userConfig.maxLevels = 20;
        //Save Level
        LevelsSave.instance.LevelSave(2);
        //Rewrite Level
        LevelsSave.instance.LevelSaveAt(1, 2);
        //Remove Level
        LevelsSave.instance.RemoveLevel(2);
        //Display Variables
        Debug.Log(CSPlayerPrefs.GetFloat("TestVar"));
        Debug.Log(CSPlayerPrefs.GetVector3("TestVector"));
    }
    #endregion
}
