using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    private void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GAMEMANAGER"))
        {
            gameManager = obj.GetComponent<GameManager>();
        }
        
        gameObject.transform.position = new Vector2(Random.Range(-8f,8f), 6);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.gameObject.CompareTag("DEADLINE"))
        {
            Destroy(this.gameObject);
        }

        else if (collision.gameObject.CompareTag("PLAYER"))
        {
            gameManager.GameOver();
        }
    }
}
