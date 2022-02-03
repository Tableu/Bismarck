using UnityEngine;

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
    }

    public void OpenMap()
    {
        mapPopup.SetActive(true);
        ships.SetActive(false);
        if (_fleetScreen != null)
        {
            _fleetScreen.SetActive(false);
        }
    }

    public void CloseMap()
    {
        mapPopup.SetActive(false);
        ships.SetActive(true);
        if (_fleetScreen != null)
        {
            _fleetScreen.SetActive(true);
        }
    }
}
