using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCamera;
    [SerializeField] private float scale;
    [SerializeField] private int maxSize;
    [SerializeField] private int minSize;
    
    void Update()
    {
        float size = vCamera.m_Lens.OrthographicSize;
        if (Mouse.current.scroll.ReadValue().y > 0 && size < maxSize)
        {
            size += scale;
        }else if (Mouse.current.scroll.ReadValue().y < 0 && size > minSize)
        {
            size -= scale;
        }

        vCamera.m_Lens.OrthographicSize = size;
    }
}
