using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
	public static MainMenuManager instance;

	[Header("---UI---")]
    [SerializeField] GameObject panel;
    [SerializeField] Button startGameBtn;
	[SerializeField] Button levelsBtn;
	[SerializeField] Button customizeBtn;
	[SerializeField] Button freeGiftBtn;
	[SerializeField] Button rewardBtn;
	[SerializeField] Button otherGamesBtn;
	[SerializeField] Button otherGamesDirectBtn;
    [SerializeField] Image progressBarImg;
    [SerializeField] TextMeshProUGUI progressLevelTxt;
    [SerializeField] TextMeshProUGUI progressExpTxt;
    [SerializeField] DOTweenVisualManager containerDTManager;

    [Header("Free Gift Button sprites")]
	[SerializeField] Sprite giftReadySprite;
	[SerializeField] Sprite giftNotReadySprite;


	[Header("---Popups---")]
	[SerializeField] OtherGamesPopup otherGamesPopup;

	private void Awake()
	{
		instance = this;
	}

	private void OnEnable()
	{
		InitListeners();
	}

	private void OnDisable()
	{
		RemoveListeners();
	}
	
	// Use this for initialization
	private IEnumerator Start()
	{
		if (FreeGiftManager.Instance.isReadyToClaim)
			GiftReady();
		else
			GiftClaimed();
		InitAudio();
		FinishPopup.isContinue = false;

        progressLevelTxt.text = "Level " + MainRoot.Instance.userConfig.currentLevel;
        progressExpTxt.text = MainRoot.Instance.userConfig.exp + "/" + ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).exp;
        progressBarImg.fillAmount = ((float)MainRoot.Instance.userConfig.exp / ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).exp);

        yield return new WaitForSeconds(2f);
		//Analytics
		Firebase.Analytics.FirebaseAnalytics.SetCurrentScreen("MainMenu", "Menu");
	}    

	private void InitListeners()
	{
		FreeGiftManager.GiftReady += GiftReady;
		FreeGiftManager.GiftClaimed += GiftClaimed;
		FreeGiftManager.TimerChanged += UpdateGiftTimer;

		startGameBtn.onClick.AddListener(StartNewGame);
		levelsBtn.onClick.AddListener(OpenLevelsScene);
		freeGiftBtn.onClick.AddListener(OpenFreeGiftPanel);
		customizeBtn.onClick.AddListener(OpenCustomizePanel);
		otherGamesBtn.onClick.AddListener(OpenOtherGamesPanel);
		otherGamesDirectBtn.onClick.AddListener(OpenOtherGamesDirect);
		rewardBtn.onClick.AddListener(OpenRewardPanel);
	}

	private void RemoveListeners()
	{
		FreeGiftManager.GiftReady -= GiftReady;
		FreeGiftManager.GiftClaimed -= GiftClaimed;
		FreeGiftManager.TimerChanged -= UpdateGiftTimer;

		startGameBtn.onClick.RemoveAllListeners();
		levelsBtn.onClick.RemoveAllListeners();
		freeGiftBtn.onClick.RemoveAllListeners();
		customizeBtn.onClick.RemoveAllListeners();
		otherGamesBtn.onClick.RemoveAllListeners();
		otherGamesDirectBtn.onClick.RemoveAllListeners();
		rewardBtn.onClick.RemoveAllListeners();
	}
    public void PanelSetActive(bool active)
    {
        //panel.SetActive(active);
        if (active)
        {
            DOTween.Rewind("2001");
            DOTween.Rewind("2002");
            progressLevelTxt.text = "Level " + MainRoot.Instance.userConfig.currentLevel;
            progressExpTxt.text = MainRoot.Instance.userConfig.exp + "/" + ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).exp;
            progressBarImg.fillAmount = ((float)MainRoot.Instance.userConfig.exp / ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).exp);
        }
        else
        {
            DOTween.Play("2001");
            DOTween.Play("2002");
        }
        ServiceIronSource.Instance.CallShowInterstitial();
    }
	private void StartNewGame()
	{
		Firebase.Analytics.FirebaseAnalytics.SetCurrentScreen("Game", "Game");
        //Preloader.Instance.LoadNewScene("Game");
        PanelSetActive(false);
        //GameManager.instance.InitGame();
        GameManager.instance.StartGame();
        TopBar.instance.PlayerContinue();
    }

	private void OpenLevelsScene()
	{
		Firebase.Analytics.FirebaseAnalytics.SetCurrentScreen("Levels", "Levels");
		Preloader.Instance.LoadNewScene("Levels");
	}

	private void OpenFreeGiftPanel()
	{
		FreeGiftManager.Instance.OpenFreeGiftPanel();
	}

	private void OpenCustomizePanel()
	{
		Firebase.Analytics.FirebaseAnalytics.SetCurrentScreen("Customize", "Menu");
		CustomizationUI.Instance.OpenCustomizationPanel();
	}

	private void OpenOtherGamesPanel()
	{
		otherGamesPopup.OpenPanel();
	}

	private void OpenOtherGamesDirect()
	{
		Application.OpenURL("market://details?id=kz.snailwhale.soccer");
	}

	private void OpenRewardPanel()
	{
		DailyRewardManager.Instance.OpenPanel();
	}

	private void GiftReady()
	{
		freeGiftBtn.GetComponent<Image>().sprite = giftReadySprite;
		freeGiftBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Free Gift";
	}

	private void GiftClaimed()
	{
		freeGiftBtn.GetComponent<Image>().sprite = giftNotReadySprite;
	}

	private void UpdateGiftTimer(string time)
	{
		freeGiftBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = time;
	}

	private void OnApplicationFocus(bool isFocus)
	{
		if (!isFocus)
		{
			ServiceFirebaseAnalytics.Instance.LogEnableMusic(Convert.ToInt32((MainRoot.Instance.userConfig.isMusic)));
			ServiceFirebaseAnalytics.Instance.LogEnableSound(Convert.ToInt32((MainRoot.Instance.userConfig.isSound)));
			ServiceGameAnalytics.Instance.LogEnableMusic(Convert.ToInt32((MainRoot.Instance.userConfig.isMusic)));
			ServiceGameAnalytics.Instance.LogEnableMusic(Convert.ToInt32((MainRoot.Instance.userConfig.isSound)));
		}
	}

	#region Audio
	private void InitAudio()
	{
		//CSSoundManager.instance.StopMusic(1);
		//CSSoundManager.instance.PlayLoopingMusic(1);
	}
	#endregion
}
