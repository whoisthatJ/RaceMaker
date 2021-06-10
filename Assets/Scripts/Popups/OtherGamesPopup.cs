using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Firebase;
//using Firebase.DynamicLinks;

public class OtherGamesPopup : MonoBehaviour
{
    [Header("---UI---")]
    [SerializeField] private Button backBtn;

    [Space(10)]
    [SerializeField] private GameObject container;
    [SerializeField] private Button linkToSkeletonBtn;
    [SerializeField] private Button linkToScrollSoccerBtn;
    [SerializeField] private Button linkToHolderBtn;
    [SerializeField] private Button linkToTTUBtn;

    // Use this for initialization
    private void Start()
    {

    }

    private void OnEnable()
    {
        InitListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void InitListeners()
    {
        Preloader.BackButtonPressed += BackBtnClick;
        backBtn.onClick.AddListener(BackBtnClick);
        linkToSkeletonBtn.onClick.AddListener(LinkToSkeleton);
        linkToScrollSoccerBtn.onClick.AddListener(LinkToScrollSoccer);
        linkToHolderBtn.onClick.AddListener(LinkToHolder);
        linkToTTUBtn.onClick.AddListener(LinkToTTU);
    }

    private void RemoveListeners()
    {
        Preloader.BackButtonPressed -= BackBtnClick;
        backBtn.onClick.RemoveAllListeners();
        linkToSkeletonBtn.onClick.RemoveAllListeners();
        linkToScrollSoccerBtn.onClick.RemoveAllListeners();
        linkToHolderBtn.onClick.RemoveAllListeners();
        linkToTTUBtn.onClick.RemoveAllListeners();
    }

    private void BackBtnClick()
    {
        container.SetActive(false);
        Preloader.Instance.RemovePanelFromTheList(container);
    }

    public void OpenPanel()
    {
        container.SetActive(true);
        Preloader.Instance.AddPanelToTheList(container);
    }

    //This method tunes in inscpector in buttons
    public void OpenOtherGamesDirect(string path)
    {
        Application.OpenURL(path);
    }

    private void LinkToScrollSoccer()
    {
        ServiceFirebaseAnalytics.Instance.LogSoccerClicked();
        ServiceGameAnalytics.Instance.LogSoccerClicked();
        // 	var components = new Firebase.DynamicLinks.DynamicLinkComponents(
        //     // The base Link.
        //     new System.Uri("https://play.google.com/store/apps/details?id=kz.snailwhale.soccer"),
        //     // The dynamic link domain.
        //     "https://scroll.page.link/?link=https://play.google.com/store/apps/details?id%3Dkz.snailwhale.soccer&apn=kz.snailwhale.soccer")
        //     {
        //         //IOSParameters = new Firebase.DynamicLinks.IOSParameters("com.example.ios"),
        //         AndroidParameters = new Firebase.DynamicLinks.AndroidParameters(
        //   "kz.snailwhale.soccer"),
        //     };
    }

    private void LinkToSkeleton()
    {
        ServiceFirebaseAnalytics.Instance.LogSkeletonClicked();
        ServiceGameAnalytics.Instance.LogSkeletonClicked();
        // 	var components = new Firebase.DynamicLinks.DynamicLinkComponents(
        //     // The base Link.
        //     new System.Uri("https://play.google.com/store/apps/details?id=kz.snailwhale.skeleton"),
        //     // The dynamic link domain.
        //     "https://trickybones.page.link/?link=https://play.google.com/store/apps/details?id%3Dkz.snailwhale.skeleton&apn=kz.snailwhale.skeleton")
        //     {
        //         //IOSParameters = new Firebase.DynamicLinks.IOSParameters("com.example.ios"),
        //         AndroidParameters = new Firebase.DynamicLinks.AndroidParameters(
        //   "kz.snailwhale.skeleton"),
        //     };
    }

    private void LinkToTTU()
    {
        ServiceFirebaseAnalytics.Instance.LogTTUClicked();
        ServiceGameAnalytics.Instance.LogTTUClicked();
    }

    private void LinkToHolder()
    {
        ServiceFirebaseAnalytics.Instance.LogHolderClicked();
        ServiceGameAnalytics.Instance.LogHolderClicked();
    }
}
