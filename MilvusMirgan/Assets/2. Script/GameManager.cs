using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Database;
using System.Threading;

public class GameManager : MonoBehaviour
{
    FirebaseApp app;
    DatabaseReference m_Reference;
    string DeviceIdentifier; //플레이 환경에 따라 변하는 변수

    public Text Scoreboard;
    public double Gravity = 0.3;
    public double Score = 0.0;

    public ObstacleController ObstaclecController;

    private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
    private ManualResetEvent _pauseEvent = new ManualResetEvent(true);

    private Thread ScoreIncreaser;
    private List<Thread> ThreadHandler = new List<Thread>();

    bool isGameStopped = false;

    private void Awake()
    {
        #region 공부필요
        /*FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.

                return;

            }
            else
            {
                Debug.LogError(string.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });*/
        #endregion

        m_Reference = FirebaseDatabase.DefaultInstance.RootReference;
        DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;

    }
    void Start()
    {
        Debug.Log(DeviceIdentifier);
        ScoreIncreaser = new Thread(AddScore);
        ScoreIncreaser.IsBackground = true;
        ThreadHandler.Add(ScoreIncreaser);
        ScoreIncreaser.Start();
    }

    void Update()
    {
        Scoreboard.text = string.Format("{0:0.##}", Score);
    }

    void UploadScore(int Score)
    {
        if (DeviceIdentifier != null)
            m_Reference.Child(DeviceIdentifier).Child("Score").SetValueAsync(Score + "");
        else
            m_Reference.Child("Guest").Child("Score").SetValueAsync(Score+"");
    }
    void AddScore()
    {
        while (true)
        {
            while (_pauseEvent.WaitOne())
            {
                Score = Convert.ToDouble(Scoreboard.text);
                Score = Score + 0.01;
                Thread.Sleep(10);
            }
        }
    }

    public void GameOver()
    {
        foreach (Thread thr in ThreadHandler)
        {
            thr.Abort();
        }
        
        Debug.Log("GameOver");
    }

    public void PaulseBtnOnClick()
    {
        if (!isGameStopped)
        {
            isGameStopped = true;
            Time.timeScale = 0;
            _pauseEvent.Reset();
        }
        else
        {
            isGameStopped = false;
            Time.timeScale = 1;
            _pauseEvent.Set();
        }
            
    }
}
