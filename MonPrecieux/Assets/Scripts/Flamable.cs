using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamable: MonoBehaviour
{

    public bool IsOnFire = false;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame  
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Flamable obj = collision.GetComponent<Flamable>();
        if (obj!= null)
        {
            Debug.Log("C'est bon");
            if (obj.IsOnFire == true)
            {
                IsOnFire = true;
                Instantiate(prefab,
                    new Vector3(transform.position.x,
                                  transform.position.y,
                                  transform.position.z),
                                   Quaternion.identity);
                gameObject.AddComponent<ParticleSystem>();
                
            }
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Flamable obj = collision.gameObject.GetComponent<Flamable>();
        if (obj != null)
        {
            Debug.Log("C'est bon");
            if (obj.IsOnFire == true)
            {
                IsOnFire = true;
                Instantiate(prefab,
                    new Vector3(transform.position.x,
                                  transform.position.y,
                                  transform.position.z),
                                   Quaternion.identity);

            }
        }
    }
}
