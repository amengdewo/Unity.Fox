using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public Collider2D coll;
    public BoxCollider2D box;
    public LayerMask ground;
    public AudioSource jumpSound;
    public AudioSource hurtSound;
    public AudioSource cherrySound;
    public Transform ceilingPoint;
    public Transform groundPoint;

    [Space]
    public float speed = 400;
    public float jumpForce = 50;
    public int cherry = 0;
    public int gem = 0;
    public Text cherryNumber;
    private bool isHurt = false;
    private bool isGround = false;
    private int extraJump = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
    }


    void FixedUpdate()
    {
        if (!isHurt)
        {
            Movement();
            // Jump();
        }
        SwithAnimation();
        isGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, ground);
    }

    void Update()
    {
        Crouch();
        Jump2();
    }

    // 移动
    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float direction = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);
            animator.SetFloat("running", Math.Abs(horizontal));
        }
        if (direction != 0)
        {
            transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    // 跳跃
    void Jump()
    {
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            jumpSound.Play();
            animator.SetBool("jumping", true);
        }
    }

    // 二段跳
    void Jump2()
    {
        if (isGround)
        {
            extraJump = 1;
        }
        if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJump--;
            jumpSound.Play();
            animator.SetBool("jumping", true);
        }
        if (Input.GetButtonDown("Jump") && extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpSound.Play();
            animator.SetBool("jumping", true);
        }
    }

    // 蹲下
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(ceilingPoint.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch") && coll.IsTouchingLayers(ground))
            {
                animator.SetBool("crouching", true);
                box.enabled = false;
            } else
            {
                animator.SetBool("crouching", false);
                box.enabled = true;
            }
        }
    }

    void SwithAnimation()
    {
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", true);
        }

        if (animator.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
        }  else if (isHurt)
        {
            animator.SetBool("hurting", true);
            if (Math.Abs(rb.velocity.x) < 0.1f)
            {
                isHurt = false;
                animator.SetBool("hurting", false);
                animator.SetFloat("running", 0);
            }
        } else if (coll.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", false);
        }
    }

    // 触发检测
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            cherrySound.Play();
            collision.GetComponent<Animator>().Play("Got");
        }
        if (collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
        }
    }

    // 碰撞检测
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (animator.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetBool("jumping", true);
            } else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                hurtSound.Play();
                rb.velocity = new Vector2(-10, rb.velocity.y);
                isHurt = true;
            } else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                hurtSound.Play();
                rb.velocity = new Vector2(10, rb.velocity.y);
                isHurt = true;
            }
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CherryCount()
    {
        cherry++;
        cherryNumber.text = cherry.ToString();
    }
}
