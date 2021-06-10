using UnityEngine;
using Facebook.Unity;
using System.Collections.Generic;

public class ServiceFacebook : MonoBehaviour {

    public static ServiceFacebook Instance
    {
        get;
        private set;
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
            });
        }
    }

    // Unity will call OnApplicationPause(false) when an app is resumed
    // from the background
    void OnApplicationPause(bool pauseStatus)
    {
        // Check the pauseStatus to see if we are in the foreground
        // or background
        if (!pauseStatus)
        {
            //app resume
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() => {
                    FB.ActivateApp();
                });
            }
        }
    }
    private const string EventLevelUp = "level_up";
    private const string EventOutOfMoves = "out_of_moves";
    private const string EventAdWatched = "ad_watched";

    public void LogLevelUp(int level)
    {
        Dictionary<string, System.Object> parameters = new Dictionary<string, System.Object>();
        parameters.Add("level", level);
        FB.LogAppEvent(EventLevelUp, null, parameters);
    }

    public void OutOfMoves(int level)
    {
        FB.LogAppEvent(EventOutOfMoves, level);
    }

    public void AdWatched(int level)
    {
        FB.LogAppEvent(EventAdWatched, level);
    }
}
