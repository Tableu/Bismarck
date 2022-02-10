using Ships.Components;
using UnityEngine;

public class InfoWindowCamera : MonoBehaviour
{
    public float Height;
    public float Width;

    private void Start()
    {
        MatchCamera();
    }

    private void Update()
    {
    }

    private void MatchCamera()
    {
        var cam = GetComponent<Camera>();
        var shipInfo = GetComponentInParent<ShipInfo>();
        if (cam == null || shipInfo == null) return;

        var position = cam.ViewportToWorldPoint(Vector3.zero);
        var up = cam.ViewportToWorldPoint(Vector3.up) - position;
        var right = cam.ViewportToWorldPoint(Vector3.right) - position;

        var bounds = shipInfo.Data.Visuals.GetComponent<SpriteRenderer>().bounds;
        var matchSize = Mathf.Max(bounds.size.y, bounds.size.x * up.magnitude / right.magnitude);

        cam.orthographicSize = matchSize;
    }
}