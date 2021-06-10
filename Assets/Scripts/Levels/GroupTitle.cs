using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GroupTitle : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI titleTxt;
    
	public void SetParameters(string text, Color color)
	{
		titleTxt.text = text;
		GetComponent<Image>().color = color;
	}
}
