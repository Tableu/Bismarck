using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StarMap;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipSaver", menuName = "Saving/Ship", order = 0)]
public class ShipSaver : ScriptableObject
{
    private static ShipSaver _instance;

    [SerializeField] private ShipDictionary _shipDictionary;

    [SerializeField] private MapContext _mapContext;

    private void OnEnable()
    {
        _mapContext.OnCurrentSystemChange += MapContextOnOnCurrentSystemChange;
    }

    private void OnDisable()
    {
        _mapContext.OnCurrentSystemChange -= MapContextOnOnCurrentSystemChange;
    }

    private void MapContextOnOnCurrentSystemChange(StarSystem arg1, StarSystem arg2)
    {
        SaveShips();
    }

    public void SaveShips()
    {
        var bf = new BinaryFormatter();
        var file = File.Create(Application.persistentDataPath + "/shipsave.save");
        var saveDatas = new List<ShipSaveData>();
        foreach (var shipData in _shipDictionary.ShipDataList())
        {
            var saveData = new ShipSaveData();
            saveData.Init(shipData);
            saveDatas.Add(saveData);
        }

        bf.Serialize(file, saveDatas);
        file.Close();
        Debug.Log("Saved Ships");
    }
}