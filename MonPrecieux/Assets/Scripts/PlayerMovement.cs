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
        if (!UI.instance.isPaused) orientation();
    }
    private void FixedUpdate()
    {
        playerbody.velocity = new Vector2(h, v) * speed;
    }
    void orientation()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );
        // transform.up = direction;
        transform.localScale = Input.mousePosition.x < Screen.width / 2 ? new Vector3(-1, 1, 1) : Vector3.one;
    }
}