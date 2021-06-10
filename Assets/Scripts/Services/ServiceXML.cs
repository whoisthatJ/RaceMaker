using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class ServiceXML : MonoBehaviour
{
    public static ServiceXML Instance
    {
        get;
        private set;
    }

    private const string PATH = "Xmls/";
    private const string FILE_SKINS = "skins";
    private const string FILE_REWARDS = "rewards";
    private const string FILE_LEVELS = "levels";
    private const string FILE_LEVELS2 = "levels2";
    private const string FILE_LEVELS_TUTORIAL = "levelsTutorial";
    private const string FILE_PROGRESS_LEVELS = "progressLevels";
    private const string FILE_DUMMIES = "dummies";

    private List<Skin> skins;
    private List<DailyReward> rewards;
    private List<RaceLevel> levels;
    private List<RaceLevel> levels2;
    private List<RaceLevel> levelsTutorial;
    private List<ProgressLevel> progressLevels;
    private List<Dummy> dummies;

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

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        ParseSkins();
        ParseRewards();
        ParseLevels();
        ParseProgressLevels();
        ParseDummies();
    }

    public void LocalizeNames()
    {
        LocalizeSkinNames();
    }

    private void ParseSkins()
    {
        SkinContainer skinContainer = SkinContainer.Load(PATH + FILE_SKINS);
        skins = skinContainer.skins;
    }

    private void LocalizeSkinNames()
    {
        foreach (Skin t in skins)
        {
            string localizedName = Lean.Localization.LeanLocalization.GetTranslationText(t.id);
            t.name = localizedName;
        }
    }

    public Skin GetSkinByID(string id)
    {
        return skins.Find(q => q.id == id);
    }

    public List<Skin> GetAllSkins()
    {
        return skins;
    }

    private void ParseRewards()
    {
        DailyRewardContainer rewardContainer = DailyRewardContainer.Load(PATH + FILE_REWARDS);
        rewards = rewardContainer.rewards;
    }

    public DailyReward GetDailyRewardByDay(int day)
    {
        return rewards.Find(q => q.day == day);
    }

    public List<DailyReward> GetDailyRewards()
    {
        return rewards;
    }
    private void ParseLevels()
    {
        RaceLevelContainer levelContainer = RaceLevelContainer.Load(PATH + FILE_LEVELS);
        levels = levelContainer.levels;
        RaceLevelContainer levelContainer2 = RaceLevelContainer.Load(PATH + FILE_LEVELS2);
        levels2 = levelContainer2.levels;
        RaceLevelContainer levelContainerTutorial = RaceLevelContainer.Load(PATH + FILE_LEVELS_TUTORIAL);
        levelsTutorial = levelContainerTutorial.levels;
    }

    public RaceLevel GetRaceLevelByLevel(int level)
    {
        if (MainRoot.Instance.userConfig.levelXmlId == 0)
            return levels.Find(l => l.level == level);
        else
            return levels2.Find(l => l.level == level);
    }

    public List<RaceLevel> GetRaceLevels()
    {
        if (MainRoot.Instance.userConfig.levelXmlId == 0)
            return levels;
        else
            return levels2;
    }
    public List<RaceLevel> GetTutorialRaceLevels()
    {
        return levelsTutorial;
    }
    private void ParseProgressLevels()
    {
        ProgressLevelContainer progressLevelContainer = ProgressLevelContainer.Load(PATH + FILE_PROGRESS_LEVELS);
        progressLevels = progressLevelContainer.levels;
    }

    public ProgressLevel GetProgressLevelByLevel(int level)
    {
        return progressLevels.Find(l => l.level == level);
    }

    public List<ProgressLevel> GetProgressLevels()
    {
        return progressLevels;
    }
    private void ParseDummies()
    {
        DummyContainer dummyContainer = DummyContainer.Load(PATH + FILE_DUMMIES);
        dummies = dummyContainer.dummies;
    }

    public List<Dummy> GetDummies()
    {
        return dummies;
    }
}