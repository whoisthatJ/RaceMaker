using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Spine.Unity;

public class Preloader : MonoBehaviour
{
    public static Preloader Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField] string loadingSceneName;
    [SerializeField] GameObject preloaderCanvas;
    [SerializeField] Slider sliderBar;
    [SerializeField] bool skipTutorial;
    [SerializeField] Image sceneSplash;
    [SerializeField] Sprite[] splashSprites;

    public delegate void BackButtonDelegate();
    public static event BackButtonDelegate BackButtonPressed; //<- event called when the back button is pressed
    private List<GameObject> openedPanels = new List<GameObject>(); //<- list stores all panels that are currently opened
    private bool firstLoad;
    string currentScene;

    void Start()
    {
        if (skipTutorial)
        {
            PlayerPrefs.SetInt("tutor", 1);
        }
        if (PlayerPrefs.GetInt("tutor") == 0)
        {
            LoadNewScene("Game");
        }
        else
        {
            LoadNewScene(loadingSceneName);
        }
    }

    public void LoadNewScene(string sceneName)
    {
        openedPanels.Clear();
        StartCoroutine(LoadNewSceneCor(sceneName));
        currentScene = sceneName;
    }
    bool nextTimes;
    IEnumerator LoadNewSceneCor(string sceneName)
    {
        preloaderCanvas.SetActive(true);
        sliderBar.value = 0f;

        if (nextTimes)
        {
			if (sceneSplash != null)
            sceneSplash.gameObject.SetActive(true);
			if (splashSprites.Length > 0)
			{
				lastSplashIndex = Random.Range(0, splashSprites.Length);
				sceneSplash.sprite = splashSprites[lastSplashIndex];
				lastSplashSprite = splashSprites[lastSplashIndex];
			}
        }      
		yield return new WaitForSeconds(.223f);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            sliderBar.value = progress;
            yield return null;
        }
        yield return new WaitForSeconds(.8f);

        preloaderCanvas.SetActive(false);
        nextTimes = true;
        if (!firstLoad)
        {
            InitAudio();
            firstLoad = true;
        }
    }

    private int lastSplashIndex; 
    public Sprite lastSplashSprite;
    public string GetCurrentScene()
    {
        return currentScene;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) HardWareBtnClick();
    }
    #region Audio
    private void InitAudio()
    {
        //CSSoundManager.instance.StopMusic(1);
        CSSoundManager.instance.PlayLoopingMusic(1);
    }
    #endregion
    #region Quit Panel
    //call this when opening a new panel
    public void AddPanelToTheList(GameObject panel)
    {
        openedPanels.Add(panel);
    }
    //call this when closing a panel
    public void RemovePanelFromTheList(GameObject panel)
    {
        if (openedPanels.Contains(panel))
            openedPanels.Remove(panel);
    }

    private void HardWareBtnClick()
    {
        //closes opened panels if the list is not empty
        if (openedPanels.Count > 0)
        {
            openedPanels.Clear();
            if (BackButtonPressed != null)
                BackButtonPressed();
        }
        else
        {
            //quits the game, pauses or returns to main menu
            if (currentScene == "Menu")
                Confirmation.Instance.Open("Quit?", YesQuitClick);
            if (currentScene == "Game")
            {
                if(!GameManager.instance.gameStarted)
                    Confirmation.Instance.Open("Quit?", YesQuitClick);
                else if (BackButtonPressed != null)
                    BackButtonPressed();
            }
            else
                LoadNewScene("Menu");
        }
    }

    private void YesQuitClick()
    {
        Application.Quit();
    }
    #endregion
}
