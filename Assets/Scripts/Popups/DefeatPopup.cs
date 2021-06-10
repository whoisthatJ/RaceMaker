using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class DefeatPopup : MonoBehaviour
{
	[SerializeField] private Slider timeSlider;

	[Space(10)]
	[SerializeField] private TextMeshProUGUI timerText;

	[Space(10)]
	[SerializeField] private Button continueAdsBtn;
	[SerializeField] private Button continueSoftBtn;

    [SerializeField] private GameObject progressBarPanel;
    [SerializeField] private Image progressBarImg;
    [SerializeField] private Image fillingBarImg;
    [SerializeField] private TextMeshProUGUI progressLevelTxt;
    [SerializeField] private TextMeshProUGUI progressExpTxt;
    //View buttons in popup defeat
    public void SetButtonsContinue(bool isContinueAds, bool isContinueSoft)
	{
		continueAdsBtn.gameObject.SetActive(isContinueAds);
		if (MainRoot.Instance.userConfig.softCurrency >= 500)
		continueSoftBtn.gameObject.SetActive(isContinueSoft);
		else continueSoftBtn.gameObject.SetActive(false);
	}
    public void SetLevelInstant()
    {
        MainRoot.Instance.userConfig.exp += GameManager.instance.score;
        while (MainRoot.Instance.userConfig.exp >= ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).exp)
        {
            MainRoot.Instance.userConfig.exp -= ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).exp;
            MainRoot.Instance.userConfig.softCurrency += ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).gold;
            MainRoot.Instance.userConfig.currentLevel++;
        }
    }
    public void SetLevel()
    {
        progressBarPanel.SetActive(true);
        progressLevelTxt.text = "Level " + MainRoot.Instance.userConfig.currentLevel;
        if (MainRoot.Instance.userConfig.currentLevel == ServiceXML.Instance.GetProgressLevels().Count)
        {
            progressBarImg.fillAmount = 1f;
            return;
        }
        progressExpTxt.text = MainRoot.Instance.userConfig.exp + "/" + ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).exp;
        progressBarImg.fillAmount = ((float)MainRoot.Instance.userConfig.exp / ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).exp);
        currentLevel = MainRoot.Instance.userConfig.currentLevel;
        currentExp  = MainRoot.Instance.userConfig.exp;
        MainRoot.Instance.userConfig.exp += GameManager.instance.score;
        while (MainRoot.Instance.userConfig.exp >= ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).exp)
        {
            MainRoot.Instance.userConfig.exp -= ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).exp;
            MainRoot.Instance.userConfig.softCurrency += ServiceXML.Instance.GetProgressLevelByLevel(MainRoot.Instance.userConfig.currentLevel).gold;
            MainRoot.Instance.userConfig.currentLevel++;
        }
        score = 0;
        adjust = 0;
        DOTween.To(() => score, x => score = x, GameManager.instance.score, GameManager.instance.score * 0.1f).OnUpdate(UpdateFillingBar);
    }
    int currentLevel;
    float currentExp;
    float score;
    float adjust;
    private void UpdateFillingBar()
    {
        if (currentExp + score - adjust >= ServiceXML.Instance.GetProgressLevelByLevel(currentLevel).exp)
        {
            adjust += ServiceXML.Instance.GetProgressLevelByLevel(currentLevel).exp - currentExp;
            currentExp = 0;
            currentLevel++;
            progressBarImg.fillAmount = 0f;
        }
        progressLevelTxt.text = "Level " + currentLevel;
        progressExpTxt.text = (currentExp + score - adjust) + "/" + ServiceXML.Instance.GetProgressLevelByLevel(currentLevel).exp;
        fillingBarImg.fillAmount = ((float)(currentExp + score - adjust) / ServiceXML.Instance.GetProgressLevelByLevel(currentLevel).exp);
    }
    public void SetTimerEnable(bool isFlag)
	{
		timeSlider.gameObject.SetActive(isFlag);
		timerText.gameObject.SetActive(isFlag);
	}

	#region Continue ADS
	public void ContinueAdsBtnClik()
	{
		//Replay Ads	
		if (ServiceFirebaseAnalytics.Instance.GetAdsRate() > 0)
		{
		    if (!ServiceIronSource.Instance.CallShowRewardedVideo(WatchAdCallback))
		    {
                FinishPopup.instance.ShowPopUp(Lean.Localization.LeanLocalization.GetTranslationText("NoAds"));
            }          
		}       
		else
		{
		    WatchAdCallback(true);
		}
	} 

	private void WatchAdCallback(bool success)
	{
		if (success)
		{
            GameManager.instance.Continue();
			FinishPopup.isContinue = true;
            FinishPopup.instance.ClosePanel();
            FinishPopup.continueOpen?.Invoke();

            ServiceGameAnalytics.Instance.LogRewardedAdsWatched("continue");
            ServiceFirebaseAnalytics.Instance.LogRewardedAdsWatched("continue");
        }
	}
	#endregion

	#region Continue Soft
	public void ContinueSoftBtnClick()
    {
		//Replay Soft Currency
		if (MainRoot.Instance.userConfig.softCurrency >= 500)
		{
			GameManager.instance.Continue();
			FinishPopup.isContinue = true;
			FinishPopup.continueOpen?.Invoke();
			FinishPopup.instance.ClosePanel();
			MainRoot.Instance.userConfig.softCurrency -= 500;
			ServiceGameAnalytics.Instance.LogSoftCurrencyRematch();
			ServiceFirebaseAnalytics.Instance.LogSoftCurrencyRematch();
		}
    }
	#endregion

	private void OnEnable()
	{
		continueAdsBtn.onClick.AddListener(ContinueAdsBtnClik);
		continueSoftBtn.onClick.AddListener(ContinueSoftBtnClick);
	}

	private void OnDisable()
	{
		continueAdsBtn.onClick.RemoveAllListeners();
		continueSoftBtn.onClick.RemoveAllListeners();
	}

	//Update timer slider for continue buttons
	public void SetSliderTimer(float timer, bool isTextTimer = true, bool isSlider = true)
	{
		if (isSlider)
		{
            progressBarPanel.SetActive(false);
			currentTimer = timer;
			timeSlider.value = timeSlider.maxValue;
			Sequence timerUpdate = DOTween.Sequence().SetUpdate(true);
			timerUpdate.Append(timeSlider.DOValue(0, timer).SetEase(Ease.Linear).OnComplete(() =>
			{
				timeSlider.gameObject.SetActive(false);
				continueAdsBtn.gameObject.SetActive(false);
				continueSoftBtn.gameObject.SetActive(false);
				timerText.gameObject.SetActive(false);
			}).OnStart(() =>
			{
				if (isTextTimer)
					timerUpdate.OnUpdate(TweenCallback);
				else
					timerText.gameObject.SetActive(false);
			}));
		}
		else
		{
			timeSlider.gameObject.SetActive(false);
			timerText.gameObject.SetActive(false);
		}
	}

	private float currentTimer;
	private void TweenCallback()
	{
		currentTimer -= Time.deltaTime;
		timerText.text = Mathf.RoundToInt(currentTimer).ToString();
	}
}
