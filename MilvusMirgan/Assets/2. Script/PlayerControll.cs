using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    // Start is called before the first frame update

    Transform trans;
    Rigidbody2D rigid;
    private void Awake()
    {
        trans = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(trans.position.y != -2)
            trans.position = new Vector2(trans.position.x, -2);
        if (trans.position.x < -8.2f)
            trans.position = new Vector2(-8.2f, trans.position.y);
        else if (trans.position.x > 8.2f)
            trans.position = new Vector2(8.2f, trans.position.y);
    }
}
