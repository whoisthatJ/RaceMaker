using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildInfo : MonoBehaviour 
{
	public static BuildInfo instance;

	[SerializeField] private TextMeshProUGUI applicationVersionTxt;
	private void Awake()
	{
		if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }		
	}

	private void Start () 
	{
    }
    public void SetText()
    {
        applicationVersionTxt.text = MainRoot.Instance.userConfig.levelXmlId.ToString() + "\n" + Application.version;
    }
}
