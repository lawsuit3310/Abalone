using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    Rigidbody2D rigid;
    public GameManager gameManager;
    public GameObject Obstacle;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        CreateObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GravityControll(double Degree)
    {
        if (Degree > 1 || Degree < 0)
        {
            Debug.Log("Error"); return;
        }
        rigid.gravityScale = (float)Degree;
    }
    void CreateObstacle()
    {
        Instantiate(Obstacle);
        Invoke("CreateObstacle", 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
    }
}
