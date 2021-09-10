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
        else
        {
            isBurning = true;
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
        if (collision is CircleCollider2D && collision.CompareTag("Torch") && CompareTag("Carpet"))
        {
            Debug.Log(collision.gameObject.name);
            Flamable obj = collision.gameObject.GetComponent<Flamable>();
            if (obj && !isBurning && obj.isBurning)
            {
                timer = timeToBurn;
                isBurning = true;
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
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision is CircleCollider2D && !collision.transform.CompareTag("Torch"))
        {
            Flamable obj = collision.gameObject.GetComponent<Flamable>();
            /*Debug.Log(obj);
            Debug.Log(isBurning);
            Debug.Log(obj.isBurning);*/
            if (obj && !isBurning && obj.isBurning && obj.timer >= 1 && obj.timer <= 2)
            {
                //Debug.Log("HDUZHUD");
                timer = timeToBurn;
                isBurning = true;
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
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.GetType());
        if (collision.collider is BoxCollider2D || (collision.collider is CircleCollider2D && collision.transform.CompareTag("Torch")))
        {
            Flamable obj = collision.gameObject.GetComponent<Flamable>();
            if (obj && !isBurning && obj.isBurning)
            {
                timer = timeToBurn;
                isBurning = true;
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
            }
        }
    }
}