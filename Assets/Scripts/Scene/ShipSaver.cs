using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ShipSaver : MonoBehaviour
{
    private static ShipSaver _instance;
    public ShipDictionary ShipDictionary;

    public static ShipSaver Instance
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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SaveShips()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/shipsave.save");
        List<ShipSaveData> saveDatas = new List<ShipSaveData>();
        foreach (ShipData shipData in ShipDictionary.ShipList())
        {
            ShipSaveData saveData = new ShipSaveData();
            saveData.Init(shipData);
            saveDatas.Add(saveData);
        }
        bf.Serialize(file, saveDatas);
        file.Close();
        Debug.Log("Saved Ships");
    }
}
