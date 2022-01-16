using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ShipLoader : MonoBehaviour
{
    public ShipSpawner ShipSpawner;
    public ShipDBScriptableObject ShipDB;
    public AttackDBScriptableObject AttackDB;
    public Transform ProjectileParent;
    public bool isStore;

    public void Start()
    {
        LoadShips();
        if (isStore)
        {
            List<GameObject> ships = ShipSpawner.ShipList.ShipList;
            foreach (GameObject ship in ships)
            {
                ShipLogic shipLogic = ship.GetComponent<ShipLogic>();
                if (shipLogic != null)
                {
                    shipLogic.enabled = false;
                }
            }
        }
    }

    public void LoadShips()
    {
        ShipSpawner.ProjectileParent = ProjectileParent;
        
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
            Debug.Log("Loaded Ships");
        }
        else
        {
            Debug.Log("No Save File");
        }
    }
}
