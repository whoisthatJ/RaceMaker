using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI notificationTxt;
    [SerializeField] Button closeBtn;
    [SerializeField] Button yesBtn;
    [SerializeField] Button noBtn;

    public delegate void CloseCallback();
    CloseCallback closeCallback;

    public delegate void YesCallback();
    YesCallback yesCallback;

    public delegate void NoCallback();
    NoCallback noCallback;

    void Start()
    {
        closeBtn.onClick.AddListener(Close);
        yesBtn.onClick.AddListener(Yes);
        noBtn.onClick.AddListener(No);
    }

    void OnDisable()
    {
        closeBtn.onClick.RemoveAllListeners();
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();
        closeCallback = null;
        yesCallback = null;
        noCallback = null;
    }

    public void Open(string _text, CloseCallback _callback = null)
    {
        yesBtn.gameObject.SetActive(false);
        noBtn.gameObject.SetActive(false);
        closeBtn.gameObject.SetActive(true);
        panel.SetActive(true);
        notificationTxt.text = _text;
        closeCallback = _callback;
    }

    public void OpenQuestion(string _text, YesCallback _yesCallback = null, NoCallback _noCallback = null)
    {
        yesBtn.gameObject.SetActive(true);
        noBtn.gameObject.SetActive(true);
        closeBtn.gameObject.SetActive(false);
        panel.SetActive(true);
        notificationTxt.text = _text;
        yesCallback = _yesCallback;
        noCallback = _noCallback;
    }

    public void Close()
    {        
        panel.SetActive(false);
        if (closeCallback != null)
        {
            closeCallback();
            closeCallback = null;
        }
        yesCallback = null;
        noCallback = null;
    }

    void Yes()
    {        
        panel.SetActive(false);
        if (yesCallback != null)
        {
            yesCallback();
            yesCallback = null;
        }
        noCallback = null;
    }

    void No()
    {        
        panel.SetActive(false);
        if (noCallback != null)
        {
            noCallback();
            noCallback = null;
        }
        yesCallback = null;
    }
}
