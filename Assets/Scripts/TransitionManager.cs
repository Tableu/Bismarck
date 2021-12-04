using System;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager _instance;
    [SerializeField] private GameObject mapPopup;
    [SerializeField] private GameObject fleetScreen;

    public static TransitionManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            _instance = null;
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(mapPopup);
        DontDestroyOnLoad(fleetScreen);
    }

    public void OpenMap()
    {
        mapPopup.SetActive(true);
        fleetScreen.SetActive(false);
    }

    public void CloseMap()
    {
        mapPopup.SetActive(false);
        fleetScreen.SetActive(true);
    }

    public void SaveShipFleetScreenPositions()
    {
        GameObject shipParent = GameObject.FindWithTag("Ships");
        ShipController[] shipControllers = shipParent.GetComponentsInChildren<ShipController>();
        foreach (ShipController controller in shipControllers)
        {
            controller.SaveFleetScreenPosition();
        }
    }
    public void SetShipFleetScreenPositions()
    {
        GameObject shipParent = GameObject.FindWithTag("Ships");
        ShipController[] shipControllers = shipParent.GetComponentsInChildren<ShipController>();
        foreach (ShipController controller in shipControllers)
        {
            controller.SetFleetScreenPosition();
        }
    }
}
