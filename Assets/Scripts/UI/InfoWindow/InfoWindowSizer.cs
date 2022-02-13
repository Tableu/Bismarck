using Ships.Components;
using UnityEngine;

public class InfoWindowSizer : MonoBehaviour
{
    private ShipInfo _shipInfo;

    private void Start()
    {
        _shipInfo = GetComponentInParent<ShipInfo>();
        MatchCamera();
    }

    private void MatchCamera()
    {
        var cam = GetComponent<Camera>();
        if (cam == null || _shipInfo == null) return;

        var position = cam.ViewportToWorldPoint(Vector3.zero);
        var up = cam.ViewportToWorldPoint(Vector3.up) - position;
        var right = cam.ViewportToWorldPoint(Vector3.right) - position;

        var bounds = _shipInfo.Data.Visuals.GetComponent<SpriteRenderer>().bounds;
        var matchSize = Mathf.Max(bounds.size.y, bounds.size.x * up.magnitude / right.magnitude);

        var multiplier = _shipInfo.Data.CameraSizeMultiplier > 0 ? _shipInfo.Data.CameraSizeMultiplier : 1f;
        cam.orthographicSize = matchSize * multiplier;
    }

    private void OnValidate()
    {
        MatchCamera();
    }
}