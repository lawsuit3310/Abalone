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
    #region Variable
    FirebaseApp app;
    DatabaseReference m_Reference;
    string DeviceIdentifier; //플레이 환경에 따라 변하는 변수

    public Text Scoreboard;
    public Image ScoreBackBoard;
    public Image PaulseScreen;
    public double Gravity = 0.3;
    public double Score = 0.0;
    public int BackboardScale = 100;
    public AudioSource BGMManger;

    public ObstacleController ObstaclecController;

    private double Scale;

    private ManualResetEvent _pauseEvent = new ManualResetEvent(true);

    private Thread ScoreIncreaser;
    private List<Thread> ThreadHandler = new List<Thread>();

    bool isGameStopped = false;
    #endregion
    
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
        Scale = Score > 100 ? Math.Log10(Score) : Math.Log10(100);
        ScoreBackBoard.rectTransform.sizeDelta = new Vector2(100 + (float)Scale * BackboardScale ,ScoreBackBoard.rectTransform.sizeDelta.y);
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
        Time.timeScale = 0;
        foreach (Thread thr in ThreadHandler)
        {
            thr.Abort();
        }
        BGMManger.Stop();
        Debug.Log("GameOver");
    }

    public void PaulseBtnOnClick()
    {
        if (!isGameStopped)
        {
            isGameStopped = true;
            Time.timeScale = 0;
            PaulseScreen.enabled = true;
            _pauseEvent.Reset();
        }
        else
        {
            isGameStopped = false;
            Time.timeScale = 1;
            PaulseScreen.enabled = false;
            _pauseEvent.Set();
        }
            
    }
}
