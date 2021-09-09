using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamable: MonoBehaviour
{

    public bool IsOnFire = false;
    public GameObject prefab;
    public bool hasburn = false;
    public float timer =0;
    public float Timetoburn = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame  
    void Update()
    {
        if (timer > 0)
        {
           
            timer -= Time.deltaTime;
        }
        else
        {
            
            hasburn = true;
            IsOnFire = false;
            
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Flamable obj = collision.GetComponent<Flamable>();
        if (obj!= null && hasburn==false)
        {
            Debug.Log("C'est bon");
            if (obj.IsOnFire == true)
            {
                timer = Timetoburn;
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
    
   
}
