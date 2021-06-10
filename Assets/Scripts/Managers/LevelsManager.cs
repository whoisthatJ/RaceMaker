using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
	[Header("---UI---")]
	[SerializeField] private Button backBtn;

	[Header("---Grid Levels---")]
	[SerializeField] private GameObject itemLevel;
	[SerializeField] private GameObject itemsContainer;

	[Space(10)]
	[SerializeField] private Transform contentPanel;

	[Space(10)]
	[SerializeField] private ScrollRect scrollRect;

	[Header("---Settings Group Title---")]
	[Tooltip("Enable grouping objects?")]
	[SerializeField] private bool isEnableGroup;
	[Tooltip("Hide number level for lock levels?")]
	[SerializeField] private bool isHideNumberLevel;
	[SerializeField] private GameObject groupTitle;

	[Space(10)]
	[SerializeField] private List<GroupTitleModel> groupTitleTexts = new List<GroupTitleModel>();

	private int typeComplexity = 0;
	// Use this for initialization
	private void Start()
	{
		InitLevelsGrid();
		FinishPopup.isContinue = false;
	}

	private void OnEnable()
	{
		backBtn.onClick.AddListener(BackMenuClick);
	}

	private void OnDisable()
	{
		backBtn.onClick.RemoveAllListeners();
	}

	private void InitLevelsGrid()
	{
		int maxLevels = MainRoot.Instance.userConfig.maxLevels;
		GameObject cloneItemsContainer = null;
		GroupTitleModel groupTitleModel = groupTitleTexts[typeComplexity];
		int currentLevelCount = groupTitleModel.countLevel;
		int indexLevelCount = 0;
		Color groupTitleColor = groupTitleModel.colorLevel;
		for (int i = 0; i < maxLevels; i++)
		{
			if (typeComplexity < groupTitleTexts.Count && indexLevelCount == currentLevelCount && isEnableGroup)
			{
				typeComplexity++;
				if (typeComplexity < groupTitleTexts.Count)
				{
					groupTitleModel = groupTitleTexts[typeComplexity];
					currentLevelCount = groupTitleModel.countLevel;
					groupTitleColor = groupTitleModel.colorLevel;
					indexLevelCount = 0;
				}
			}
			SetGroupTitle(contentPanel, indexLevelCount, ref cloneItemsContainer, groupTitleColor);
			GameObject cloneItemLevel = Instantiate(itemLevel, cloneItemsContainer.transform);
			LevelModel levelModel = MainRoot.Instance.userConfig.levelModel.Find(x => x.level == i);
			if (levelModel != null)
			{
				Item item = cloneItemLevel.GetComponent<Item>();
				item.SetParameters((i + 1).ToString(), levelModel.starCount, groupTitleTexts[typeComplexity].openLevelColor);
				item.SetLevelID(i);
				item.SetInteractable(true);
			}
			else
			{
				Item item = cloneItemLevel.GetComponent<Item>();
				item.SetParameters((i + 1).ToString(), 0, Color.white);
				item.SetLevelID(i);
				item.SetLockImage(isHideNumberLevel);
			}
			indexLevelCount++;
		}
		SetNormalizePosition();
	}
    /// <summary>
    /// Set Up Position Scroll.
    /// </summary>
	private void SetNormalizePosition()
	{
		scrollRect.normalizedPosition = new Vector2(0, 1);
	}

	private void SetGroupTitle(Transform parent, int index, ref GameObject clone, Color color)
	{
		//Group Title
		if (index == 0 && isEnableGroup)
		{
			if (typeComplexity < groupTitleTexts.Count)
			{
				GameObject cloneGroupTitle = Instantiate(groupTitle, parent);

				cloneGroupTitle.GetComponent<GroupTitle>().SetParameters(groupTitleTexts[typeComplexity].groupName, groupTitleTexts[typeComplexity].colorTitle);
			}
		}
        //Container
		if (index % 4 == 0)
		{
			clone = Instantiate(itemsContainer, contentPanel);
			clone.GetComponent<Image>().color = color;
		}
	}

	private void BackMenuClick()
	{
		Preloader.Instance.LoadNewScene("Menu");
	}

	private void OnApplicationFocus(bool isFocus)
    {
        if (!isFocus)
        {
            ServiceFirebaseAnalytics.Instance.LogEnableMusic(Convert.ToInt32((MainRoot.Instance.userConfig.isMusic)));
            ServiceFirebaseAnalytics.Instance.LogEnableSound(Convert.ToInt32((MainRoot.Instance.userConfig.isSound)));
            ServiceGameAnalytics.Instance.LogEnableMusic(Convert.ToInt32((MainRoot.Instance.userConfig.isMusic)));
            ServiceGameAnalytics.Instance.LogEnableMusic(Convert.ToInt32((MainRoot.Instance.userConfig.isSound)));
        }
    }
}
