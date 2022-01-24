using StarMap;
using UnityEditor;
using UnityEngine;

namespace StarMapEditor
{
    //Unity Editor version of StarSystem. Meant to be used in a Unity Scene to create/update MapData
    //Star system gameObject names should have no spaces
    [ExecuteInEditMode]
    public class StarSystem : MonoBehaviour
    {
        public string SystemName;
        public StarType MainStar;
        public float StarSize;
        public FleetDBScriptableObject RandomFleetDB;

        //Looks for an asset matching the system name, updating it if found, otherwise creates a new asset.
        public StarMap.StarSystem Save(string MapDataPath)
        {
            string[] results = AssetDatabase.FindAssets(gameObject.name, new []{MapDataPath});
            StarMap.StarSystem starSystem;
            if (results.Length > 0)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(results[0]);
                starSystem = AssetDatabase.LoadAssetAtPath<StarMap.StarSystem>(assetPath);
                starSystem.SystemName = SystemName;
                starSystem.MainStar = MainStar;
                starSystem.StarSize = StarSize;
                starSystem.RandomFleetDB = RandomFleetDB;
                starSystem.Coordinates = transform.position;
            }
            else
            {
                starSystem = ScriptableObject.CreateInstance<StarMap.StarSystem>();
                starSystem.SystemName = SystemName;
                starSystem.MainStar = MainStar;
                starSystem.StarSize = StarSize;
                starSystem.RandomFleetDB = RandomFleetDB;
                starSystem.Coordinates = transform.position;
                string path = MapDataPath + gameObject.name + ".asset";
                AssetDatabase.CreateAsset(starSystem,path);
            }

            return starSystem;
        }
    }
}