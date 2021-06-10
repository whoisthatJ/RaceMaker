using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FreeGiftManager : MonoBehaviour
{
    public static FreeGiftManager Instance;

    public delegate void TimerChangedDelegate(string time);
    public static event TimerChangedDelegate TimerChanged;
    public delegate void GiftDelegate();
    public static event GiftDelegate GiftReady, GiftClaimed;

    [SerializeField] double cooldown = 30;
    
    [SerializeField] GameObject pnlFreeGift;
    [SerializeField] Button btnClaim;
    [SerializeField] Button btnDoubleReward;
    [SerializeField] GameObject adLoading;
    [SerializeField] TextMeshProUGUI txtGift;
	[SerializeField] TextMeshProUGUI doubleRewardTxt;
    
	private int factor = 2;
    
    TimeSpan timeToClaim;
    public bool isReadyToClaim { get; private set; }
    int[] rewards = { 25, 50, 75, 100 };

    private void Awake()
    {
        Instance = this;
        if (MainRoot.Instance.userConfig.lastClaimedFreeGift + TimeSpan.FromMinutes(cooldown) > DateTime.Now)
            isReadyToClaim = true;
        else
            isReadyToClaim = false;

        //if (!ServiceIronSource.Instance.CheckRewardBasedVideo())
        //{
        //    AdLoading();
        //}
    }

    private void Start()
    {
        if (MainRoot.Instance.userConfig.lastClaimedFreeGift + TimeSpan.FromMinutes(cooldown) > DateTime.Now)
            StartCoroutine(StartTimer());
        else
            ReadyToClaim();
    }

    void OnEnable()
    {
        Preloader.BackButtonPressed += ClosePanel;
        btnClaim.onClick.AddListener(Claim);
        btnDoubleReward.onClick.AddListener(WatchAd);
    }
    
    void OnDisable()
    {
        Preloader.BackButtonPressed -= ClosePanel;
        btnClaim.onClick.RemoveAllListeners();
        btnDoubleReward.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }

    public void OpenFreeGiftPanel()
    {
        if (isReadyToClaim)
        {
            if (true)//ServiceIronSource.Instance.CheckRewardBasedVideo())
                btnDoubleReward.gameObject.SetActive(true);
            if (MainRoot.Instance.userConfig.freeGiftDoubled)
            {
                btnDoubleReward.gameObject.SetActive(false);
            }
            pnlFreeGift.SetActive(true);
            doubleRewardTxt.text = "x" + GetFactor(MainRoot.Instance.userConfig.freeGiftValue);
            txtGift.text = MainRoot.Instance.userConfig.freeGiftValue.ToString() + " <sprite name=Coin> ";
            Preloader.Instance.AddPanelToTheList(pnlFreeGift);
        }
        else
        {
            Notification.Instance.Open(Lean.Localization.LeanLocalization.GetTranslationText("GiftNotReady"));
        }
    }

    IEnumerator StartTimer()
    {
        DateTime nextRewardTime = MainRoot.Instance.userConfig.lastClaimedFreeGift + TimeSpan.FromMinutes(cooldown);
        timeToClaim = nextRewardTime - DateTime.Now - TimeSpan.FromSeconds(1);
        while (timeToClaim.TotalSeconds > 0)
        {
            UpdateUI(2);
            yield return new WaitForSeconds(1);
            timeToClaim = nextRewardTime - DateTime.Now;
        }
        ReadyToClaim();
    }

    void ReadyToClaim()
    {
        UpdateUI(1);
        MainRoot.Instance.userConfig.freeGiftDoubled = false;
    }

    public void Claim()
    {
        MainRoot.Instance.userConfig.softCurrency += MainRoot.Instance.userConfig.freeGiftValue;
        if (GiftClaimed != null)
            GiftClaimed();
        MainRoot.Instance.userConfig.lastClaimedFreeGift = DateTime.Now;
        StartCoroutine(StartTimer());
        pnlFreeGift.SetActive(false);
		int randomResult = UnityEngine.Random.Range(0, rewards.Length);
        MainRoot.Instance.userConfig.freeGiftValue = rewards[randomResult];
        MainRoot.Instance.userConfig.freeGiftDoubled = false;
        Preloader.Instance.RemovePanelFromTheList(pnlFreeGift);
        //Analytics
		ServiceFirebaseAnalytics.Instance.LogClaimFreeGift();
		ServiceGameAnalytics.Instance.LogClaimFreeGift();
    }

    void UpdateUI(int state)
    {
        if (state == 1)
        {
            //actions to do when ready to claim
            isReadyToClaim = true;
            if (GiftReady != null)
                GiftReady();
        }
        else if (state == 2)
        {
            //actions to do when NOT ready to claim
            isReadyToClaim = false;
            if (TimerChanged!=null)
                TimerChanged(String.Format("{0:00}:{1:00}", timeToClaim.Minutes, timeToClaim.Seconds));
        }
    }

    private void WatchAd()
    {
        //if (!ServiceIronSource.Instance.CallShowRewardedVideo(WatchAdCallback))
            //Notification.Instance.Open(Lean.Localization.LeanLocalization.GetTranslationText("No Ads"));
        WatchAdCallback(true);
    }

    private void AdLoadedCallback()
    {
        if (btnDoubleReward != null && !MainRoot.Instance.userConfig.freeGiftDoubled)
            btnDoubleReward.gameObject.SetActive(true);
        if (adLoading != null)
            adLoading.SetActive(false);
    }

    private void AdLoading()
    {
        if (btnDoubleReward != null)
            btnDoubleReward.gameObject.SetActive(false);
        if (adLoading != null && !MainRoot.Instance.userConfig.freeGiftDoubled)
            adLoading.SetActive(true);
    }
    
    private void WatchAdCallback(bool success)
    {
        if (success)
        {
			GetFactor(MainRoot.Instance.userConfig.freeGiftValue);
			txtGift.text = " " + MainRoot.Instance.userConfig.freeGiftValue.ToString() +  " <sprite name=Coin> " + "\n+" +  
				(MainRoot.Instance.userConfig.freeGiftValue * factor - MainRoot.Instance.userConfig.freeGiftValue).ToString() + " <sprite name=Coin> ";
			MainRoot.Instance.userConfig.freeGiftValue *= factor;
            btnDoubleReward.gameObject.SetActive(false);
            MainRoot.Instance.userConfig.freeGiftDoubled = true;
			//Analytics
			ServiceFirebaseAnalytics.Instance.LogDoubleClaimFreeGift();
			//ServiceFirebaseAnalytics.Instance.LogRewardedAdsWatched("");
			//ServiceGameAnalytics.Instance.LogRewardedAdsWatched("");
			ServiceGameAnalytics.Instance.LogDoubleClaimFreeGift();
        }
    }

    private int GetFactor(int value)
	{
		switch(value)
		{
			case 25:
				factor = 8;
				break;
			case 50:
				factor = 4;
                break;
			case 75:
				factor = 3;
                break;
			case 100:
				factor = 3;
                break;
			default:
				factor = 2;
                break;
		}
		return factor;
	}

    private void ClosePanel()
    {
        pnlFreeGift.SetActive(false);
        Preloader.Instance.RemovePanelFromTheList(pnlFreeGift);
    }
}