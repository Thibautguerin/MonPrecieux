using UnityEngine;
class PlayerCamera : MonoBehaviour
{
    public Camera camera;
    public Transform target;
    [Header("Menu Movement")]
    [Range(0, 1)]
    public float amplitude;
    [Range(0, 1)]
    public float speed;
    Vector3 offset;
    void Start() { offset = transform.position; }
    void FixedUpdate()
    {
        if (!UI.instance.isMenued)
        {
            if (Input.GetKey(KeyCode.F)) transform.position = Vector3.Lerp(transform.position, target.position + offset + new Vector3(10, 0, offset.z), .05f);
            else transform.position = Vector3.Lerp(transform.position, target.position + offset, .16f);
        }
    }
}