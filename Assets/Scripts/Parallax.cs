using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float moveRate;
    public bool lockY;

    [Space]
    private float startPointX;
    private float startPointY;

    void Start()
    {
        startPointX = transform.position.x;
        startPointY = transform.position.y;
    }

    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(startPointX + cam.position.x * moveRate, transform.position.y);    
        } else
        {
            transform.position = new Vector2(startPointX + cam.position.x * moveRate, startPointX + cam.position.y * moveRate);    
        }
    }
}
