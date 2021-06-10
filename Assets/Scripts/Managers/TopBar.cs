using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TopBar : MonoBehaviour {

    public static TopBar instance;

    [SerializeField] Button settingsBtn;
    [SerializeField] Button pauseBtn;
    [SerializeField] TextMeshProUGUI softCurrencyTxt;
    [SerializeField] TextMeshProUGUI highScoreTxt;
    [SerializeField] GameObject newBest;
    [SerializeField] TextMeshProUGUI coinTxt;

    public delegate void PauseEvent();
    public static PauseEvent Pause;

    private int lastSoftCurrency;

    // Use this for initialization
    void Start () {
		
        settingsBtn.gameObject.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
        highScoreTxt.gameObject.SetActive(true);
        lastSoftCurrency = MainRoot.Instance.userConfig.softCurrency;
        RenderSoftCurrency();
        RenderHighScore();
	}
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this) Destroy(gameObject);
    }
    private void OnEnable()
    {
        FinishPopup.completeOpen += PlayerHighScore;
        FinishPopup.defeatOpen += PlayerLost;
        FinishPopup.continueOpen += PlayerContinue;
        FinishPopup.restartButtonClick += PlayerRestart;
        UserConfig.OnSoftCurrencyAmountChanged += RenderSoftCurrency;
        UserConfig.OnHighScoreChanged += RenderHighScore;
        settingsBtn.onClick.AddListener(OpenSettings);
        pauseBtn.onClick.AddListener(TogglePause);
    }

    private void OnDisable()
    {
        FinishPopup.completeOpen -= PlayerHighScore;
        FinishPopup.defeatOpen -= PlayerLost;
        FinishPopup.continueOpen -= PlayerContinue;
        FinishPopup.restartButtonClick -= PlayerRestart;
        UserConfig.OnSoftCurrencyAmountChanged -= RenderSoftCurrency;
        UserConfig.OnHighScoreChanged -= RenderHighScore;
        settingsBtn.onClick.RemoveAllListeners();
        pauseBtn.onClick.RemoveAllListeners();
    }
    private void RenderSoftCurrency()
    {
        softCurrencyTxt.text = MainRoot.Instance.userConfig.softCurrency + "<sprite name=Coin>";
        if (lastSoftCurrency < MainRoot.Instance.userConfig.softCurrency)
            CoinDT(MainRoot.Instance.userConfig.softCurrency - lastSoftCurrency);
        lastSoftCurrency = MainRoot.Instance.userConfig.softCurrency;
    }
    private void RenderHighScore()
    {
        highScoreTxt.text = MainRoot.Instance.userConfig.highScore.ToString() + "<sprite name=BestScore>";
    }
    private void CoinDT(int coins)
    {
        coinTxt.gameObject.SetActive(true);
        //goalCoinText.rectTransform.DOAnchorPos(new Vector2(100f, 600f), 0.25f).ChangeStartValue(new Vector2(60f, 700f));
        coinTxt.transform.localScale = Vector3.one * 0.3f;
        coinTxt.alpha = 0f;
        coinTxt.DOFade(1f, 0.17f);
        coinTxt.text = "+" + coins + "<sprite name=Coin>";
        Sequence coinScaleSeq = DOTween.Sequence();
        coinScaleSeq.Append(coinTxt.transform.DOScale(1.2f, 0.25f).SetEase(Ease.OutCubic));
        coinScaleSeq.Append(coinTxt.transform.DOScale(1f, 0.25f).SetEase(Ease.InCubic));
        coinScaleSeq.AppendInterval(0.5f);
        coinScaleSeq.AppendCallback(() =>
        {
            coinTxt.gameObject.SetActive(false);
        });
    }
    private void OpenSettings()
    {
        Settings.Instance.OpenSettings();
    }

    private void TogglePause()
    {
        if (Pause != null)
            Pause();
    }
    //change top bar after player won
    private void PlayerHighScore()
    {
		pauseBtn.gameObject.SetActive(false);
		settingsBtn.gameObject.SetActive(true);
        highScoreTxt.gameObject.SetActive(true);
        newBest.SetActive(true);
    }
    //change top bar after player lost
    private void PlayerLost()
    {
		pauseBtn.gameObject.SetActive(false);
        settingsBtn.gameObject.SetActive(true);
        highScoreTxt.gameObject.SetActive(true);
    }
    //change top bar after player continue
    public void PlayerContinue()
    {
        pauseBtn.gameObject.SetActive(true);
        settingsBtn.gameObject.SetActive(false);
        highScoreTxt.gameObject.SetActive(false);
        RenderSoftCurrency();
        RenderHighScore();
        newBest.SetActive(false);
    }
    public void PlayerRestart()
    {
        pauseBtn.gameObject.SetActive(false);
        settingsBtn.gameObject.SetActive(true);
        highScoreTxt.gameObject.SetActive(true);
        RenderSoftCurrency();
        RenderHighScore();
        newBest.SetActive(false);
    }
}
