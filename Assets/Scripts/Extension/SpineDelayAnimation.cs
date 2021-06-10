using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineDelayAnimation : MonoBehaviour
{
	[SerializeField] private float delay = .2f;
	private MeshRenderer rend;

	private void Awake()
	{
		rend = GetComponent<MeshRenderer>();
	}

	// Use this for initialization
	void Start()
	{
		StartCoroutine(DelayView());
	}

	private IEnumerator DelayView()
	{
		yield return new WaitForSeconds(delay);
		if (rend != null)
		rend.enabled = true;
	}
}
