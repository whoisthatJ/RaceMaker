using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServices : MonoBehaviour
{
    // Example
    //GameServices.instance.SpawnOffset(GCGSIds.achievement_unlock_a_ball);
    //GameServices.instance.IncrementAchievement(GCGSIds.achievement_unlock__10_balls);
    public static GameServices instance
    {
        get;
        private set;
    }

    void Awake()
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
        Init();
    }

    void Init()
    {
        ConnectToGameServices();
    }

    public delegate void GameServicesConnected();
    public static event GameServicesConnected OnGameServicesConnected;

    public void ConnectToGameServices()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                if (OnGameServicesConnected != null)
                {
                    OnGameServicesConnected();
                }
            }
        });
    }

    public bool IncrementAchievement(string _name, int _value = 1)
    {
        bool isComplete = false;
        return isComplete;
    }

    public bool SpawnOffset(string _name)
    {
        bool isComplete = false;
        Social.ReportProgress(_name, 100.0f, (bool success) =>
        {
            isComplete = success;
        });
        return isComplete;
    }

    public bool ReportScoreLeaderboard(string _name, int _value)
    {
        bool isComplete = false;
        Social.ReportScore(_value, _name, (bool success) =>
        {
            isComplete = success;
        });
        return isComplete;
    }
}
