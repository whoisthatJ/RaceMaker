using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour
{
	[SerializeField] private GameObject pausePnl;
	[SerializeField] private Button GooglePlayBtn;
	[SerializeField] private Button soundBtn;
	[SerializeField] private Button musicBtn;
	[SerializeField] private Button resumeBtn;
	[SerializeField] private Button homeBtn;
	[SerializeField] private Button restartBtn;

	private bool isPause;

	private void Start()
	{
		SetEnableButton();
	}

	private void OnEnable()
	{
		Preloader.BackButtonPressed += BackButton;
		TopBar.Pause += Pause;
        FinishPopup.continueOpen += Resume;
		GooglePlayBtn.onClick.AddListener(GooglePlay);
		soundBtn.onClick.AddListener(Sound);
		musicBtn.onClick.AddListener(Music);
		resumeBtn.onClick.AddListener(Resume);
		homeBtn.onClick.AddListener(Home);
		restartBtn.onClick.AddListener(RestartGame);
	}

	private void OnDisable()
	{
		Preloader.BackButtonPressed -= BackButton;
		TopBar.Pause -= Pause;
        FinishPopup.continueOpen -= Resume;
        GooglePlayBtn.onClick.RemoveAllListeners();
		soundBtn.onClick.RemoveAllListeners();
		musicBtn.onClick.RemoveAllListeners();
		resumeBtn.onClick.RemoveAllListeners();
		homeBtn.onClick.RemoveAllListeners();
		restartBtn.onClick.RemoveAllListeners();
	}

	private void Pause()
	{
        if (GameManager.instance.gameStarted)
        {
            if (!isPause)
            {
                pausePnl.SetActive(true);
                Time.timeScale = 0f;
                Preloader.Instance.AddPanelToTheList(pausePnl);
                isPause = true;
            }
            else
            {
                Resume();
            }
        }
	}

	private void GooglePlay()
	{

	}

	private void Sound()
	{
		if (MainRoot.Instance.userConfig.isSound)
		{
			MainRoot.Instance.userConfig.isSound = false;
			CSSoundManager.instance.SetVolumeSound(0);
		}
		else
		{
			MainRoot.Instance.userConfig.isSound = true;
			CSSoundManager.instance.SetVolumeSound(1);
		}
		SetEnableButton();
	}

	private void Music()
	{
		if (MainRoot.Instance.userConfig.isMusic)
		{
			MainRoot.Instance.userConfig.isMusic = false;
			CSSoundManager.instance.SetVolumeMusic(0);
		}
		else
		{
			MainRoot.Instance.userConfig.isMusic = true;
			CSSoundManager.instance.SetVolumeMusic(1);
		}
		SetEnableButton();
	}

	private void SetEnableButton()
	{
		if (MainRoot.Instance.userConfig.isSound)
			soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
		else
			soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
		if (MainRoot.Instance.userConfig.isMusic)
			musicBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
		else
			musicBtn.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
	}

	private void Resume()
	{
		isPause = false;
		Time.timeScale = 1f;
		pausePnl.SetActive(false);
		Preloader.Instance.RemovePanelFromTheList(pausePnl);
	}

	private void Home()
	{
		Time.timeScale = 1f;
		pausePnl.SetActive(false);
        MainMenuManager.instance.PanelSetActive(true);
        GameManager.instance.InitGame();
        //Preloader.Instance.LoadNewScene("Menu");
	}

	private void RestartGame()
	{		
		Time.timeScale = 1f;
        pausePnl.SetActive(false);
        MainMenuManager.instance.PanelSetActive(true);
        GameManager.instance.InitGame();
        FinishPopup.restartButtonClick?.Invoke();
        //GameManager.instance.StartGame();
        Preloader.Instance.LoadNewScene("Game");
    }

	private void BackButton()
	{
        if (GameManager.instance.gameStarted)
        {
            if (Time.timeScale == 0f)
                Resume();
            else
                Pause();
        }
	}
}
