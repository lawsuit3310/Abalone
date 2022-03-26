using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rigid;
    public GameManager gameManager;
    public AudioClip DestroySFX;
    AudioSource AudioSource;
    AudioManager audioManager;
    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GAMEMANAGER"))
        {
            gameManager = obj.GetComponent<GameManager>();
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AUDIOMANAGER"))
        {
            audioManager = obj.GetComponent<AudioManager>();
        }

        gameObject.transform.position = new Vector2(Random.Range(-8f,8f), 6);
    }
    private void Update()
    {
        GravityControll(Mathf.Sqrt((float)gameManager.Score/100));
    }
    public void GravityControll(double Degree)
    {
        if (Degree < 0)
        {
            Debug.Log("Error"); return;
        }
        else if (Degree > 1)
        {
            rigid.mass = (float)Degree;
        }
        rigid.gravityScale = (float)Degree;
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

    private void OnDestroy()
    {
        try
        {
            audioManager.PlaySFX(DestroySFX); 
        }
        catch
        {
            return;
        }
    }


}
