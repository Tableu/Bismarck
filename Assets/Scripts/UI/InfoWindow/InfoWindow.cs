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

            subsystemButtonManager.Refresh(shipInfo);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
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