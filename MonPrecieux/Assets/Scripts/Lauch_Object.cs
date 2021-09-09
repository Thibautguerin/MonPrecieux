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
            PlayerMovement.Instance.getTorch = false;

            switch (PlayerMovement.Instance.spriteOrientation)
            {
                case SpriteOrientation.SIDE:
                    PlayerMovement.Instance.spriteRenderer.sprite = PlayerMovement.Instance.lookBottomRight;
                    PlayerMovement.Instance.torchLightSide.SetActive(false);
                    break;
                case SpriteOrientation.BOTTOM:
                    PlayerMovement.Instance.spriteRenderer.sprite = PlayerMovement.Instance.lookBottom;
                    PlayerMovement.Instance.torchLightBottom.SetActive(false);
                    break;
                case SpriteOrientation.TOP:
                    PlayerMovement.Instance.spriteRenderer.sprite = PlayerMovement.Instance.lookTop;
                    PlayerMovement.Instance.torchLightTop.SetActive(false);
                    break;
                default:
                    break;
            }

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
                PlayerMovement.Instance.getTorch = true;
                Destroy(collision.transform.parent.gameObject);

                switch (PlayerMovement.Instance.spriteOrientation)
                {
                    case SpriteOrientation.SIDE:
                        PlayerMovement.Instance.spriteRenderer.sprite = PlayerMovement.Instance.lookBottomRightTorch;
                        PlayerMovement.Instance.torchLightSide.SetActive(true);
                        break;
                    case SpriteOrientation.BOTTOM:
                        PlayerMovement.Instance.spriteRenderer.sprite = PlayerMovement.Instance.lookBottomTorch;
                        PlayerMovement.Instance.torchLightBottom.SetActive(true);
                        break;
                    case SpriteOrientation.TOP:
                        PlayerMovement.Instance.spriteRenderer.sprite = PlayerMovement.Instance.lookTopTorch;
                        PlayerMovement.Instance.torchLightTop.SetActive(true);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}