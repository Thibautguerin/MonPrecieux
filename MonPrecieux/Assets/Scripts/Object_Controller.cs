using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Controller : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        //Get position of player
        transform.position = new Vector2(transform.parent.position.x, transform.position.y);

        //Rigibody component
        rb = GetComponent<Rigidbody2D>();

        //Initial velocity
        rb.velocity = new Vector2(30, 5);
    }

    // Update is called once per frame
    void Update()
    {
        //Change scale by Y position to do some Gravity effect
        transform.localScale = new Vector2(transform.position.y / 2, transform.position.y / 2);

        //Rebond
        if (transform.position.y <= 1.09f && rb.velocity.y < -1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y * 0.5f));
            transform.position = new Vector2(transform.position.x, 1.1f);
        }
        //Stop rebond
        else if (transform.position.y <= 1.09f)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            transform.position = new Vector2(transform.position.x, 1.1f);
        }
    }
}