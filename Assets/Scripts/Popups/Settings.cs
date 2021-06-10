using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public static Settings Instance;

    [SerializeField] private GameObject settingsPnl;
    [SerializeField] private Button GooglePlayBtn;
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button removeAdsBtn;
    [SerializeField] private Button restorePurchasesBtn;
    [SerializeField] private Button backBtn;

    private void Awake()
    {
        Instance = this;
    }

	private void Start()
	{
		SetEnableButton();
	}

	private void OnEnable()
    {
        Preloader.BackButtonPressed += Back;
        GooglePlayBtn.onClick.AddListener(GooglePlay);
        soundBtn.onClick.AddListener(Sound);
        musicBtn.onClick.AddListener(Music);
        removeAdsBtn.onClick.AddListener(RemoveAds);
        restorePurchasesBtn.onClick.AddListener(RestorePurchases);
        backBtn.onClick.AddListener(Back);
    }

    private void OnDisable()
    {
        Preloader.BackButtonPressed -= Back;
        GooglePlayBtn.onClick.RemoveAllListeners();
        soundBtn.onClick.RemoveAllListeners();
        musicBtn.onClick.RemoveAllListeners();
        removeAdsBtn.onClick.RemoveAllListeners();
        restorePurchasesBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
    }

    public void OpenSettings()
    {
        settingsPnl.SetActive(true);
        Preloader.Instance.AddPanelToTheList(settingsPnl);
		Firebase.Analytics.FirebaseAnalytics.SetCurrentScreen("Settings", "Menu");
    }

    private void GooglePlay()
    {

    }

    private void Sound()
    {
        if (MainRoot.Instance.userConfig.isSound)
        {
            MainRoot.Instance.userConfig.isSound = false;
			CSSoundManager.instance.SetVolumeSound(0);
        }
        else
        {
            MainRoot.Instance.userConfig.isSound = true;
			CSSoundManager.instance.SetVolumeSound(1);
        }
		SetEnableButton();
    }

    private void Music()
    {
        if (MainRoot.Instance.userConfig.isMusic)
        {                                  
            MainRoot.Instance.userConfig.isMusic = false;
			CSSoundManager.instance.SetVolumeMusic(0);
        }                                  
        else                               
        {                                  
            MainRoot.Instance.userConfig.isMusic = true;
			CSSoundManager.instance.SetVolumeMusic(1);
        }
		SetEnableButton();
    }

    private void SetEnableButton()
	{
		if (MainRoot.Instance.userConfig.isSound)
			soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
		else
			soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
		if (MainRoot.Instance.userConfig.isMusic)
			musicBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        else
			musicBtn.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
	}

    private void RemoveAds()
    {

    }

    private void RestorePurchases()
    {

    }

    private void Back()
    {
        settingsPnl.SetActive(false);
        Preloader.Instance.RemovePanelFromTheList(settingsPnl);
		Firebase.Analytics.FirebaseAnalytics.SetCurrentScreen("MainMenu", "Menu");
    }
}
