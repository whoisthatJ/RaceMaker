using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelModel
{
	public int level;
	public int starCount;

	public LevelModel(int level, int starCount)
	{
		this.level = level;
		this.starCount = starCount;
	}
}
