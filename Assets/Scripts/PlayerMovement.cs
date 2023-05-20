using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    Vector2 moveInput;

    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    [SerializeField] float runSpeed=5f;
    [SerializeField] float jumpSpeed=3f;
    [SerializeField] float climbSpeed=3f;
    [SerializeField] Vector2 deathKick = new Vector2 (10f,10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    
    float gravityAtStart;
    bool isAlive=true;

    void Start()
    {
        myRigidbody=GetComponent<Rigidbody2D>();
        myAnimator=GetComponent<Animator>();
        myBodyCollider=GetComponent<CapsuleCollider2D>();
        gravityAtStart=myRigidbody.gravityScale;
        myFeetCollider=GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(!isAlive) { return; }
        
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    
    void OnFire(InputValue value)
    {
        if(!isAlive) { return; }
        Vector2 bulletSpriteDirection = new Vector2(transform.localScale.x * 0.75f , 0.75f);
        bullet.transform.localScale = bulletSpriteDirection;
        //transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f)
        Instantiate(bullet, gun.position, transform.rotation);
    }


    void OnMove(InputValue value)
    {
        if(!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        //Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if(!isAlive) { return; }
        //moveInput = value.Get<Vector2>();
        if(value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            //Debug.Log("Skwosh");
        }
    }

    void Run()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        Vector2 playerVelocity = new Vector2(moveInput.x*runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void ClimbLadder()
    {   
        //bool playerVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            
            myRigidbody.gravityScale=0f;
            Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y*climbSpeed);
            myRigidbody.velocity = climbVelocity;
            myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        }
        else
        {
            myRigidbody.gravityScale=gravityAtStart;
            myAnimator.SetBool("isClimbing", false);
        }
    }



    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive=false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
