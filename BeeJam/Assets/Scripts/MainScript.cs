using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk, 
    attack,
    interact,
    stagger,
    idle
}

public class MainScript : MonoBehaviour
{
    public PlayerState currentState;
    public bool facingRight = true;
    public float speed;
    private Rigidbody2D rb;
    public Vector3 change;
    private Animator anim;
    public float moveX;
    public float moveY;

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        anim.SetFloat("moveX", 0);
        anim.SetFloat("moveY", -1);
    }

    /*
    private void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        if (moveX < 0.0f && facingRight == false)
        {
        FlipPlayer();
        } else if (moveX > 0.0f && facingRight == true)
        {
        FlipPlayer();
        }
        //Physics
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * speed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
    }
    */

    // Update is called once per frame
    
     void Update()
     {
         //moveX = Input.GetAxisRaw("Horizontal");
        change = Vector2.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown("space") && currentState != PlayerState.attack) //&& currentState != PlayerState.stagger
        {
            StartCoroutine(AttackCo());
        }
        else if(currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationandMove();
        }
        //if(change != Vector3.zero)
        //{
           // MoveCharacter();
         //}
     }

    private IEnumerator AttackCo()
    {
        anim.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        anim.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
        rb.velocity = Vector2.zero;
    }
    

    void UpdateAnimationandMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            anim.SetFloat("moveX", change.x);
            anim.SetFloat("moveY", change.y);
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }
    

    void MoveCharacter()
    {
        change.Normalize();
        rb.MovePosition(
            transform.position + change * speed * Time.deltaTime);
        //transform.position + change * speed * Time.deltaTime);
        //if(moveX < 0.0f && facingRight == false)
        //{
            //FlipPlayer();
        //} else if (moveX > 0.0f && facingRight == true)
        //{
            //FlipPlayer();
        //}
        //Physics
        //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * speed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
    }

    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo(knockTime));
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector2.zero;
            rb.GetComponent<Enemy>().currentState = EnemyState.idle;
        }
    }

    /*
    void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    */
}
