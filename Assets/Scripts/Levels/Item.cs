using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Item : MonoBehaviour 
{
	[SerializeField] private TextMeshProUGUI title;

    [Space(10)]
	[SerializeField] private List<GameObject> stars = new List<GameObject>();

	[Space(10)]
	[SerializeField] private Image lockImage;

	[HideInInspector]
	[SerializeField] private Button itemBtn;

	[Space(10)]
	[SerializeField] private Image image;
    
	//Install button level
	private int numberLevel;

	private void OnEnable()
    {
		itemBtn.onClick.AddListener(LoadGame);
    }

    private void OnDisable()
    {
        itemBtn.onClick.RemoveAllListeners();
    }

	private void LoadGame()
    {
		MainRoot.Instance.userConfig.currentLevel = numberLevel;
        Preloader.Instance.LoadNewScene("Game");
    }

    /// <summary>
	/// Install parameters for level
    /// </summary>
    /// <param name="titleLevel">Title level</param>
    /// <param name="count">Star saves count</param>
	public void SetParameters(string titleLevel, int count, Color color)
	{
		title.gameObject.SetActive(true);
		image.color = color;
		title.text = titleLevel;
		if (count == 0) return;
		for (int i = 0; i < count; i++)
		{
			stars[i].gameObject.SetActive(true);
		}
	}
    /// <summary>
    /// Sets the lock image for locks levels
    /// </summary>
    /// <param name="isHideNumberLevel">If set to <c>true</c> is hide number text level.</param>
	public void SetLockImage(bool isHideNumberLevel)
	{
		GetComponent<Image>().enabled = false;
		lockImage.enabled = true;
		if (isHideNumberLevel)
		title.gameObject.SetActive(false);
		else title.gameObject.SetActive(true);
	}

    //Level Number
	public void SetLevelID(int numberLevel)
	{
		this.numberLevel = numberLevel;
	}

	public void SetInteractable(bool isFlag)
	{
		if (isFlag)
		{
			image.enabled = true;
			lockImage.enabled = false;
		}
		itemBtn.interactable = isFlag;
	}
}
