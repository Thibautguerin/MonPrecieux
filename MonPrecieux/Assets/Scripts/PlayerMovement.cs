using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    [Header("Animations Speed")]
    public float animationSpeed = 3;
    public float animationToIdleSpeed = 3;
    public float idleAnimationSpeed = 0.2f;

    [Header("Animations Parameters")]
    public float maxRotation = 10;
    public float minIdleScale = 0.95f;
    public float maxIdleScale = 1.05f;

    [Space]
    public Transform rotator;

    private float h;
    private float v;
    private Rigidbody2D playerbody;

    private float currentAnimRotation;
    private bool toggleAnimation;
    private float currentAnimScale;

    private Vector3 scaleSave;

    void Start()
    {
        playerbody = GetComponent<Rigidbody2D>();
        scaleSave = transform.localScale;
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        if (!UI.instance.isPaused) orientation();

        Animation();
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
            mousePosition.x - rotator.position.x,
            mousePosition.y - rotator.position.y
        );
        rotator.up = direction;

        scaleSave = Input.mousePosition.x < Screen.width / 2 ? new Vector3(-1, 1, 1) : Vector3.one;
    }

    private void Animation()
    {
        if (playerbody.velocity.magnitude > 0)
        {
            if (toggleAnimation)
            {
                if (currentAnimRotation == 1)
                {
                    toggleAnimation = false;
                    currentAnimRotation = Mathf.Max(currentAnimRotation - Time.deltaTime * animationSpeed * playerbody.velocity.magnitude, -1);
                }
                else
                {
                    currentAnimRotation = Mathf.Min(currentAnimRotation + Time.deltaTime * animationSpeed * playerbody.velocity.magnitude, 1);
                }
            }
            else
            {
                if (currentAnimRotation == -1)
                {
                    toggleAnimation = true;
                    currentAnimRotation = Mathf.Min(currentAnimRotation + Time.deltaTime * animationSpeed * playerbody.velocity.magnitude, 1);
                }
                else
                {
                    currentAnimRotation = Mathf.Max(currentAnimRotation - Time.deltaTime * animationSpeed * playerbody.velocity.magnitude, -1);
                }
            }

            if (currentAnimScale > 1)
            {
                currentAnimScale = Mathf.Max(currentAnimScale - Time.deltaTime * idleAnimationSpeed, 1);
            }
            else if (currentAnimScale < 1)
            {
                currentAnimScale = Mathf.Min(currentAnimScale + Time.deltaTime * idleAnimationSpeed, 1);
            }
        }
        else
        {
            if (toggleAnimation)
            {
                if (currentAnimScale == maxIdleScale)
                {
                    toggleAnimation = false;
                    currentAnimScale = Mathf.Max(currentAnimScale - Time.deltaTime * idleAnimationSpeed, minIdleScale);
                }
                else
                {
                    currentAnimScale = Mathf.Min(currentAnimScale + Time.deltaTime * idleAnimationSpeed, maxIdleScale);
                }
            }
            else
            {
                if (currentAnimScale == minIdleScale)
                {
                    toggleAnimation = true;
                    currentAnimScale = Mathf.Min(currentAnimScale + Time.deltaTime * idleAnimationSpeed, maxIdleScale);
                }
                else
                {
                    currentAnimScale = Mathf.Max(currentAnimScale - Time.deltaTime * idleAnimationSpeed, minIdleScale);
                }
            }

            if (currentAnimRotation > 0)
            {
                currentAnimRotation = Mathf.Max(currentAnimRotation - Time.deltaTime * animationToIdleSpeed, 0);
            }
            else if (currentAnimRotation < 0)
            {
                currentAnimRotation = Mathf.Min(currentAnimRotation + Time.deltaTime * animationToIdleSpeed, 0);
            }
        }

        Quaternion newRotation = new Quaternion();
        newRotation.eulerAngles = new Vector3(transform.rotation.x, 0, currentAnimRotation * maxRotation);
        transform.rotation = newRotation;

        transform.localScale = scaleSave * currentAnimScale;
    }
}