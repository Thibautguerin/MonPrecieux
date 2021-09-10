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
    [Range(0.0f, 1f)]
    public float size;
    [HideInInspector]
    public bool canPickUp;
    
    private Rigidbody2D rb;
    private CircleCollider2D coll2d;
    private bool useGravity;

    private void Awake()
    {
        //Rigibody component
        rb = GetComponent<Rigidbody2D>();

        //Collider Component
        coll2d = GetComponent<CircleCollider2D>();

        //Initialize Variable
        canPickUp = false;
        useGravity = true;

        //Ignore collision with player
        //Physics2D.IgnoreLayerCollision(6, 7);
    }

    public void Setup(Vector3 vector3, Vector2 vector2)
    {
        //Initial velocity
        rb.velocity = new Vector3(vector3.x * distance, vector3.y * distance);

        //Get position of player
        transform.position = new Vector3(vector2.x, vector2.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Change scale by Y position to do some Gravity effect
        transform.localScale = new Vector3(Mathf.Abs(transform.position.z) * size, Mathf.Abs(transform.position.z) * size, 1);
        coll2d.radius = 0.9f / Mathf.Abs(transform.position.z * size);
        
        //Rebond
        if (transform.position.z >= -1.09f && Mathf.Abs(height) > 0.05f)
        {
            height = Mathf.Abs(height * rebond);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1.09f);
        }
        //Stop rebond
        else if (transform.position.z >= -1.09f)
        {
            //Physics2D.IgnoreLayerCollision(6, 7, false);
            canPickUp = true;
            useGravity = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, -1.08f);
        }
    }

    private void FixedUpdate()
    {
        if (useGravity)
        {
            height -= 0.0981f * gravityScale;
            transform.Translate(Vector3.back * height, Space.Self);
        }
    }

}