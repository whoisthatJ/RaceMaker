using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceIronSource : MonoBehaviour
{

    public static ServiceIronSource Instance
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

    private float deltaTime = 0.0f;
    private static string outputMessage = string.Empty;
    private System.DateTime lastShownInterstitial;
    private System.DateTime lastAdvertisement;
    public static string OutputMessage
    {
        set { outputMessage = value; }
    }

    public static string uniqueUserId = "demoUserUnity";
    public static string appKey = "835000fd";
    public static string REWARDED_INSTANCE_ID = "0";
    public static string INTERSTITIAL_INSTANCE_ID = "0";
    // Use this for initialization
    void Start()
    {
        Debug.Log("unity-script: MyAppStart Start called");
        // Add Banner Events
        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
        IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
        IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
        IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
        //Add Rewarded Video Events
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;

        //Add Rewarded Video DemandOnly Events
        IronSourceEvents.onRewardedVideoAdOpenedDemandOnlyEvent += RewardedVideoAdOpenedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAdClosedDemandOnlyEvent += RewardedVideoAdClosedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedDemandOnlyEvent += RewardedVideoAvailabilityChangedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAdRewardedDemandOnlyEvent += RewardedVideoAdRewardedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedDemandOnlyEvent += RewardedVideoAdShowFailedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAdClickedDemandOnlyEvent += RewardedVideoAdClickedDemandOnlyEvent;
        // SDK init

        //IronSource.Agent.init (appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);
        //IronSource.Agent.initISDemandOnly (appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL);

        // Load Banner example
        //IronSource.Agent.loadBanner (IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);

        // Add Interstitial Events
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        // Add Interstitial DemandOnly Events
        IronSourceEvents.onInterstitialAdReadyDemandOnlyEvent += InterstitialAdReadyDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedDemandOnlyEvent += InterstitialAdLoadFailedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdShowSucceededDemandOnlyEvent += InterstitialAdShowSucceededDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdShowFailedDemandOnlyEvent += InterstitialAdShowFailedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdClickedDemandOnlyEvent += InterstitialAdClickedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdOpenedDemandOnlyEvent += InterstitialAdOpenedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdClosedDemandOnlyEvent += InterstitialAdClosedDemandOnlyEvent;

        // Add Rewarded Interstitial Events
        IronSourceEvents.onInterstitialAdRewardedEvent += InterstitialAdRewardedEvent;
        //IronSource tracking sdk
        //IronSource.Agent.reportAppStarted();

        //Dynamic config example
        IronSourceConfig.Instance.setClientSideCallbacks(true);

        string id = IronSource.Agent.getAdvertiserId();
        Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);

        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("unity-script: unity version" + IronSource.unityVersion());
        Debug.Log("unity-script: IronSource.Agent.init");
        IronSource.Agent.setUserId(uniqueUserId);
        IronSource.Agent.init(appKey);
        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO);
        //For Interstitial
        IronSource.Agent.init(appKey, IronSourceAdUnits.INTERSTITIAL);
        //For Banners
        IronSource.Agent.init(appKey, IronSourceAdUnits.BANNER);


        IronSource.Agent.loadInterstitial();
        if (ServiceFirebaseAnalytics.Instance.GetAdsRate() > 1)
            CallRequestBanner();
        lastShownInterstitial = System.DateTime.Now;
        lastTouched = System.DateTime.Now;
        IronSource.Agent.validateIntegration();
        lastAdvertisement = System.DateTime.Now;
        FinishPopup.restartButtonClick += CallShowInterstitial;
    }

    private void OnDisable()
    {
        FinishPopup.restartButtonClick -= CallShowInterstitial;
    }
    public bool CallShowRewardedVideo(System.Action<bool> callback)
    {
#if UNITY_EDITOR
        callback(true);
        return true;
#else
        
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            Debug.Log("Loaded");
            rewardedVideoCallback = callback;
            System.TimeSpan duration = new System.TimeSpan(0, 0, 30);
            lastTouched = System.DateTime.Now.Add(duration);
            IronSource.Agent.showRewardedVideo();
            ExecuteLoadingCallbacks();
            return true;
        }
        else if(!noAds)
        {
            ServiceFirebaseAnalytics.Instance.LogNoAdsAvailable();
            ServiceGameAnalytics.Instance.LogNoAdsAvailable();
         //   Fabric.Crashlytics.Crashlytics.Log("no_ads_available");
            noAds = true;
        }
        Debug.Log("Not Loaded");            
        return false;       
