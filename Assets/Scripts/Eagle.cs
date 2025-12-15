using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Eagle : Enemy
{
    public Rigidbody2D rb;
    public Transform topPoint;
    public Transform bottomPoint;

    [Space]
    public float speed = 1.5f;
    public float topY;
    public float bottomY;

    private bool isUp = true;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        topY = topPoint.position.y;
        bottomY = bottomPoint.position.y;
        Destroy(topPoint.gameObject);
        Destroy(bottomPoint.gameObject);
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > topY)
            {
                isUp = false;
            }
        } else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < bottomY)
            {
                isUp = true;
            }
        }
    }
}
