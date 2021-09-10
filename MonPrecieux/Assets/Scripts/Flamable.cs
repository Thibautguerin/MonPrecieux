using UnityEngine;
class Flamable : MonoBehaviour
{
    public float timeToBurn = 3;
    public bool itsTheTorch = false;
    public GameObject fireParticle;
    public GameObject ashesPrefab;
    public Sprite objectAshesSprite;
    public AudioClip fireSound;

    private AudioSource audioSource;
    private Color baseColor = new Color(1f, 1f, 1f);
    private float timer = 0;
    [HideInInspector]
    public bool isBurning = false;
    private GameObject fire;
    private Fire fireScript;
    private SpriteRenderer sr;


    void Start()
    {
        if (!itsTheTorch)
        {
            audioSource = GetComponent<AudioSource>();
            sr = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (!itsTheTorch)
        {
            if (isBurning && timer > 0)
            {
                timer -= Time.deltaTime;
                sr.color = new Color(baseColor.r * timer / timeToBurn, baseColor.g * timer / timeToBurn, baseColor.b * timer / timeToBurn);
            }
            else if (isBurning && timer <= 0)
            {
                fire.GetComponent<ParticleSystem>().Stop();
                if (fireScript)
                {
                    fireScript.TurnOffLight();
                }
                Destroy(fire, 5f);
                isBurning = false;

                GameObject ashes = Instantiate(ashesPrefab, transform.position, Quaternion.identity);
                SpriteRenderer ashesSpriteRenderer = ashes.GetComponent<SpriteRenderer>();

                if (ashesSpriteRenderer && objectAshesSprite)
                {
                    ashesSpriteRenderer.sprite = objectAshesSprite;
                }
                GameManager.instance.IncPerc();
                fire.transform.SetParent(GameManager.instance.transform);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        // if (gameObject.transform.parent.CompareTag("Torch")) { Debug.Log(collision.name); }
        /*Flamable obj = collision.GetComponent<Flamable>();
        if (obj != null && hasBurned == false)
        {
            Debug.Log("C'est bon Trigger");
            if (obj.isBurning == true)
            {
                if (timer == 0)
                {
                    timer = timeToBurn;
                }
                isBurning = true;
                fire = Instantiate(prefab,
                    new Vector3(transform.position.x,
                        transform.position.y,
                        transform.position.z),
                    Quaternion.identity,
                    gameObject.transform);
            }
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Flamable obj = collision.gameObject.GetComponent<Flamable>();
        if (obj != null && !isBurning)
        {
            if (obj.isBurning == true)
            {
                if (audioSource && fireSound)
                {
                    audioSource.PlayOneShot(fireSound);
                }
                fire = Instantiate(fireParticle, transform.position, Quaternion.identity, transform);
                fireScript = fire.GetComponent<Fire>();
                if (fireScript)
                {
                    fireScript.TurnOnLight();
                }
                timer = timeToBurn;
                isBurning = true;
            }
        }
    }
}