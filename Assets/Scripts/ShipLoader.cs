using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipLoader : MonoBehaviour
{
    public ShipSpawner ShipSpawner;
    public ShipDictionary ShipDictionary;
    public ShipDBScriptableObject ShipDB;
    public AttackDBScriptableObject AttackDB;

    public void Start()
    {
        SceneManager.sceneLoaded += LoadShips;
        SceneManager.sceneUnloaded += SaveShips;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveShips(Scene scene)
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
        ShipDictionary.ClearDict();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Saved Ships");
    }

    public void LoadShips(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (File.Exists(Application.persistentDataPath + "/shipsave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/shipsave.save", FileMode.Open);
            List<ShipSaveData> saveDatas = (List<ShipSaveData>)bf.Deserialize(file);
            List<ShipData> shipDatas = new List<ShipData>();
            foreach (ShipSaveData saveData in saveDatas)
            {
                ShipData shipData = saveData.Convert(AttackDB, ShipDB);
                if (shipData != null)
                {
                    shipDatas.Add(shipData);
                }
            }
            file.Close();
            ShipSpawner.SpawnFleet(shipDatas, transform);
            gameObject.SetActive(true);
            Debug.Log("Loaded Ships");
        }
        else
        {
            Debug.Log("No Save File");
        }
    }
}
