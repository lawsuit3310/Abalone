using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public Text UserName;
    public Text UserScore;
    public Image ScoreBackBoard;
    public Image PaulseScreen;
    public Image Continue;
    public double Gravity = 0.3;
    public double Score = 0.0;
    public int BackboardScale = 100;
    public AudioSource BGMManger;

    public ObstacleController ObstaclecController;

    public GameObject LeaderBoard;

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
        LeaderBoard.SetActive(false);
        Continue.gameObject.SetActive(false);
        Debug.Log(DeviceIdentifier);
        ScoreIncreaser = new Thread(AddScore);
        ScoreIncreaser.IsBackground = true;
        ThreadHandler.Add(ScoreIncreaser);
        ScoreIncreaser.Start();
        UserName = null;
        UserScore = null;
    }

    void Update()
    {
        Scoreboard.text = string.Format("{0:0.##}", Score);
        Scale = Score > 100 ? Math.Log10(Score) : Math.Log10(100);
        ScoreBackBoard.rectTransform.sizeDelta = new Vector2(100 + (float)Scale * BackboardScale ,ScoreBackBoard.rectTransform.sizeDelta.y);
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
        LeaderBoard.SetActive(true);
        SendResult();                                                       
    }

    private void SendResult()
    {
            m_Reference.Child("Result").Child(SystemInfo.deviceName).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log($"do not have references {SystemInfo.deviceName}");
                }

                else
                {
                    DataSnapshot dataSnapshot = task.Result;

                    if (Convert.ToDouble(dataSnapshot.Child("Score").GetValue(true)) >= Score) //현재 데이터 베이스에 있는 점수가 현재 점수와 같거나 더 큰 경우
                    {
                        CreateLeaderBoard();
                        return;
                    }
                }

                m_Reference.Child("Result").Child(SystemInfo.deviceName).Child("Score").SetValueAsync(Score);
                CreateLeaderBoard();
            });
        
    }

    private void CreateLeaderBoard()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Result").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Err! Couldn't get References from firebase!");
            }
            else if (task.IsCompleted) //파이어 베이스에서 값 가져와서 저장
            {
                DataSnapshot snapshot = task.Result;
                Dictionary<string, double> results = new Dictionary<string, double>(); 
                foreach (DataSnapshot data in snapshot.Children)
                {
                    results.Add(data.Key, Convert.ToDouble(data.Child("Score").GetValue(true)));
                }

                List<KeyValuePair<string,double>> tempValue = results.OrderByDescending(x => x.Value).ToList(); //내림차순 정렬
                results = tempValue.ToDictionary(x => x.Key, x => x.Value);
                
                tempValue = null;

                for (int i = 0; i < 1; i++) //캔버스에 있는 오브젝트 찾는 기능 구현 바람  --04-11--
                {
                    UserName = GameObject.FindGameObjectsWithTag("USERNAMESPACE")[i].GetComponent<Text>();
                    UserScore = GameObject.FindGameObjectsWithTag("USERSCORESPACE")[i].GetComponent<Text>();
                }
                
                UserName.text = " ";
                UserScore.text = " ";
                foreach (KeyValuePair<string,double> x in results)
                {
                    UserName.text += $"{x.Key}\n";
                    UserScore.text += $"{x.Value:0.0.##}\n";
                }
            }
        });
        Continue.gameObject.SetActive(true);
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

    public void WhenContinueClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//현재 씬 다시 불러옴

        //버그 수정 바람--0412--
    }

}
