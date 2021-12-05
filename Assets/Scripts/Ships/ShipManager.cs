using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipManager : MonoBehaviour
{
    private static ShipManager _instance;
    private List<GameObject> _playerShips;
    private List<GameObject> _enemyShips;

    public GameObject ShipParent
    {
        get;
        private set;
    }
    public static ShipManager Instance
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
        _playerShips = new List<GameObject>();
        _enemyShips = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ShipParent = GameObject.FindWithTag("Ships");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddShip(GameObject ship)
    {
        if (ship.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            _playerShips.Add(ship);
        }else if (ship.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            _enemyShips.Add(ship);
        }
    }

    public void RemoveShip(GameObject ship)
    {
        if (ship.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            _playerShips.Remove(ship);
        }else if (ship.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            _enemyShips.Remove(ship);
        }

        if (_enemyShips.Count <= 0 && SceneManager.GetActiveScene().name == "BattleScene")
        {
            TransitionManager.Instance.GoBackToFleetScreen();
        }
    }
    public List<GameObject> Ships(GameObject ship)
    {
        if (ship.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            return _playerShips;
        }else if (ship.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            return _enemyShips;
        }
        return null;
    }
    public List<GameObject> EnemyShips(GameObject ship)
    {
        if (ship.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            return _enemyShips;
        }else if (ship.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            return _playerShips;
        }
        return null;
    }
}
