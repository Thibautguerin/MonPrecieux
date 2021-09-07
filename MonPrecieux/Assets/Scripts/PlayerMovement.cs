using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private float h;
    private float v;

    private Rigidbody2D playerbody;
    void Start()
    {
        playerbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        
    }
    private void FixedUpdate()
    {
        playerbody.velocity = new Vector2(h, v) * speed;
    }

}
