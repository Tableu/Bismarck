using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    private static ShipManager _instance;
    private List<GameObject> _playerShips;
    
    public static ShipManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        _instance = this;
        _playerShips = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddShip(GameObject ship)
    {
        _playerShips.Add(ship);
    }

    public void RemoveShip(GameObject ship)
    {
        _playerShips.Remove(ship);
        Debug.Log("removed ship");
    }
    public List<GameObject> PlayerShips
    {
        get => _playerShips;
    }
}
