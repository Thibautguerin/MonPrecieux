using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteOrientation
{
    SIDE,
    BOTTOM,
    TOP
}

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public float speed = 5f;

    [Header("Sprites")]
    public Sprite lookBottomRight;
    public Sprite lookBottomRightTorch;
    public Sprite lookBottom;
    public Sprite lookBottomTorch;
    public Sprite lookTop;
    public Sprite lookTopTorch;

    [Header("Torch")]
    public GameObject torchLightSide;
    public GameObject torchLightBottom;
    public GameObject torchLightTop;

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
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public SpriteOrientation spriteOrientation;

    private float currentAnimRotation;
    private bool toggleAnimation;
    private float currentAnimScale;

    private Vector3 scaleSave;

    [HideInInspector]
    public bool getTorch = true;

    void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
        #endregion
    }

    void Start()
    {
        playerbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        Vector2 newDirection = new Vector2(h, v);
        if (newDirection.magnitude > 1)
        {
            newDirection.Normalize();
        }
        playerbody.velocity = newDirection * speed;
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

        Vector2 playerDirection = mousePosition - transform.position;
        playerDirection.Normalize();

        Debug.Log(playerDirection);
        if (playerDirection.y > 0.9f)
        {
            if (spriteOrientation != SpriteOrientation.TOP)
            {
                spriteOrientation = SpriteOrientation.TOP;
                if (getTorch)
                {
                    spriteRenderer.sprite = lookTopTorch;
                    torchLightSide.SetActive(false);
                    torchLightTop.SetActive(true);
                    torchLightBottom.SetActive(false);
                }
                else
                {
                    spriteRenderer.sprite = lookTop;
                    torchLightSide.SetActive(false);
                    torchLightTop.SetActive(false);
                    torchLightBottom.SetActive(false);
                }
            }
        }
        else if (playerDirection.y < -0.9f)
        {
            if (spriteOrientation != SpriteOrientation.BOTTOM)
            {
                spriteOrientation = SpriteOrientation.BOTTOM;
                if (getTorch)
                {
                    spriteRenderer.sprite = lookBottomTorch;
                    torchLightSide.SetActive(false);
                    torchLightTop.SetActive(false);
                    torchLightBottom.SetActive(true);
                }
                else
                {
                    spriteRenderer.sprite = lookBottom;
                    torchLightSide.SetActive(false);
                    torchLightTop.SetActive(false);
                    torchLightBottom.SetActive(false);
                }
            }
        }
        else
        {
            if (spriteOrientation != SpriteOrientation.SIDE)
            {
                spriteOrientation = SpriteOrientation.SIDE;
                if (getTorch)
                {
                    spriteRenderer.sprite = lookBottomRightTorch;
                    torchLightSide.SetActive(true);
                    torchLightTop.SetActive(false);
                    torchLightBottom.SetActive(false);
                }
                else
                {
                    spriteRenderer.sprite = lookBottomRight;
                    torchLightSide.SetActive(false);
                    torchLightTop.SetActive(false);
                    torchLightBottom.SetActive(false);
                }
            }
            scaleSave = Input.mousePosition.x < Screen.width / 2 ? new Vector3(-1, 1, 1) : Vector3.one;
        }
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