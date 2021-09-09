using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Controller : MonoBehaviour
{
    public float distance;
    public float height;
    public float gravityScale;
    [Range(0.0f, 1f)]
    public float rebond;
    public bool canPickUp;
    
    private Rigidbody rb;
    private bool useGravity;

    private void Awake()
    {
        //Rigibody component
        rb = GetComponent<Rigidbody>();

        //Initialize Variable
        canPickUp = false;
        useGravity = true;
    }

    public void Setup(Vector3 vector3, Vector2 vector2)
    {
        //Initial velocity
        rb.velocity = new Vector3(vector3.x * distance, vector3.y * distance, -height);

        //Get position of player
        transform.position = new Vector3(vector2.x, vector2.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Change scale by Y position to do some Gravity effect
        transform.localScale = new Vector2(Mathf.Abs(transform.position.z) / 2, Mathf.Abs(transform.position.z) / 2);

        //Rebond
        if (transform.position.z >= -1.09f && rb.velocity.z > 1f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -rb.velocity.z * rebond);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1.1f);
        }
        //Stop rebond
        else if (transform.position.z >= -1.09f)
        {
            canPickUp = true;
            useGravity = false;
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, transform.position.y, -1.08f);
        }
    }

    private void FixedUpdate()
    {
        if (useGravity)
        {
            //Custom Gravity controller
            Vector3 gravity = -9.81f * gravityScale * Vector3.back;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}