using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lauch_Object : MonoBehaviour
{
    private bool getTorch = true;

    // Update is called once per frame
    void Update()
    {
        //Mouse position
        Vector3 v3Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
        v3Pos -= transform.position;

        //Player position
        Vector2 v2Pos = new Vector2(transform.position.x + 1.5f * transform.localScale.x, transform.position.y);

        //Press Left Click == Launch
        if (Input.GetMouseButtonDown(0) && getTorch)
        {
            getTorch = false;
            PlayerMovement.Instance.spriteRenderer.sprite = PlayerMovement.Instance.lookBottomRight;
            PlayerMovement.Instance.torchLight.SetActive(false);
            Instantiate(Resources.Load<GameObject>("Prefabs/Object"), transform.parent, true).GetComponent<Object_Controller>().Setup(v3Pos.normalized, v2Pos);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Parent == Object
        if (collision.transform.parent.CompareTag("Torch"))
        {
            //If Object stop bouncing, canPickUp == true
            if (collision.gameObject.GetComponentInParent<Object_Controller>().canPickUp)
            {
                getTorch = true;
                Destroy(collision.transform.parent.gameObject);
                PlayerMovement.Instance.spriteRenderer.sprite = PlayerMovement.Instance.lookBottomRightTorch;
                PlayerMovement.Instance.torchLight.SetActive(true);
            }
        }
    }
}