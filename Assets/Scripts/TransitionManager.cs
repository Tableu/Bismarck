using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager _instance;
    [SerializeField] private GameObject mapPopup;
    [SerializeField] private GameObject ships;
    private GameObject _fleetScreen;

    public static TransitionManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(mapPopup);
        DontDestroyOnLoad(ships);
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name == "StoreScene")
            {
                InputManager.Instance.EnableStoreInput();
                mapPopup.SetActive(false);
                ships.SetActive(true);
                _fleetScreen = GameObject.FindWithTag("FleetScreen");
                SetShipFleetScreenPositions();
            }else if (scene.name == "BattleScene")
            {
                InputManager.Instance.EnableCombatInput();
                mapPopup.SetActive(false);
                ships.SetActive(true);
                _fleetScreen = null;
                SaveShipFleetScreenPositions();
            }
        };
    }

    public void OpenMap()
    {
        mapPopup.SetActive(true);
        ships.SetActive(false);
        if(_fleetScreen != null)
            _fleetScreen.SetActive(false);
    }

    public void CloseMap()
    {
        mapPopup.SetActive(false);
        ships.SetActive(true);
        if(_fleetScreen != null)
            _fleetScreen.SetActive(true);
    }

    public void GoBackToFleetScreen()
    {
        SceneManager.LoadScene("Scenes/StoreScene");
    }
    public void SaveShipFleetScreenPositions()
    {
        GameObject shipParent = GameObject.FindWithTag("Ships");
        ShipController[] shipControllers = shipParent.GetComponentsInChildren<ShipController>();
        foreach (ShipController controller in shipControllers)
        {
            controller.SaveFleetScreenPosition();
            controller.enabled = true;
        }
    }
    public void SetShipFleetScreenPositions()
    {
        GameObject shipParent = GameObject.FindWithTag("Ships");
        ShipController[] shipControllers = shipParent.GetComponentsInChildren<ShipController>();
        foreach (ShipController controller in shipControllers)
        {
            controller.SetFleetScreenPosition();
            controller.enabled = false;
        }
    }
}
