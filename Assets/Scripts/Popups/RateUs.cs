using UnityEngine;
using UnityEngine.UI;

public class RateUs : MonoBehaviour
{
    public static RateUs Instance;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button laterBtn;
    [SerializeField] private Button neverBtn;
    [SerializeField] private Button rateBtn;

    private const string ANDROID_RATE_URL = "market://details?id=kz.snailwhale.soccer";

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Preloader.BackButtonPressed += ClosePanel;
        laterBtn.onClick.AddListener(Later);
        neverBtn.onClick.AddListener(Never);
        rateBtn.onClick.AddListener(Rate);
    }

    private void OnDisable()
    {
        Preloader.BackButtonPressed -= ClosePanel;
        laterBtn.onClick.RemoveAllListeners();
        neverBtn.onClick.RemoveAllListeners();
        rateBtn.onClick.RemoveAllListeners();
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        Preloader.Instance.AddPanelToTheList(panel);
    }

    private void ClosePanel()
    {
        panel.SetActive(false);
        Preloader.Instance.RemovePanelFromTheList(panel);
    }

    private void Later()
    {
        ClosePanel();
    }

    private void Never()
    {
        MainRoot.Instance.userConfig.stopShowingRateUs = true;
        ClosePanel();
    }

    private void Rate()
    {
        MainRoot.Instance.userConfig.stopShowingRateUs = true;
        Application.OpenURL(ANDROID_RATE_URL);
        ClosePanel();
    }
}
