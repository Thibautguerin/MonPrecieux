using UnityEngine;
class PlayerCamera : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    [Range(0, 1)]
    public float movSpeed;
    [Range(0, 1)]
    public float zoomSpeed;
    public float zoomSize;
    public float dezoomSize;
    Vector3 offset;
    void Start()
    {
        cam.orthographicSize = zoomSize;
        offset = transform.position;
    }
    void FixedUpdate()
    {
        if (!UI.instance.isMenued)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, movSpeed);
            if (Input.GetButton("Jump") || GameManager.instance.isDone) cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, dezoomSize, zoomSpeed);
            else cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, zoomSpeed);
        }
    }
}