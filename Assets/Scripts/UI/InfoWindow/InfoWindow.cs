using Ships.Components;
using UI.InfoWindow;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Refreshes InfoWindow based on a ShipInfo instance. Enables/Disables the InfoWindow camera and canvas elements accordingly.
/// </summary>
public class InfoWindow : MonoBehaviour
{
    public Camera Camera;
    public ShipInfo Player;
    [SerializeField] private SubsystemButtonManager subsystemButtonManager;
    [SerializeField] private ShipInfo shipInfo;
    [SerializeField] private Button closeButton;

    public void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseWindow);
        }

        if (shipInfo == null)
        {
            gameObject.SetActive(false);
        }

        subsystemButtonManager.Player = Player;
    }

    public void Start()
    {
        if (shipInfo != null)
        {
            Refresh();
        }
    }
    
    public void Refresh(ShipInfo newTarget = null)
    {
        if (newTarget != null)
        {
            shipInfo = newTarget;
        }

        if (Camera != null && shipInfo != null)
        {
            var shipPos = shipInfo.Visuals.transform.position;
            Camera.transform.position = new Vector3(shipPos.x, shipPos.y, Camera.transform.position.z);

            subsystemButtonManager.Player = Player;
            subsystemButtonManager.Refresh(shipInfo);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void CloseWindow()
    {
        Destroy(Camera.gameObject);
        Destroy(gameObject);
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