#endif
    }
    public bool CheckRewardBasedVideo()
    {
#if UNITY_EDITOR
        return true;
#endif
        return IronSource.Agent.isRewardedVideoAvailable();
    }
    public void CallRequestRewardedVideo()
    {
        //this.RequestRewardBasedVideo();
    }
    public void CallShowInterstitial()
    {
        if ((System.DateTime.Now - lastAdvertisement).TotalSeconds < 45f)
            return;
        if (IronSource.Agent.isInterstitialReady())
        {
            lastAdvertisement = System.DateTime.Now;
            IronSource.Agent.showInterstitial();
            IronSource.Agent.loadInterstitial();
            //CSSoundManager.instance.AdvertisementRunning(true);
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
            IronSource.Agent.loadInterstitial();
        }
    }
    public void CallRequestBanner()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }
    public void CallDestroyBanner()
    {
        //if (this.bannerView == null)
        //return;
        IronSource.Agent.hideBanner();
    }
    public void CallDisplayBanner()
    {
        //if (this.bannerView == null)
        //return;
        IronSource.Agent.displayBanner();
    }
    System.DateTime lastTouched = System.DateTime.Now;
    float loadedCallbacksCheck = 0f;

    // Update is called once per frame
    void Update()
    {
        this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
        loadedCallbacksCheck += Time.unscaledDeltaTime;
        if (loadedCallbacksCheck > 5f)
        {
            loadedCallbacksCheck = 0f;
            if (!loadedCallbackExecuted && CheckRewardBasedVideo())
                ExecuteLoadedCallbacks();
        }
#if UNITY_ANDROID || UNITY_IPHONE
        // if (Input.touchCount > 0)
        //     lastTouched = System.DateTime.Now;
        if ((System.DateTime.Now - lastAdvertisement).TotalSeconds > 45f && ServiceFirebaseAnalytics.Instance.GetAdsRate() > 1)
        {
            if (Preloader.Instance.GetCurrentScene() != "Game")
            {
                System.TimeSpan duration = new System.TimeSpan(0, 0, 30);

                //lastTouched = System.DateTime.Now.Add(duration);
                lastShownInterstitial = System.DateTime.Now;
                //lastAdvertisement = System.DateTime.Now;
                CallShowInterstitial();
            }
        }
#endif
    }
    void OnApplicationPause(bool isPaused)
    {
        Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }


    System.Action<bool> rewardedVideoCallback;
    List<System.Action> rewardedVideoLoadedCallbacks = new List<System.Action>();
    List<System.Action> rewardedVideoLoadingCallbacks = new List<System.Action>();
    bool noAds;

    public void AddRewardedVideoLoadedCallback(System.Action loadedCallback)
    {
        if (!rewardedVideoLoadedCallbacks.Contains(loadedCallback))
            rewardedVideoLoadedCallbacks.Add(loadedCallback);
    }
    public void RemoveRewardedVideoLoadedCallback(System.Action loadedCallback)
    {
        if (rewardedVideoLoadedCallbacks.Contains(loadedCallback))
            rewardedVideoLoadedCallbacks.Remove(loadedCallback);
    }
    public void AddRewardedVideoLoadingCallback(System.Action loadingCallback)
    {
        if (!rewardedVideoLoadingCallbacks.Contains(loadingCallback))
            rewardedVideoLoadingCallbacks.Add(loadingCallback);
    }
    public void RemoveRewardedVideoLoadingCallback(System.Action loadingCallback)
    {
        if (rewardedVideoLoadingCallbacks.Contains(loadingCallback))
            rewardedVideoLoadingCallbacks.Remove(loadingCallback);
    }
    bool loadedCallbackExecuted;
    void ExecuteLoadedCallbacks()
    {
        foreach (System.Action callback in rewardedVideoLoadedCallbacks)
        {
            if (callback != null && callback.Target != null)
                callback();
        }
        loadedCallbackExecuted = true;
    }
    void ExecuteLoadingCallbacks()
    {
        foreach (System.Action callback in rewardedVideoLoadingCallbacks)
        {
            if (callback != null && callback.Target != null)
                callback();
        }
        loadedCallbackExecuted = false;
    }

    #region RewardedVideo
    /************* RewardedVideo API *************/
    public void ShowRewardedVideoButtonClicked()
    {
        Debug.Log("unity-script: ShowRewardedVideoButtonClicked");
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
        }

        // DemandOnly
        // ShowDemandOnlyRewardedVideo ();
    }

    void ShowDemandOnlyRewardedVideo()
    {
        Debug.Log("unity-script: ShowDemandOnlyRewardedVideoButtonClicked");
        if (IronSource.Agent.isISDemandOnlyRewardedVideoAvailable(REWARDED_INSTANCE_ID))
        {
            IronSource.Agent.showISDemandOnlyRewardedVideo(REWARDED_INSTANCE_ID);
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isISDemandOnlyRewardedVideoAvailable - False");
        }
    }

    /************* RewardedVideo Delegates *************/
    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
        if (canShowAd)
        {
            ExecuteLoadedCallbacks();
            //ShowText.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.blue;
        }
        else
        {
            ExecuteLoadingCallbacks();
            //ShowText.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.red;
        }
    }

    void RewardedVideoAdOpenedEvent()
    {
        lastTouched = System.DateTime.Now;
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
        lastAdvertisement = System.DateTime.Now;
    }

    void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
    {
        lastTouched = System.DateTime.Now;
        lastAdvertisement = System.DateTime.Now;
        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());
        //userTotalCredits = userTotalCredits + ssp.getRewardAmount();
        //AmountText.GetComponent<UnityEngine.UI.Text>().text = "" + userTotalCredits;
        if (rewardedVideoCallback != null)
        {
            rewardedVideoCallback(true);
            rewardedVideoCallback = null;
        }
    }

    void RewardedVideoAdClosedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
        if (rewardedVideoCallback != null)
        {
            rewardedVideoCallback(false);
            //rewardedVideoCallback = null;
        }
        lastTouched = System.DateTime.Now;
        lastAdvertisement = System.DateTime.Now;
        Time.timeScale = 1;
    }

    void RewardedVideoAdStartedEvent()
    {
        lastTouched = System.DateTime.Now;
        Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
    }

    void RewardedVideoAdEndedEvent()
    {
        lastTouched = System.DateTime.Now;
        Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
        lastAdvertisement = System.DateTime.Now;
    }

    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
    }

    /************* RewardedVideo DemandOnly Delegates *************/

    void RewardedVideoAvailabilityChangedDemandOnlyEvent(string instanceId, bool canShowAd)
    {
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedDemandOnlyEvent for instance: " + instanceId + ", value = " + canShowAd);
        if (canShowAd)
        {
            //ShowText.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.blue;
        }
        else
        {
            //ShowText.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.red;
        }
    }

    void RewardedVideoAdOpenedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdRewardedDemandOnlyEvent(string instanceId, IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdRewardedDemandOnlyEvent for instance: " + instanceId + ", amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());
        //userTotalCredits = userTotalCredits + ssp.getRewardAmount();
        //AmountText.GetComponent<UnityEngine.UI.Text>().text = "" + userTotalCredits;
    }

    void RewardedVideoAdClosedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClosedDemandOnlyEvent for instance: " + instanceId);
        Time.timeScale = 1;
    }

    void RewardedVideoAdStartedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdStartedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdEndedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdEndedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedDemandOnlyEvent(string instanceId, IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedDemandOnlyEvent for instance: " + instanceId + ", name = " + ssp.getRewardName());
    }
    #endregion
    #region Interstitial
    /************* Interstitial API *************/
    public void LoadInterstitialButtonClicked()
    {
        Debug.Log("unity-script: LoadInterstitialButtonClicked");
        IronSource.Agent.loadInterstitial();

        //DemandOnly
        // LoadDemandOnlyInterstitial ();
    }

    public void ShowInterstitialButtonClicked()
    {
        Debug.Log("unity-script: ShowInterstitialButtonClicked");
        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
        }

        // DemandOnly
        // ShowDemandOnlyInterstitial ();
    }

    void LoadDemandOnlyInterstitial()
    {
        Debug.Log("unity-script: LoadDemandOnlyInterstitialButtonClicked");
        IronSource.Agent.loadISDemandOnlyInterstitial(INTERSTITIAL_INSTANCE_ID);
    }

    void ShowDemandOnlyInterstitial()
    {
        Debug.Log("unity-script: ShowDemandOnlyInterstitialButtonClicked");
        if (IronSource.Agent.isISDemandOnlyInterstitialReady(INTERSTITIAL_INSTANCE_ID))
        {
            IronSource.Agent.showISDemandOnlyInterstitial(INTERSTITIAL_INSTANCE_ID);
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isISDemandOnlyInterstitialReady - False");
        }
    }

    /************* Interstitial Delegates *************/
    void InterstitialAdReadyEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
        //ShowText.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.blue;
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void InterstitialAdShowSucceededEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
        //ShowText.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.red;
        lastAdvertisement = System.DateTime.Now;
    }

    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        //ShowText.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.red;
    }

    void InterstitialAdClickedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
    }

    void InterstitialAdOpenedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
    }

    void InterstitialAdClosedEvent()
    {
        lastAdvertisement = System.DateTime.Now;
        Debug.Log("unity-script: I got InterstitialAdClosedEvent");
        Time.timeScale = 1;
    }

    void InterstitialAdRewardedEvent()
    {
        lastAdvertisement = System.DateTime.Now;
        Debug.Log("unity-script: I got InterstitialAdRewardedEvent");
    }

    /************* Interstitial DemandOnly Delegates *************/

    void InterstitialAdReadyDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdReadyDemandOnlyEvent for instance: " + instanceId);
        //ShowText.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.blue;
    }

    void InterstitialAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", error code: " + error.getCode() + ",error description : " + error.getDescription());
    }

    void InterstitialAdShowSucceededDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdShowSucceededDemandOnlyEvent for instance: " + instanceId);
        //ShowText.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.red;
    }

    void InterstitialAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", error code :  " + error.getCode() + ",error description : " + error.getDescription());
        //ShowText.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.red;
    }

    void InterstitialAdClickedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdClickedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdOpenedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdClosedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdClosedDemandOnlyEvent for instance: " + instanceId);
        Time.timeScale = 1;
    }

    void InterstitialAdRewardedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdRewardedDemandOnlyEvent for instance: " + instanceId);
    }
    #endregion
    #region Banner Events
    void BannerAdLoadedEvent()
    {
        Debug.Log("unity-script: I got BannerAdLoadedEvent");
    }

    void BannerAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void BannerAdClickedEvent()
    {
        Debug.Log("unity-script: I got BannerAdClickedEvent");
    }

    void BannerAdScreenPresentedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
    }

    void BannerAdScreenDismissedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
    }

    void BannerAdLeftApplicationEvent()
    {
        Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
    }
    #endregion
}
