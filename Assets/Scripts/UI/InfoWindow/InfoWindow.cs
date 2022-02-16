using Ships.Components;
using UI.InfoWindow;
using UnityEngine;

/// <summary>
/// Refreshes InfoWindow based on a ShipInfo instance. Enables/Disables the InfoWindow camera and canvas elements accordingly.
/// </summary>
public class InfoWindow : MonoBehaviour
{
    [SerializeField] private SubsystemButtonManager subsystemButtonManager;
    [SerializeField] private Camera camera;

    public void Refresh(ShipInfo shipInfo)
    {
        if (camera != null && shipInfo != null)
        {
            var shipPos = shipInfo.transform.position;
            camera.transform.position = new Vector3(shipPos.x, shipPos.y, camera.transform.position.z);
            MatchCamera(camera, shipInfo);

            subsystemButtonManager.Refresh(shipInfo);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void MatchCamera(Camera cam, ShipInfo shipInfo)
    {
        if (cam == null || shipInfo == null)
        {
            return;
        }

        var position = cam.ViewportToWorldPoint(Vector3.zero);
        var up = cam.ViewportToWorldPoint(Vector3.up) - position;
        var right = cam.ViewportToWorldPoint(Vector3.right) - position;

        var bounds = shipInfo.Data.Visuals.GetComponent<SpriteRenderer>().bounds;
        var matchSize = Mathf.Max(bounds.size.y, bounds.size.x * up.magnitude / right.magnitude);

        var multiplier = shipInfo.Data.CameraSizeMultiplier > 0 ? shipInfo.Data.CameraSizeMultiplier : 1f;
        cam.orthographicSize = matchSize * multiplier;
    }

#if UNITY_EDITOR
    [Header("Debug")] [SerializeField] private ShipInfo testInfo;

    [ContextMenu("Refresh")]
    private void DebugRefresh()
    {
        Refresh(testInfo);
    }
#endif
}