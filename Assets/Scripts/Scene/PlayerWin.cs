using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerWin : MonoBehaviour
{
    public ShipListScriptableObject EnemyShipList;

    public UnityEvent PlayerWinEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (EnemyShipList.Count <= 0)
        {
            PlayerWinEvent.Invoke();
            Debug.Log("Player Win");
        }
    }

    public void GoToStore()
    {
        SceneManager.LoadScene("Scenes/StoreScene");
    }
}
