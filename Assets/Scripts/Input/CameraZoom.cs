using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private float scale;
    [SerializeField] private int maxSize;
    [SerializeField] private int minSize;
    
    void Update()
    {
        float size = camera.m_Lens.OrthographicSize;
        if (Mouse.current.scroll.ReadValue().y > 0 && size < maxSize)
        {
            size += scale;
        }else if (Mouse.current.scroll.ReadValue().y < 0 && size > minSize)
        {
            size -= scale;
        }

        camera.m_Lens.OrthographicSize = size;
    }
}
