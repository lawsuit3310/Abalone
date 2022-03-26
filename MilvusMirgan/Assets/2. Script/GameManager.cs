using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Database;


public class GameManager : MonoBehaviour
{
    FirebaseApp app;
    DatabaseReference m_Reference;

    // Start is called before the first frame update
    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
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
        });

        m_Reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    void Start()
    {
        m_Reference.Child("0").SetValueAsync("a");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
