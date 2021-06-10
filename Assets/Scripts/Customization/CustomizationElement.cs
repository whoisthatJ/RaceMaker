using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationElement : MonoBehaviour {
    public delegate void SkinChangedDelegate();
    public static event SkinChangedDelegate SkinChangedEvent;

    [SerializeField] Image preview;
    [SerializeField] Button buyBtn;
    [SerializeField] GameObject skinPickedSign;
    [SerializeField] GameObject typeCoins;
    [SerializeField] TextMeshProUGUI priceCoins;
    [SerializeField] GameObject typeAds;
    [SerializeField] TextMeshProUGUI priceAds;

    private Skin currentSkin;

    private void OnEnable()
    {
        buyBtn.onClick.AddListener(ItemClicked);
        UserConfig.OnCustomizationAdWatchedEvent += CustomizationAdWatched;
        CustomizationElement.SkinChangedEvent += SkinChanged;
    }

    private void OnDisable()
    {
        buyBtn.onClick.RemoveAllListeners();
        UserConfig.OnCustomizationAdWatchedEvent -= CustomizationAdWatched;
        CustomizationElement.SkinChangedEvent -= SkinChanged;
    }

    public void SetSkin(Skin s)
    {
        currentSkin = s;
        if (MainRoot.Instance.userConfig.purchasedSkins.Contains(currentSkin.id))
        {
            typeCoins.SetActive(false);
            typeAds.SetActive(false);
            if (MainRoot.Instance.userConfig.currentSkin == currentSkin.id)
                skinPickedSign.SetActive(true);
            else
                skinPickedSign.SetActive(false);
        }
        else
        {
            skinPickedSign.SetActive(false);
            switch (currentSkin.priceType)
            {
                case "coins":
                    typeCoins.SetActive(true);
                    typeAds.SetActive(false);
                    priceCoins.text = currentSkin.price.ToString() + " <sprite name=Coin>";
                    break;
                case "ad":
                    typeCoins.SetActive(false);
                    typeAds.SetActive(true);
                    priceAds.text = MainRoot.Instance.userConfig.customizationAdsWatched.ToString() + "/" + currentSkin.price.ToString() + " <sprite name=Ads>";
                    break;
            }
        }
    }

    private void ItemClicked()
    {
        if (MainRoot.Instance.userConfig.purchasedSkins.Contains(currentSkin.id))
        {
            skinPickedSign.SetActive(true);
            typeCoins.SetActive(false);
            MainRoot.Instance.userConfig.currentSkin = currentSkin.id;
            SkinChangedEvent?.Invoke();
        }
        else
            switch (currentSkin.priceType)
            {
                case "coins":
                    PayWithCoins();
                    break;
                case "ad":
                    WatchAd();
                    break;
            }
    }

    private void PayWithCoins()
    {
        if (MainRoot.Instance.userConfig.softCurrency >= currentSkin.price)
        {
            typeCoins.SetActive(false);
            MainRoot.Instance.userConfig.purchasedSkins.Add(currentSkin.id);
            MainRoot.Instance.userConfig.currentSkin = currentSkin.id;
            SkinChangedEvent?.Invoke();
            MainRoot.Instance.userConfig.softCurrency -= currentSkin.price;
        }
        else
        {
            Notification.Instance.Open(Lean.Localization.LeanLocalization.GetTranslationText("NoMoney"));
        }
    }

    private void WatchAd()
    {
        //if (!ServiceIronSource.Instance.CallShowRewardedVideo(WatchAdCallback))
        //Notification.Instance.Open(Lean.Localization.LeanLocalization.GetTranslationText("NoAds"));
        WatchAdCallback(true);
    }
    
    private void WatchAdCallback(bool success)
    {
        if (success)
            MainRoot.Instance.userConfig.customizationAdsWatched++;
    }

    private void CustomizationAdWatched()
    {
        if (currentSkin.priceType == "ad" && !MainRoot.Instance.userConfig.purchasedSkins.Contains(currentSkin.id))
        {
            priceAds.text = MainRoot.Instance.userConfig.customizationAdsWatched.ToString() + "/" + currentSkin.price.ToString() + " <sprite name=Ads>";
            if (MainRoot.Instance.userConfig.customizationAdsWatched>= currentSkin.price)
            {
                MainRoot.Instance.userConfig.purchasedSkins.Add(currentSkin.id);
                typeCoins.SetActive(false);
                typeAds.SetActive(false);
            }
        }
    }
    
    private void SkinChanged()
    {
        if (MainRoot.Instance.userConfig.purchasedSkins.Contains(currentSkin.id))
        {
            if (MainRoot.Instance.userConfig.currentSkin == currentSkin.id)
                skinPickedSign.SetActive(true);
            else
                skinPickedSign.SetActive(false);
        }
    }
}
