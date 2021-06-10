using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public static Notification Instance;
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI notificationTxt;
    [SerializeField] Button closeBtn;

    public delegate void CloseCallback();
    CloseCallback closeCallback;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        Preloader.BackButtonPressed += Close;
        closeBtn.onClick.AddListener(Close);
    }

    void OnDisable()
    {
        Preloader.BackButtonPressed -= Close;
        closeBtn.onClick.RemoveAllListeners();
        closeCallback = null;
    }

    public void Open(string _text, CloseCallback _callback = null)
    {
        closeBtn.gameObject.SetActive(true);
        panel.SetActive(true);
        notificationTxt.text = _text;
        closeCallback = _callback;
        Preloader.Instance.AddPanelToTheList(panel);
    }

    private void Close()
    {
        panel.SetActive(false);
        if (closeCallback != null)
        {
            closeCallback();
            closeCallback = null;
        }
        Preloader.Instance.RemovePanelFromTheList(panel);
    }
}
