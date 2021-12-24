using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager _instance;
    [SerializeField] private ShipDictionary playerShipDict;
    [SerializeField] private ShipSpawner playerShipSpawner;
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
        DontDestroyOnLoad(ships);
        _instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
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

    public void LoadPlayerShips()
    {
        Dictionary<int, ShipData>.Enumerator enumerator = playerShipDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            playerShipSpawner.SpawnShip(enumerator.Current.Value, ships.transform);
            playerShipDict.RemoveShip(enumerator.Current.Key);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ships.SetActive(true);
    }
}
