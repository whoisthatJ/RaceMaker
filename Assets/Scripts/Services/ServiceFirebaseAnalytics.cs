using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using Firebase;

public class ServiceFirebaseAnalytics : MonoBehaviour
{

    public static ServiceFirebaseAnalytics Instance
    {
        get;
        private set;
    }

    private const string EVENT_MUSIC_ENABLE = "music_enable";
    private const string EVENT_SOUND_ENABLE = "sound_enable";
    private const string EVENT_LEVEL_COUNT = "level_count";
    private const string EVENT_SOFT_VALUE_SESSION = "soft_value_session";
    private const string EVENT_BUY_CUSTOMIZE_ITEM = "buy_customize_item";
    private const string EVENT_CLAIM_FREEGIFT = "claim_freegift";
    private const string EVENT_DOUBLE_CLAIM_FREEGIFT = "double_claim_freegift";
    private const string EVENT_RATEUS_LETER = "rate_us_later";
    private const string EVENT_RATEUS_NEVER = "rate_us_never";
    private const string EVENT_RATEUS_RATE = "rate_us_rate";
    private const string EVENT_REMOVE_ADS_CLICKED = "remove_ads_clicked";
    private const string EVENT_SOFT_CURRENCY_REMATCH = "rematch_soft_currency";
    private const string EVENT_ADS_WATCHES = "ads_watches";
    private const string EVENT_SCORE_SHARE = "score_share";
    private const string EVENT_NOADS_AVAILABLE = "no_ads_available";
    private const string EVENT_COMPLETE_TUTORIAL = "complete_tutorial";
    private const string EVENT_LEVEL_STARTED = "level_started";
    private const string EVENT_LEVEL_FAILED = "level_failed";
    private const string EVENT_DEFEAT = "defeat";
    private const string EventSkeletonClicked = "skeleton_clicked";
    private const string EventSoccerClicked = "soccer_clicked";
    private const string EventHolderClicked = "holder_clicked";
    private const string EventTTUClicked = "ttu_clicked";
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
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                    "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        Debug.Log("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        Dictionary<string, object> defaults =
            new Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:

        defaults.Add("ads_rate", 2);

