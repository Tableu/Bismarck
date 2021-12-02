using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager _instance;
    [SerializeField] private GameObject mapPopup;
    [SerializeField] private GameObject battleScene;

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
    }

    public void OpenMap()
    {
        mapPopup.SetActive(true);
        battleScene.SetActive(false);
    }

    public void CloseMap()
    {
        mapPopup.SetActive(false);
        battleScene.SetActive(true);
    }
}
