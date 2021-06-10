using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationUI : MonoBehaviour {
    public static CustomizationUI Instance;

    [SerializeField] private int adReward = 100;
    [SerializeField] GameObject pnlCustomization;
    [SerializeField] Button btnRewardAd;
    [SerializeField] Button btnBack;
    [SerializeField] GameObject adLoading;
    [SerializeField] Transform scrollParent;
    [SerializeField] GameObject itemPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Preloader.BackButtonPressed += ClosePanel;
        btnRewardAd.onClick.AddListener(WatchAd);
        btnBack.onClick.AddListener(ClosePanel);
    }

    private void OnDisable()
    {
        Preloader.BackButtonPressed -= ClosePanel;
        btnRewardAd.onClick.RemoveAllListeners();
        btnBack.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        InitializeStoreContent();
    }

    public void OpenCustomizationPanel()
    {		
        pnlCustomization.SetActive(true);
        Preloader.Instance.AddPanelToTheList(pnlCustomization);
    }

    private void ClosePanel()
    {
        pnlCustomization.SetActive(false);
		//this.SetActiveDelay(pnlCustomization, .223f);
        Preloader.Instance.RemovePanelFromTheList(pnlCustomization);
    }

    private void InitializeStoreContent()
    {
        List<Skin> allSkins = ServiceXML.Instance.GetAllSkins();
        for(int i = 0; i < allSkins.Count; i++)
        {
            GameObject temp = Instantiate(itemPrefab, scrollParent) as GameObject;
            temp.GetComponent<CustomizationElement>().SetSkin(allSkins[i]);
        }
    }

    private void WatchAd()
    {
        //if (!ServiceIronSource.Instance.CallShowRewardedVideo(WatchAdCallback))
            //Notification.Instance.Open(Lean.Localization.LeanLocalization.GetTranslationText("NoAds"));
        WatchAdCallback(true);
    }

    private void AdLoadedCallback()
    {		
        adLoading.SetActive(false);
    }

    private void AdLoading()
    {
        adLoading.SetActive(true);
    }
    
    private void WatchAdCallback(bool success)
    {
        if (success)
        {
            MainRoot.Instance.userConfig.softCurrency += adReward;
        }
    }
}