        if (PlayerPrefs.GetInt("ads_removed", 0) == 1)
            defaults["ads_rate"] = 0;
        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
        FetchData();
    }
    void FetchData()
    {
        Debug.Log("Fetching data...");
        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
        // By default the timespan is 12 hours, and for production apps, this is a good
        // number.  For this example though, it's set to a timespan of zero, so that
        // changes in the console will always show up immediately.
        System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(
            System.TimeSpan.Zero);
        fetchTask.ContinueWith(FetchComplete);
    }

    void FetchComplete(System.Threading.Tasks.Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                Debug.Log(string.Format("Remote data loaded and ready (last fetch time {0}).",
                    info.FetchTime));
                try
                {

                    int adsRate = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("ads_rate").LongValue;
                    //adsRate = 3;
                    if (PlayerPrefs.GetInt("ads_removed", 0) == 0 && adsRate != GetAdsRate())
                    {
                        PlayerPrefs.SetInt("ads_rate", adsRate);
                        if (adsRate > 1)
                            ServiceIronSource.Instance.CallRequestBanner();
                    }
                    if (PlayerPrefs.GetInt("ads_removed", 0) == 1)
                        PlayerPrefs.SetInt("ads_rate", 0);
                    /*else
                    {
                        PlayerPrefs.SetInt("ads_rate", 0);
                    }*/
                }
                catch (System.Exception e)
                {
                    Debug.Log(e.ToString());
                    Debug.Log("Firebase Remote Config did it again.");
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }

    public int GetAdsRate()
    {
        return PlayerPrefs.GetInt("ads_rate", 2);
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
    public void LogSkeletonClicked()
    {
        FirebaseAnalytics.LogEvent(EventSkeletonClicked, "skeleton_clicked", 1);
    }

    public void LogSoccerClicked()
    {
        FirebaseAnalytics.LogEvent(EventSoccerClicked, "soccer_clicked", 1);
    }
    public void LogHolderClicked()
    {
        FirebaseAnalytics.LogEvent(EventHolderClicked, "holder_clicked", 1);
    }
    public void LogTTUClicked()
    {
        FirebaseAnalytics.LogEvent(EventTTUClicked, "ttu_clicked", 1);
    }
    public void LogTutorialComplete(string tutorial)
    {
        FirebaseAnalytics.LogEvent(EVENT_COMPLETE_TUTORIAL, "tutorial", tutorial);
    }
    public void LogLevelUp(int level)
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelUp, new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter (
                FirebaseAnalytics.ParameterLevel, level),
            new Parameter (
                FirebaseAnalytics.ParameterLevel + "_str", level.ToString ())
        });
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter (
                FirebaseAnalytics.ParameterLevel, level),
            new Parameter (
                FirebaseAnalytics.ParameterLevel + "_str", level.ToString ())
        });
    }
    public void LogLevelStart(int level)
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter (
                FirebaseAnalytics.ParameterLevel, level),
            new Parameter (
                FirebaseAnalytics.ParameterLevel + "_str", level.ToString ())
        });
        FirebaseAnalytics.LogEvent(EVENT_LEVEL_STARTED, new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter (
                FirebaseAnalytics.ParameterLevel, level),
            new Parameter (
                FirebaseAnalytics.ParameterLevel + "_str", level.ToString ())
        });
    }
    public void LogDefeat(int level)
    {
        FirebaseAnalytics.LogEvent(EVENT_DEFEAT, new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter (
                "level", level),
            new Parameter (
                "level_str", level.ToString ())
        });
    }
    public void LogLevelFail(int level, string how)
    {
        FirebaseAnalytics.LogEvent(EVENT_DEFEAT, new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter (
                "level", level),
            new Parameter (
                "level_str", level.ToString ()),
            new Parameter (
                "how", how)
        });
    }
    public void LogRewardedAdsWatched(string reward)
    {
        FirebaseAnalytics.LogEvent(EVENT_ADS_WATCHES, "reward", reward);
    }

    public void LogUnlockAchievement(string id)
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventUnlockAchievement, FirebaseAnalytics.ParameterAchievementId, id);
    }
    public void LogShareScore()
    {
        FirebaseAnalytics.LogEvent(EVENT_SCORE_SHARE, "shared", 1);
    }

    public void LogRateUsLater()
    {
        FirebaseAnalytics.LogEvent(EVENT_RATEUS_LETER, "later", 1);
    }
    public void LogRateUsNever()
    {
        FirebaseAnalytics.LogEvent(EVENT_RATEUS_NEVER, "never", 1);
    }
    public void LogRateUsRate()
    {
        FirebaseAnalytics.LogEvent(EVENT_RATEUS_RATE, "rate", 1);
    }
    public void LogRemoveAdsClicked(string place)
    {
        FirebaseAnalytics.LogEvent(EVENT_REMOVE_ADS_CLICKED + "_" + place, "clicked", 1);
    }
    public void LogSoftCurrencyRematch()
    {
        FirebaseAnalytics.LogEvent(EVENT_SOFT_CURRENCY_REMATCH, "rematch", 1);
    }

    public void LogEnableMusic(int musicEnable = 1)
    {
        FirebaseAnalytics.LogEvent(EVENT_MUSIC_ENABLE, "music_enable", musicEnable);
    }

    public void LogClaimFreeGift()
    {
        FirebaseAnalytics.LogEvent(EVENT_CLAIM_FREEGIFT, "claim", 1);
    }

    public void LogDoubleClaimFreeGift()
    {
        FirebaseAnalytics.LogEvent(EVENT_DOUBLE_CLAIM_FREEGIFT, "double_claim", 1);
    }

    public void LogEnableSound(int soundEnable = 1)
    {
        FirebaseAnalytics.LogEvent(EVENT_SOUND_ENABLE, "sound_enable", soundEnable);
    }

    public void LogSpendSoftCurrency(string item, int value)
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSpendVirtualCurrency, new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter (
                FirebaseAnalytics.ParameterItemName, item),
            new Firebase.Analytics.Parameter (
                FirebaseAnalytics.ParameterVirtualCurrencyName, "soft"),
            new Parameter (
                FirebaseAnalytics.ParameterValue, value)
        });
    }
    public void LogEarnSoftCurrency(string item, int value)
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency, new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter (
                FirebaseAnalytics.ParameterItemName, item),
            new Firebase.Analytics.Parameter (
                FirebaseAnalytics.ParameterVirtualCurrencyName, "soft"),
            new Parameter (
                FirebaseAnalytics.ParameterValue, value)
        });
    }
    bool noAds;
    public void LogNoAdsAvailable()
    {
        if (!noAds)
        {
            FirebaseAnalytics.LogEvent(EVENT_NOADS_AVAILABLE, "no_ads", 1);
            noAds = true;
        }
    }
}
