using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CSGameObjectExtension 
{
	public static Coroutine SetActiveDelay(this MonoBehaviour monoBehaviour, GameObject obj, float time)
    {
        return monoBehaviour.StartCoroutine(InvokeImpl(obj, time));
    }

	private static IEnumerator InvokeImpl(GameObject obj, float time)
    {
        yield return new WaitForSecondsRealtime(time);   
		obj.SetActive(false);
    }
}
