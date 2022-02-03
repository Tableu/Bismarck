using UnityEngine;

public class PointLaser : MonoBehaviour
{
    [SerializeField] private int lifetime;
    private int _timer;
    void Start()
    {
        _timer = 0;
    }

    void FixedUpdate()
    {
        if (_timer > lifetime)
        {
            Destroy(gameObject);
        }
        _timer++;
    }
}
