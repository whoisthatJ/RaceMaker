using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomInvoke
{
    // Safe Call Invoke
    public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
    {
        return monoBehaviour.StartCoroutine(InvokeImpl(action, time));
    }
    // Safe Call StopInvoke
    public static void StopInvoke(this MonoBehaviour monoBehaviour, Action action)
    {
        monoBehaviour.StopCoroutine(action.Method.Name);
    }
    private static IEnumerator InvokeImpl(Action action, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        action();
    }
    // Safe Call InvokeRepeating
    public static Coroutine InvokeRepeating(this MonoBehaviour monoBehaviour, Action action, float delay, float repeatRete)
    {
        return monoBehaviour.StartCoroutine(InvokeRepeatingImpl(action, delay, repeatRete));
    }

    private static IEnumerator InvokeRepeatingImpl(Action action, float delay, float repeatRete)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            yield return new WaitForSeconds(repeatRete);
            action();
        }
    }
}