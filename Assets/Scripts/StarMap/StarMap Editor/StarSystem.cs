using StarMap;
using UnityEditor;
using UnityEngine;

namespace StarMapEditor
{
    [ExecuteInEditMode]
    public class StarSystem : MonoBehaviour
    {
        public string SystemName;
        public StarType MainStar;
        public float StarSize;
        public FleetDBScriptableObject RandomFleetDB;

        public StarMap.StarSystem Save(string MapDataPath)
        {
            string[] results = AssetDatabase.FindAssets(SystemName, new []{MapDataPath});
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
                string path = MapDataPath + starSystem.SystemName + ".asset";
                AssetDatabase.CreateAsset(starSystem,path);
            }

            return starSystem;
        }
    }
}