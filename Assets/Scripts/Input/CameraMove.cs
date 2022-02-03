using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float boundary;
    [SerializeField] private Camera camera;
    [SerializeField] private Vector2 scale;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!camera.enabled)
        {
            return;
        }
        if (Mouse.current.position.ReadValue().x > Screen.width - boundary)
        {
            camera.transform.position += new Vector3(scale.x, 0, 0);
        }
        else if (Mouse.current.position.ReadValue().x < 0 + boundary)
        {
            camera.transform.position -= new Vector3(scale.x, 0, 0);
        }

        if (Mouse.current.position.ReadValue().y > Screen.height - boundary)
        {
            camera.transform.position += new Vector3(0, scale.y, 0);
        }
        else if (Mouse.current.position.ReadValue().y < 0 + boundary)
        {
            camera.transform.position -= new Vector3(0, scale.y, 0);
        }
    }
}
