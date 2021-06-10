using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using System.Runtime.Serialization;

[System.Serializable]
public class UserConfig
{
    public delegate void UserConfigEvent();
    public static event UserConfigEvent OnSoftCurrencyAmountChanged, OnCustomizationAdWatchedEvent, OnHighScoreChanged;
    public int softCurrency
    {
        get
        {
            return softCurrencyAmount;
        }
        set
        {
            softCurrencyAmount = value;
            if (OnSoftCurrencyAmountChanged != null)
                OnSoftCurrencyAmountChanged();
        }
    }
    public int customizationAdsWatched
    {
        get
        {
            return customizationAdsWatchedAmount;
        }
        set
        {
            customizationAdsWatchedAmount = value;
            if (OnCustomizationAdWatchedEvent != null)
                OnCustomizationAdWatchedEvent();
        }
    }
    public int highScore
    {
        get
        {
            return bestScore;
        }
        set
        {
            bestScore = value;
            if (OnHighScoreChanged != null)
                OnHighScoreChanged();
        }
    }
    [SerializeField]
    private int softCurrencyAmount;
    public int bestScore;
    public int currentScore;
    public bool freeGiftDoubled;
    public int freeGiftValue;
    public DateTime lastClaimedFreeGift;
    public bool isSound;
    public bool isMusic;
    public bool stopShowingRateUs;
    public bool isTutorialPassed;
    public int levelXmlId;
    public string appVersion;
    public bool rateUsIsReady;

	//Levlels
	public int currentLevel;
    public int exp;
	public int maxLevels;
	public List<LevelModel> levelModel;
    //Skins
    public string currentSkin;
    public List<string> purchasedSkins;
    [SerializeField]
    private int customizationAdsWatchedAmount;
    //rewards
    public DateTime lastClaimedRewardDate;
    public int lastClaimedRewardDayNumber;

    public static UserConfig GetDefault()
    {
        UserConfig defaultConfig = new UserConfig();
        defaultConfig.softCurrency = 500;
        defaultConfig.freeGiftDoubled = false;
        defaultConfig.freeGiftValue = 50;
        defaultConfig.lastClaimedFreeGift = DateTime.Now;
        defaultConfig.isSound = true;
        defaultConfig.isMusic = true;
        defaultConfig.stopShowingRateUs = false;
        defaultConfig.rateUsIsReady = false;
        defaultConfig.isTutorialPassed = false;
        defaultConfig.currentScore = 0;
		defaultConfig.bestScore = 0;
        defaultConfig.currentSkin = "Default";
        defaultConfig.purchasedSkins = new List<string>();
        defaultConfig.purchasedSkins.Add("Default");
        defaultConfig.customizationAdsWatched = 0;
        defaultConfig.lastClaimedRewardDate = DateTime.Now - TimeSpan.FromHours(24);
        defaultConfig.lastClaimedRewardDayNumber = 0;
		defaultConfig.levelModel = new List<LevelModel>();
		defaultConfig.levelModel.Add(new LevelModel(0, 0));	
		defaultConfig.maxLevels = 100;
        defaultConfig.currentLevel = 1;
        return defaultConfig;
    }
}
