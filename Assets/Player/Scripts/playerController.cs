using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float speed = 10f;

    [Header("Jump")]
    public float jumpForce = 14f;
    public float fallMultiplier = 2f;

    [Header("Grounded")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.3f;

    [Header("Particle System")]
    public ParticleSystem psRun;

    //private variables  
    private bool isGrounded;
    private Rigidbody2D rb;
    private float moveX;
    private Animator anim;
    private bool isFacingRight = true;
    private bool isRunning;
    private bool canJump;
    private bool isJumping;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
               
        CheckColliders();
        CheckJump();
        checkAnims();

        FacingDirection();

        if(canJump)
        {
            Jump();           
        }
    }

    private void FixedUpdate()
    {
        Move();    
        FallAfterJump();
    }

    private void checkAnims()
    {
        //Run
        if (moveX != 0)
        {
            anim.SetBool("isRunning", true);            
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        //Jump
    }

    private void CheckColliders()
    {
        //check if player on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public void CheckJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("takeOf");
            canJump = true;         
        }
        else
        {
            canJump = false;          
        }

        if(isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isJumping", true);
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
    }

    private void Jump()
    {                
        rb.velocity = Vector2.up * jumpForce;        
    }

    private void FallAfterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            anim.SetBool("isFalling", true);   
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight; //true = false & false = true
        //transform.Rotate(0.0f, 180.0f, 0.0f);
        transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void FacingDirection()
    {
        //change facing direction
        if (isFacingRight && moveX < 0) //moving right
        {
            Flip();
        }
        else if (!isFacingRight && moveX > 0) //moving left
        {
            Flip();
        }
    }

    private void OnDrawGizmos() 
    {

        //GroundCheck
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
