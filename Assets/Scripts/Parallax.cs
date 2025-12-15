using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform camera;
    public float moveRate;
    
    [Space]
    private float startPoint;

    void Start()
    {
        startPoint = transform.position.x;
    }

    void Update()
    {
        transform.position = new Vector3(startPoint + camera.position.x * moveRate, transform.position.y, transform.position.z);    
    }
}
