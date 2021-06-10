using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Confirmation : MonoBehaviour
{
    public static Confirmation Instance;
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI notificationTxt;
    [SerializeField] Button yesBtn;
    [SerializeField] Button noBtn;

    public delegate void BtnCallback();
    BtnCallback noCallback, yesCallback;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        Preloader.BackButtonPressed += No;
        yesBtn.onClick.AddListener(Yes);
        noBtn.onClick.AddListener(No);
    }

    void OnDisable()
    {
        Preloader.BackButtonPressed -= No;
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();
        noCallback = null;
        yesCallback = null;
    }

    public void Open(string _text, BtnCallback _callbackYes = null, BtnCallback _callbackNo = null)
    {
        noBtn.gameObject.SetActive(true);
        panel.SetActive(true);
        notificationTxt.text = _text;
        yesCallback = _callbackYes;
        noCallback = _callbackNo;
        Preloader.Instance.AddPanelToTheList(panel);
    }

    private void Yes()
    {
        panel.SetActive(false);
        if (yesCallback != null)
        {
            yesCallback();
            yesCallback = null;
            noCallback = null;
        }
        Preloader.Instance.RemovePanelFromTheList(panel);
    }

    private void No()
    {
        panel.SetActive(false);
        if (noCallback != null)
        {
            noCallback();
            yesCallback = null;
            noCallback = null;
        }
        Preloader.Instance.RemovePanelFromTheList(panel);
    }
}
