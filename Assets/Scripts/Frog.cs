using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    private Rigidbody2D rb;
    private Collider2D coll;
    public Transform leftPoint;
    public Transform rightPoint;
    public LayerMask ground;
    public float speed = 1.5f;
    public float jumpForce = 5;
    public bool faceLeft = true;
    private float leftX;
    private float rightX;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        transform.DetachChildren();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    void Update()
    {
        SwithAnimation();
    }

    void Movement()
    {
        if (rb.velocity.y < 0.1f)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        if (faceLeft)
        {
            // 转向右
            if (transform.position.x < leftX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = false;
            }

            if (coll.IsTouchingLayers(ground))
            {
                rb.velocity = new Vector2(speed * (faceLeft ? -1 : 1), jumpForce);
                anim.SetBool("jumping", true);
                anim.SetBool("falling", false);
            }
        } else
        {
            // 转向左
            if (transform.position.x > rightX)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceLeft = true;
            }

            if (coll.IsTouchingLayers(ground))
            {
                rb.velocity = new Vector2(speed * (faceLeft ? -1 : 1), jumpForce);
                anim.SetBool("falling", false);
                anim.SetBool("jumping", true);
            }
        }
    }

    void SwithAnimation()
    {
        if (anim.GetBool("jumping"))
        {
            // 下落
            if (rb.velocity.y < 0.1f)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        } else if (coll.IsTouchingLayers(ground) && anim.GetBool("falling")) // 落地
        {
            anim.SetBool("falling", false);
        }
    }
}
