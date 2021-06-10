using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsSave : MonoBehaviour
{
	public static LevelsSave instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this) Destroy(gameObject);
	}

	//Safe rewrite level
	public void LevelSaveAt(int level, int countStar)
	{
		if (countStar > 3) countStar = 3;
		List<LevelModel> levelModels = MainRoot.Instance.userConfig.levelModel;
		int index = levelModels.FindIndex(x => x.level == level);
		if (index >= 0)
		{
			levelModels.RemoveAt(index);
			levelModels.Insert(index, new LevelModel(level, countStar));
			int nextLevelIndex = levelModels.FindIndex(x => x.level == level + 1);
			if (nextLevelIndex < 0)
			{
				levelModels.Add(new LevelModel(level + 1, 0));
                //Analytics
				//ServiceFirebaseAnalytics.Instance.LogLevelUp(level + 1);
				//ServiceGameAnalytics.Instance.LogLevelUp(level + 1);
			}
		}
		else
		{
			levelModels.Add(new LevelModel(level, countStar));
			if (MainRoot.Instance.userConfig.maxLevels <= MainRoot.Instance.userConfig.currentLevel + 1)
			{
				levelModels.Add(new LevelModel(level + 1, 0));
				//Analytics
                //ServiceFirebaseAnalytics.Instance.LogLevelUp(level + 1);
                //ServiceGameAnalytics.Instance.LogLevelUp(level + 1);
			}
		}
	}

	public void LevelSave(int countStar)
	{
		if (countStar > 3) countStar = 3;
		List<LevelModel> levelModels = MainRoot.Instance.userConfig.levelModel;
		levelModels.Add(new LevelModel(MainRoot.Instance.userConfig.currentLevel, countStar));
		if (MainRoot.Instance.userConfig.maxLevels <= MainRoot.Instance.userConfig.currentLevel + 1)
		{
			levelModels.Add(new LevelModel(MainRoot.Instance.userConfig.currentLevel + 1, 0));
			//Analytics
			//ServiceFirebaseAnalytics.Instance.LogLevelUp(MainRoot.Instance.userConfig.currentLevel + 1);
			//ServiceGameAnalytics.Instance.LogLevelUp(MainRoot.Instance.userConfig.currentLevel + 1);
		}
	}

	//Remove level
	public void RemoveLevel(int level)
	{
		List<LevelModel> levelModels = MainRoot.Instance.userConfig.levelModel;
		int index = levelModels.FindIndex(x => x.level == level);
		if (index > -1)
		{
			levelModels.RemoveAt(index);
		}
	}
}
