using UnityEngine;
class PlayerCamera : MonoBehaviour
{
    public Camera camera;
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
        camera.orthographicSize = zoomSize;
        offset = transform.position;
    }
    void FixedUpdate()
    {
        if (!UI.instance.isMenued)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, movSpeed);
            camera.orthographicSize = Input.GetKey(KeyCode.F) ?
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, dezoomSize, zoomSpeed) :
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomSize, zoomSpeed);
        }
    }
}