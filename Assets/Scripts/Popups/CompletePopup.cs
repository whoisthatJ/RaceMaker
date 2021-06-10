using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CompletePopup : MonoBehaviour
{
	[SerializeField] private List<GameObject> starsImage = new List<GameObject>();
	[SerializeField] private GameObject starsContainer;

	public void SetStars(int stars = 0)
	{
		if (stars > 0)
		{
			starsContainer.SetActive(true);
			for (int i = 0; i < stars; i++)
			{
				starsImage[i].SetActive(true);
			}
		}
		else
		{
			starsContainer.SetActive(false);
		}
	}
}
