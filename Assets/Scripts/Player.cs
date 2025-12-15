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
    [Space]
    public float speed = 400;
    public float jumpForce = 50;
    public int cherry = 0;
    public int gem = 0;
    public Text cherryNumber;
    public bool isHurt = false;

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
            Jump();
        }
        SwithAnimation();
    }

    void Update()
    {
        Crouch();
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
            jumpSound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
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
                // box.offset = new Vector2(box.offset.x, -0.6f);
            } else
            {
                animator.SetBool("crouching", false);
                box.enabled = true;
                // box.offset = new Vector2(box.offset.x, -0.1f);
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
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
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
