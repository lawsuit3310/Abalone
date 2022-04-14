using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class ObstacleController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject Obstacle;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateObstacle", 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateObstacle()
    {
        CancelInvoke("CreateObstacle");
        Instantiate(Obstacle);
        float pulse = gameManager.Score > 80? 0.25f : Mathf.Sqrt(float.Parse("" + 10 / (gameManager.Score == 0 ? 1 : gameManager.Score) ));
        Invoke("CreateObstacle", pulse);
    }

    public void ClearObstacle()
    {
        CancelInvoke("CreateObstacle");
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("OBSTACLE"))
        {
            Destroy(obj);
        }
    }


}
