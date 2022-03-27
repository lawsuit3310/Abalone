using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObstacleController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject Obstacle;
    // Start is called before the first frame update
    void Start()
    {
        CreateObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateObstacle()
    {
        Instantiate(Obstacle);
        float pulse = gameManager.Score > 80? 0.3f : Mathf.Sqrt(float.Parse("" + 10 / (gameManager.Score == 0 ? 1 : gameManager.Score) ));
        Debug.Log(pulse);
        Invoke("CreateObstacle", pulse);
    }


}
