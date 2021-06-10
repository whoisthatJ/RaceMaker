using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyRewardManager : MonoBehaviour {
    public static DailyRewardManager Instance;
    [SerializeField] GameObject rewardPanel;
    [SerializeField] Button btnClaim;
    [SerializeField] Button btnClose;
    [SerializeField] TextMeshProUGUI txtTimer;
    [SerializeField] Transform rewardsParent;

    private List<DailyReward> rewards;
    private DailyReward currentReward;
    private DateTime nextRewardTime;

    private void OnEnable()
    {
        Preloader.BackButtonPressed += ClosePanel;
        btnClaim.onClick.AddListener(ClaimButtonClicked);
        btnClose.onClick.AddListener(ClosePanel);
    }

    private void OnDisable()
    {
        Preloader.BackButtonPressed -= ClosePanel;
        btnClaim.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        InitRewards();
	}

    public void OpenPanel()
    {
        rewardPanel.SetActive(true);
        Preloader.Instance.AddPanelToTheList(rewardPanel);
    }

    private void ClosePanel()
    {
        rewardPanel.SetActive(false);
        Preloader.Instance.RemovePanelFromTheList(rewardPanel);
    }

    private void ClaimButtonClicked()
    {
        if (currentReward.rewardType == "Skin")
        {
            MainRoot.Instance.userConfig.purchasedSkins.Add(currentReward.reward);
        }
        else if (currentReward.rewardType == "Coins")
        {
            int amount = Int32.Parse(currentReward.reward);
            MainRoot.Instance.userConfig.softCurrency += amount;
        }
        MainRoot.Instance.userConfig.lastClaimedRewardDate = DateTime.Now;
        MainRoot.Instance.userConfig.lastClaimedRewardDayNumber = currentReward.day;
        int dayAfter14 = MainRoot.Instance.userConfig.lastClaimedRewardDayNumber % 14;
        rewardsParent.GetChild(dayAfter14 - 1).GetChild(1).gameObject.SetActive(true);
        nextRewardTime = DateTime.Now + TimeSpan.FromHours(24);
        StartCoroutine(TimerCoroutine());
    }

    private void InitRewards()
    {
        rewards = ServiceXML.Instance.GetDailyRewards();
        int inc14 = MainRoot.Instance.userConfig.lastClaimedRewardDayNumber / 14;
        int dayAfter14 = MainRoot.Instance.userConfig.lastClaimedRewardDayNumber % 14;
        for (int i = 1; i <= 14; i++)
        {
            DailyReward r = rewards.Find(q => q.day == (14 * inc14 + i));
            if (r == null)
                break;
            rewardsParent.GetChild(i - 1).gameObject.SetActive(true);
            if (r.rewardType == "Skin")
            {
                rewardsParent.GetChild(i - 1).GetComponent<Image>().sprite = (Sprite)ServiceResources.Load("SkinPreviews/" + r.reward);//reward preview
                rewardsParent.GetChild(i - 1).GetChild(0).gameObject.SetActive(false);
            }
            else if (r.rewardType == "Coins")
            {
                //rewardsParent.GetChild(i - 1).GetComponent<Image>().sprite = some coin sprites
                rewardsParent.GetChild(i - 1).GetChild(0).gameObject.SetActive(true);
                rewardsParent.GetChild(i - 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = r.reward + "<sprite name=Coin>";
            }
            if (i <= dayAfter14)
                rewardsParent.GetChild(i - 1).GetChild(1).gameObject.SetActive(true);
            else
                rewardsParent.GetChild(i - 1).GetChild(1).gameObject.SetActive(false);
            if (i == dayAfter14 + 1)
                currentReward = r;
        }
        nextRewardTime = MainRoot.Instance.userConfig.lastClaimedRewardDate + TimeSpan.FromHours(24);

        if ((nextRewardTime-DateTime.Now).TotalSeconds > 0)
            StartCoroutine(TimerCoroutine());
        else
        {
            txtTimer.gameObject.SetActive(false);
            btnClaim.gameObject.SetActive(true);
        }
    }

    IEnumerator TimerCoroutine()
    {
        txtTimer.gameObject.SetActive(true);
        btnClaim.gameObject.SetActive(false);
        TimeSpan timeToClaim = nextRewardTime - DateTime.Now;
        while (timeToClaim.TotalSeconds > 0)
        {
            txtTimer.text = string.Format("{0}:{1}:{2}", timeToClaim.Hours, timeToClaim.Minutes, timeToClaim.Seconds);
            yield return new WaitForSeconds(1f);
            timeToClaim = nextRewardTime - DateTime.Now;
        }
        btnClaim.gameObject.SetActive(true);
    }
}
