using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lauch_Object : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Press T == Launch
        if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Object"), transform, true); 
        }
    }
}