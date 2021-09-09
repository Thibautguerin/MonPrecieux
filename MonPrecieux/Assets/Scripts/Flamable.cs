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
    public SpriteRenderer Sr;
    public Color BaseColor;
    public Sprite Ashe;
    public GameObject Fire;

    // Start is called before the first frame update
    void Start()
    {
        Sr = GetComponent<SpriteRenderer>();
        BaseColor = Sr.color;

        

    }

    // Update is called once per frame  
    void Update()
    {
        if (timer > 0)
        {
           
            timer -= Time.deltaTime;
            Sr.color = new Color(BaseColor.r*timer/Timetoburn, BaseColor.g * timer / Timetoburn, BaseColor.b * timer / Timetoburn);
        }
        else if (IsOnFire == true)
        {
            
            hasburn = true;
            IsOnFire = false;
            Sr.sprite = Ashe;
            Fire.GetComponent<ParticleSystem>().Stop();
            
            
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
                Fire = Instantiate(prefab,
                    new Vector3(transform.position.x,
                                  transform.position.y,
                                  transform.position.z),
                                   Quaternion.identity,
                                   gameObject.transform);
                
                
            }
        }
       
    }
    
   
}
