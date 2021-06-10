using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class ServiceGameAnalytics : MonoBehaviour
{
    public static ServiceGameAnalytics Instance
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
        GameAnalytics.Initialize();
    }

    private const string EVENT_LEVEL_UP = "level_up:";
    private const string EVENT_LEVEL_START = "level_start:";
    private const string EVENT_LEVEL_FAIL = "level_fail:";
    private const string EVENT_DEFEAT = "defeat:";
    private const string EVENT_UNLOCK_ACHIEVEMENT = "unlock_achievement:";
    private const string EVENT_SPEND_VIRTUAL_CURRENCY = "spend_currency:";
    private const string EVENT_EARN_VIRTUAL_CURRENCY = "earn_currency:";
    private const string EVENT_MATCH_SCORE = "match_score:";
    private const string EVENT_MUSIC_ENABLE = "music_enable:";
    private const string EVENT_SOUND_ENABLE = "sound_enable:";
    private const string EVENT_LEVEL_COUNT = "level_count:";
    private const string EVENT_SOFT_VALUE_SESSION = "soft_value_session:";
    private const string EVENT_BUY_CUSTOMIZE_ITEM = "buy_customize_item:";
    private const string EVENT_CLAIM_FREEGIFT = "claim_freegift:";
    private const string EVENT_DOUBLE_CLAIM_FREEGIFT = "double_claim_freegift:";
    private const string EVENT_RATEUS_LETER = "rate_us_later:";
    private const string EVENT_RATEUS_NEVER = "rate_us_never:";
    private const string EVENT_RATEUS_RATE = "rate_us_rate:";
    private const string EVENT_REMOVE_ADS_CLICKED = "remove_ads_clicked:";
    private const string EVENT_SOFT_CURRENCY_REMATCH = "rematch_soft_currency:";
    private const string EVENT_ADS_WATCHES = "ads_watches:";
    private const string EVENT_SCORE_SHARE = "score_share:";
    private const string EVENT_NOADS_AVAILABLE = "no_ads_available:";
    private const string EVENT_COMPLETE_TUTORIAL = "complete_tutorial:";
    private const string EventSkeletonClicked = "skeleton_clicked";
    private const string EventSoccerClicked = "soccer_clicked";
    private const string EventTTUClicked = "ttu_clicked";
    private const string EventHolderClicked = "holder_clicked";

    public void LogTutorialComplete(string tutorial)
    {
        GameAnalytics.NewDesignEvent(EVENT_COMPLETE_TUTORIAL + tutorial, 1f);
    }
    public void LogLevelUp(int level)
    {
        GameAnalytics.NewDesignEvent(EVENT_LEVEL_UP + level, 1f);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level " + level.ToString());
    }

    public void LogLevelStart(int level)
    {
        GameAnalytics.NewDesignEvent(EVENT_LEVEL_START + level, 1f);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level " + level.ToString());
    }
    public void LogLevelFail(int level, string how)
    {
        GameAnalytics.NewDesignEvent(EVENT_LEVEL_FAIL + how + ":" + level, 1f);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, how, "Level " + level.ToString());
    }
    public void LogDefeat(int level)
    {
        GameAnalytics.NewDesignEvent(EVENT_DEFEAT + level, 1f);
    }
    public void LogRewardedAdsWatched(string reward)
    {
        GameAnalytics.NewDesignEvent(EVENT_ADS_WATCHES + reward, 1f);
    }

    public void LogUnlockAchievement(string id)
    {
        GameAnalytics.NewDesignEvent(EVENT_UNLOCK_ACHIEVEMENT + id, 1f);
    }
    public void LogShareScore()
    {
        GameAnalytics.NewDesignEvent(EVENT_SCORE_SHARE, 1f);
    }

    public void LogClaimFreeGift()
    {
        GameAnalytics.NewDesignEvent(EVENT_CLAIM_FREEGIFT, 1f);
    }

    public void LogDoubleClaimFreeGift()
    {
        GameAnalytics.NewDesignEvent(EVENT_DOUBLE_CLAIM_FREEGIFT, 1f);
    }

    public void LogRateUsLater()
    {
        GameAnalytics.NewDesignEvent(EVENT_RATEUS_LETER, 1f);
    }
    public void LogRateUsNever()
    {
        GameAnalytics.NewDesignEvent(EVENT_RATEUS_NEVER, 1f);
    }
    public void LogRateUsRate()
    {
        GameAnalytics.NewDesignEvent(EVENT_RATEUS_RATE, 1f);
    }
    public void LogRemoveAdsClicked(string place)
    {
        GameAnalytics.NewDesignEvent(EVENT_REMOVE_ADS_CLICKED + "_" + place, 1f);
    }
    public void LogSoftCurrencyRematch()
    {
        GameAnalytics.NewDesignEvent(EVENT_SOFT_CURRENCY_REMATCH, 1f);
    }
    public void LogMatchScore(string score)
    {
        GameAnalytics.NewDesignEvent(EVENT_MATCH_SCORE + score, 1f);
    }
    public void LogSpendSoftCurrency(string item, int value)
    {
        GameAnalytics.NewDesignEvent(EVENT_SPEND_VIRTUAL_CURRENCY + item, value);
    }
    public void LogEarnSoftCurrency(string item, int value)
    {
        GameAnalytics.NewDesignEvent(EVENT_EARN_VIRTUAL_CURRENCY + item, value);
    }

    public void LogEnableMusic(int musicEnable = 1)
    {
        GameAnalytics.NewDesignEvent(EVENT_MUSIC_ENABLE, musicEnable);
    }

    public void LogEnableSound(int soundEnable = 1)
    {
        GameAnalytics.NewDesignEvent(EVENT_SOUND_ENABLE, soundEnable);
    }
    public void LogSkeletonClicked()
    {
        GameAnalytics.NewDesignEvent(EventSkeletonClicked, 1f);
    }
    public void LogSoccerClicked()
    {
        GameAnalytics.NewDesignEvent(EventSoccerClicked, 1f);
    }

    public void LogHolderClicked()
    {
        GameAnalytics.NewDesignEvent(EventHolderClicked, 1f);
    }

    public void LogTTUClicked()
    {
        GameAnalytics.NewDesignEvent(EventTTUClicked, 1f);
    }
    bool noAds;
    public void LogNoAdsAvailable()
    {
        if (!noAds)
        {
            GameAnalytics.NewDesignEvent(EVENT_NOADS_AVAILABLE, 1f);
            noAds = true;
        }
    }
}
