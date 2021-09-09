using UnityEngine;
class Flamable : MonoBehaviour
{
    public bool isBurning = false;
    public bool hasBurned = false;
    public float timer = 0;
    public float timeToBurn = 5;
    public GameObject prefab;
    public GameObject fire;
    public SpriteRenderer sr;
    public Color baseColor;
    public Sprite ashes;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            sr.color = new Color(baseColor.r * timer / timeToBurn, baseColor.g * timer / timeToBurn, baseColor.b * timer / timeToBurn);
        }
        else if (isBurning == true)
        {
            hasBurned = true;
            isBurning = false;
            sr.sprite = ashes;
            fire.GetComponent<ParticleSystem>().Stop();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (gameObject.transform.parent.CompareTag("Torch")) { Debug.Log(collision.name); }
        Flamable obj = collision.GetComponent<Flamable>();
        if (obj != null && hasBurned == false)
        {
            Debug.Log("C'est bon");
            if (obj.isBurning == true)
            {
                timer = timeToBurn;
                isBurning = true;
                fire = Instantiate(prefab,
                    new Vector3(transform.position.x,
                        transform.position.y,
                        transform.position.z),
                    Quaternion.identity,
                    gameObject.transform);
            }
        }
    }
}