using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Controller : MonoBehaviour
{
    public float distance;
    public float height;
    [Range(0.0f, 1f)]
    public float rebond;
    private Rigidbody rb;

    private void Awake()
    {
        //Get position of player
        transform.position = new Vector2(transform.parent.position.x, transform.position.y);

        //Rigibody component
        rb = GetComponent<Rigidbody>();
    }

    public void Setup(Vector3 vector3)
    {
        //Initial velocity
        rb.velocity = new Vector3(vector3.x * distance, height, vector3.z * distance);
    }

    // Update is called once per frame
    void Update()
    {
        //Change scale by Y position to do some Gravity effect
        transform.localScale = new Vector2(transform.position.y / 2, transform.position.y / 2);

        //Rebond
        if (transform.position.y <= 1.09f && rb.velocity.y < -1f)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Abs(rb.velocity.y * rebond), rb.velocity.z);
            transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z);
        }
        //Stop rebond
        else if (transform.position.y <= 1.09f)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z);
        }
    }
}