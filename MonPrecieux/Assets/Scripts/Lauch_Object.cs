using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lauch_Object : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Mouse position
        Vector3 v3Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
        v3Pos -= transform.position;

        //Press T == Launch
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Object"), transform, true).GetComponent<Object_Controller>().Setup(v3Pos.normalized);
            
        }
    }
}