using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FinishPopup : MonoBehaviour
{
    public static FinishPopup instance;

    public static bool isContinue;

    [Header("---Popups---")]
    public CompletePopup completePopup;
    public DefeatPopup defeatPopup;
    public PopUpUI popUpUI;


    [Header("---UI---")]
    [SerializeField] private Button homeBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button shareBtn;
    [SerializeField] private Button freeGiftBtn;
    [SerializeField] private Button customizeBtn;
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Button restartGameBtn;

    [Space(10)]
    [SerializeField] private GameObject container;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI title;

    [Header("---Defeat Popup---")]
    //View elements in popup defeat
    [SerializeField] private bool isSlider;
    [SerializeField] private bool isTextTimer;
    [SerializeField] private bool isContinueAds;
    [SerializeField] private bool isContinueSoft;

    [Header("---Settings Finish Popup---")]
    [Tooltip("Time show buttons in finish popup")]
    [SerializeField] private float timeShowButtons = 2;
    [SerializeField] private float timeShowRestart = 1.5f;

    //Events
    public delegate void CompleteEvent();
    public static CompleteEvent completeOpen;

    public delegate void DefeatEvent();
    public static DefeatEvent defeatOpen;

    public delegate void ContinueEvent();
    public static ContinueEvent continueOpen;

    public delegate void RestartButtonClick();
    public static RestartButtonClick restartButtonClick;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void OnEnable()
    {
        InitListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void InitListeners()
    {
        FreeGiftManager.GiftReady += TurnFreeGiftButtonOn;
        FreeGiftManager.GiftClaimed += TurnFreeGiftButtonOff;
        homeBtn.onClick.AddListener(BackMenuBtnClick);
        settingBtn.onClick.AddListener(OpenSettingPanel);
        freeGiftBtn.onClick.AddListener(OpenFreeGiftPanel);
        customizeBtn.onClick.AddListener(OpenCustomizePanel);
        nextLevelBtn.onClick.AddListener(NextLevelClick);
        restartGameBtn.onClick.AddListener(RestartGameClick);
    }

    private void RemoveListeners()
    {
        FreeGiftManager.GiftReady -= TurnFreeGiftButtonOn;
        FreeGiftManager.GiftClaimed -= TurnFreeGiftButtonOff;
        homeBtn.onClick.RemoveAllListeners();
        settingBtn.onClick.RemoveAllListeners();
        freeGiftBtn.onClick.RemoveAllListeners();
        customizeBtn.onClick.RemoveAllListeners();
        nextLevelBtn.onClick.RemoveAllListeners();
        restartGameBtn.onClick.RemoveAllListeners();
    }

    public void OpenPanel(string titleText, float timeDelay, bool isComplete = false, bool highScore = false, int stars = 0, Action action = null)
    {
        title.text = titleText;
        switch (isComplete)
        {
            case true:
                StartCoroutine(OpenPanelComplete(timeDelay, stars));
                break;
            case false:
                StartCoroutine(OpenPanelDefeat(timeDelay, highScore));
                break;
        }
        action?.Invoke();
    }
    public void ClosePanel()
    {
        container.SetActive(false);
        defeatPopup.gameObject.SetActive(false);
    }
    public void ShowPopUp(string text)
    {
        popUpUI.Open(text);
    }
    bool expGained;
    private void ShowButtonsInstant()
    {
        container.SetActive(true);
        freeGiftBtn.gameObject.SetActive(false);
        shareBtn.gameObject.SetActive(true);
        restartGameBtn.gameObject.SetActive(true);
        //settingBtn.gameObject.SetActive(true);
        defeatPopup.SetLevel();
        expGained = true;
    }
    private IEnumerator ShowButtons(float time)
    {
        expGained = false;
        container.SetActive(true);
        yield return new WaitForSecondsRealtime(time);
        //homeBtn.gameObject.SetActive(true);
        /*if (!FreeGiftManager.Instance.isReadyToClaim)
			freeGiftBtn.gameObject.SetActive(true);
		else*/
        freeGiftBtn.gameObject.SetActive(false);
        //customizeBtn.gameObject.SetActive(true);
        shareBtn.gameObject.SetActive(true);
        restartGameBtn.gameObject.SetActive(true);
        //settingBtn.gameObject.SetActive(true);
        if (!isContinue)
        {
            defeatPopup.SetLevel();
            expGained = true;
        }
    }

    private void BackMenuBtnClick()
    {
        MainMenuManager.instance.PanelSetActive(true);
        container.SetActive(false);
        GameManager.instance.InitGame();
        //Preloader.Instance.LoadNewScene("Menu");
    }

    private void OpenSettingPanel()
    {
        Settings.Instance.OpenSettings();
    }

    private void OpenFreeGiftPanel()
    {
        FreeGiftManager.Instance.OpenFreeGiftPanel();
    }

    private void OpenCustomizePanel()
    {
        CustomizationUI.Instance.OpenCustomizationPanel();
    }

    private void RestartGameClick()
    {
        restartButtonClick?.Invoke();
        //Preloader.Instance.LoadNewScene("Game");
        isContinue = false;
        if (!expGained)
            defeatPopup.SetLevelInstant();
        GameManager.instance.InitGame();
        MainMenuManager.instance.PanelSetActive(true);
        //GameManager.instance.StartGame();
        container.SetActive(false);
        //TopBar.instance.PlayerContinue();
    }

    #region CompletePanel
    private IEnumerator OpenPanelComplete(float timeDelay, int stars = 0)
    {
        yield return new WaitForSeconds(timeDelay);
        completeOpen?.Invoke();
        StartCoroutine(ShowButtons(0));
        completePopup.gameObject.SetActive(true);
        completePopup.SetStars(stars);
        title.gameObject.SetActive(true);
        //if Game Contains Levels
        //nextLevelBtn.gameObject.SetActive(true);
        //LevelsSave.instance.LevelSaveAt(MainRoot.Instance.userConfig.currentLevel, stars);
    }

    private void NextLevelClick()
    {
        //Load next level button click
        MainRoot.Instance.userConfig.currentLevel++;
        Preloader.Instance.LoadNewScene("Game");
    }
    #endregion

    #region DefeatPanel
    private IEnumerator OpenPanelDefeat(float timeDelay, bool highScore)
    {
        yield return new WaitForSeconds(timeDelay);
        defeatOpen?.Invoke();
        if (highScore)
            completeOpen?.Invoke();
        defeatPopup.gameObject.SetActive(true);
        if (!isContinue && GameManager.instance.score > 0)
        {
            StartCoroutine(ShowButtons(timeShowButtons));
            defeatPopup.SetSliderTimer(timeShowButtons, isTextTimer, isSlider);
            defeatPopup.SetButtonsContinue(isContinueAds, isContinueSoft);
            defeatPopup.SetTimerEnable(true);
            restartGameBtn.gameObject.SetActive(false);
            StartCoroutine(TurnRestartButtonOn());
        }
        else
        {
            ShowButtonsInstant();
            defeatPopup.SetButtonsContinue(false, false);
            defeatPopup.SetTimerEnable(false);
        }

        title.gameObject.SetActive(true);
    }
    #endregion
    IEnumerator TurnRestartButtonOn()
    {
        yield return new WaitForSecondsRealtime(timeShowRestart);
        restartGameBtn.gameObject.SetActive(true);
    }
    private void TurnFreeGiftButtonOn()
    {
        //freeGiftBtn.gameObject.SetActive(true);
    }

    private void TurnFreeGiftButtonOff()
    {
        freeGiftBtn.gameObject.SetActive(false);
    }

    #region Custom Call Methods
#if UNITY_EDITOR
    [SHOW_IN_HIER]
    public void CSVictory()
    {
        //OpenPanel("Victory", 1, true, 3);
    }
    [SHOW_IN_HIER]
    public void CSDefeat()
    {
        //OpenPanel("Defeat", 1, false, 0);
    }

    [SHOW_IN_HIER]
    public void PopUp()
    {
        ShowPopUp("Test");
    }
#endif
    #endregion
}