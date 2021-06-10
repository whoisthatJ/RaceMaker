using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {
    public Vector3 currentDest;
    public Vector3 currentDestNextLevel = Vector3.back;
    public float speed = 1f;
    public float stableSpeed;
    public float speedLevel = 1f;
    //public List<Transform> points = new List<Transform>();
    public GameObject blowParticle;
    public GameObject trailParticle;
    public GameObject smokeParticle;
    public SpriteRenderer mainSprite;
    Vector3 lastPoint;
    bool hor;
    bool vert;
    public List<Vector3> bezierPoints = new List<Vector3>();
    
    // Use this for initialization
    void Start () {
        stableSpeed = 0.5f;
        lastPoint = transform.position;
        vert = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (bezierPoints.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, bezierPoints[0], speed * Time.deltaTime);
            if (transform.position.Equals(bezierPoints[0]))
            {
                lastPoint = bezierPoints[0];
                bezierPoints.RemoveAt(0);

            }
            else
            {
                Vector3 vectorToTarget = bezierPoints[0] - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10f * speed);
            }
            if (bezierPoints.Count == 0)
            {
                blowParticle.SetActive(true);
                GameManager.instance.GameOver("time");
                mainSprite.color = Color.black;
                CSSoundManager.instance.PlaySound(1);
            }
        }
       
        if (currentDest != Vector3.back)
        {
            if (bezierPoints.Contains(currentDest))
            {
                speed = 10f;
                GameManager.instance.EnablePathButton(false);
                updateSpeed = false;
            }
            else
            {
                if (!updateSpeed)
                {
                    stableSpeed =  distance / 13f * speedLevel;
                    
                    updateSpeed = true;
                    GameManager.instance.SetTimeBar();
                    time = 0f;
                    //Debug.Log(stableSpeed + " " + pointsCount / 14f + " " + speedLevel);
                }
                speed = stableSpeed;
                GameManager.instance.EnablePathButton(true);
                currentDest = Vector3.back;
            }
        }
        if (currentDestNextLevel != Vector3.back)
        {
            if (bezierPoints.Contains(currentDestNextLevel))
            {
                speed = 7.5f;
                GameManager.instance.EnablePathButton(false);
                updateSpeed = false;
            }
            else
            {
                if (!updateSpeed)
                {
                    stableSpeed = distance / 13f * speedLevel;
                    updateSpeed = true;
                    GameManager.instance.SetTimeBar();
                    time = 0f;                    
                }
                speed = stableSpeed;
                GameManager.instance.EnablePathButton(true);
                currentDestNextLevel = Vector3.back;
                GameManager.instance.cameraFollow.SetFollowPlayer(false);
                levelUp = false;
                GameManager.instance.SetRoadAfterLevelUp();
            }
        }
        if (levelUp && transform.position == playerTarget)
        {
            GameManager.instance.cameraFollow.SetFollowPlayer(true, GameManager.instance.pathSquares[3].gameObject);
            CSSoundManager.instance.PlaySound(3);
        }
        time += Time.deltaTime;
    }
    
    public int pointsCount;
    public float distance;
    public void UpdateSpeed()
    {
        stableSpeed = distance / 13f * speedLevel;
        updateSpeed = true;
        GameManager.instance.SetTimeBar();
        time = 0f;
        speed = stableSpeed;
        mainSprite.color = Color.white;
    }
    public void NextLevel(Vector3 nextLevelPoint)
    {
        currentDestNextLevel = nextLevelPoint;
    }
    public void BezierPoints(List<Transform> points, Vector3 nextSquare)
    {
        distance = 0f;
        pointsCount = points.Count;
        bezierPoints.Add(points[0].position);
        if (points.Count > 2)
        {
            for (int i = 1; i < points.Count - 1; i++)
            {
                if (Mathf.Round(points[i + 1].position.x) != Mathf.Round(points[i - 1].position.x) && Mathf.Round(points[i + 1].position.y) != Mathf.Round(points[i - 1].position.y))
                {
                    Vector3 mult = points[i + 1].position - points[i - 1].position;
                    Vector3 startPoint = (points[i - 1].position + points[i].position) * 0.5f;
                    for (int j = 0; j < 5; j++)
                    {
                        Vector3 radialPoint;
                        if (Mathf.Round(points[i - 1].position.x) == Mathf.Round(points[i].position.x))
                            radialPoint = startPoint + 0.5f * new Vector3((Mathf.Cos(Mathf.PI / 180f * (180f - j * 22.5f)) + 1f) * mult.x, Mathf.Sin(Mathf.PI / 180f * (180f - j * 22.5f)) * mult.y, 0f);
                        else
                            radialPoint = startPoint + 0.5f * new Vector3((Mathf.Cos(Mathf.PI / 180f * (-90f + j * 22.5f))) * mult.x, (Mathf.Sin(Mathf.PI / 180f * (-90f + j * 22.5f)) + 1f) * mult.y, 0f);
                        distance += Vector3.Distance(bezierPoints[bezierPoints.Count - 1], radialPoint);
                        bezierPoints.Add(radialPoint);
                    }
                }
                else
                {
                    distance += Vector3.Distance(bezierPoints[bezierPoints.Count - 1], points[i].position);
                    bezierPoints.Add(points[i].position);
                }
            }
        }
        distance += Vector3.Distance(bezierPoints[bezierPoints.Count - 1], points[points.Count - 1].position);
        bezierPoints.Add(points[points.Count - 1].position);
        nextCurrentDest = (points[points.Count - 1].position + nextSquare) * 0.5f;
        if (GameManager.instance.pathSquares.Count > 1)
        {
            distance += Vector3.Distance(bezierPoints[bezierPoints.Count - 1], (points[points.Count - 1].position + nextSquare) * 0.5f);
            bezierPoints.Add((points[points.Count - 1].position + nextSquare) * 0.5f);
        }
    }
    public void SpeedUpCarToDest()
    {
        currentDest = nextCurrentDest;
    }
    Vector3 nextCurrentDest;
    float time;
    public bool updateSpeed;
    public Vector3 playerTarget;
    public bool levelUp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.lost)
        {
            speed = 0;
            blowParticle.SetActive(true);
            GameManager.instance.GameOver("wrong");
            mainSprite.color = Color.black;
            CSSoundManager.instance.PlaySound(1);
        }
    }
    public void InitCar()
    {
        bezierPoints = new List<Vector3>();
        speed = 0f;
        speedLevel = 1f;
        transform.position = new Vector3(0f, -10f, 0f);
        trailParticle.SetActive(false);
        bezierPoints.Add(GameManager.instance.pathSquares[0].currentTrail.transforms[0].transform.position + Vector3.down * 0.5f);
        mainSprite.color = Color.white;
        transform.eulerAngles = new Vector3(0f, 0f, 90f);
        StartCoroutine(InitParticles());
    }
    public void Lose()
    {
        speed = 0;
        blowParticle.SetActive(true);
        GameManager.instance.GameOver();
        mainSprite.color = Color.black;
    }
    public void ClearParticle()
    {
        StartCoroutine(ClearParticles());
    }
    public void EnableParticle()
    {
        trailParticle.SetActive(true);
        smokeParticle.SetActive(true);
    }
    IEnumerator ClearParticles()
    {
        yield return new WaitForSeconds(2f);
        blowParticle.SetActive(false);
        trailParticle.SetActive(false);
        smokeParticle.SetActive(false);
    }
    IEnumerator InitParticles()
    {
        yield return new WaitForEndOfFrame();
        EnableParticle();
    }
}
