using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _instance;
    public static EnemySpawner Instance
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnShips()
    {
        
    }
}