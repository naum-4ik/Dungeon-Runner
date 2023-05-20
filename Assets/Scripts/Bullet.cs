using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    [SerializeField] float bulletSpeed=20f;
    PlayerMovement player;
    float xSpeed;
    
    void Start()
    {
        myRigidbody=GetComponent<Rigidbody2D>();
        player=FindObjectOfType<PlayerMovement>();
        xSpeed = (player.transform.localScale.x * bulletSpeed);
        //transform.localScale = new Vector2(), 0f);
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
        //FlipSprite();
        

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
            Destroy(gameObject);
    }

    // void FlipSprite()
    // {
    //     bullet.localScale=player.transform.localScale;
    // }
    
}
