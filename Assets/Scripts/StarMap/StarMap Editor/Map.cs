using System.Collections.Generic;
using StarMap;
using UnityEditor;
using UnityEngine;

namespace StarMapEditor
{
    //Script for Unity Editor to save systempairs and starsystem information to MapData
    //Star system gameObject names should have no spaces
    public class Map : MonoBehaviour
    {
        public GameObject StarSystems;
        public StarSystemPairs StarSystemPairs;
        public string MapName;
        public string MapDataPath;

        public void Save()
        {
            StarSystem[] editorSystems = StarSystems.GetComponentsInChildren<StarSystem>();
            List<StarMap.StarSystem> assetSystems = new List<StarMap.StarSystem>(); 
            
            foreach (StarSystem editorSystem in editorSystems)
            {
                assetSystems.Add(editorSystem.Save(MapDataPath));
            }

            string[] maps = AssetDatabase.FindAssets(MapName,new []{MapDataPath});
            MapData mapData;
            if (maps.Length > 0)
            {
                string mapPath = AssetDatabase.GUIDToAssetPath(maps[0]);
                mapData = AssetDatabase.LoadAssetAtPath<MapData>(mapPath);
                mapData.StarSystems = assetSystems;
            }
            else
            {
                mapData = ScriptableObject.CreateInstance<MapData>();
                mapData.StarSystems = assetSystems;
                string path = MapDataPath + MapName + ".asset";
                AssetDatabase.CreateAsset(mapData, path);
            }
            
            List<SystemPair> systemPairs = StarSystemPairs.SystemPairs;
            mapData.SystemPairs = new List<MapData.SystemPair>();
            foreach (SystemPair systemPair in systemPairs)
            {
                if (systemPair.System1 != null && systemPair.System2 != null)
                {
                    string[] results1 = AssetDatabase.FindAssets(systemPair.System1.SystemName, new []{MapDataPath});
                    string systemPath1 = AssetDatabase.GUIDToAssetPath(results1[0]);
                    StarMap.StarSystem system1 = AssetDatabase.LoadAssetAtPath<StarMap.StarSystem>(systemPath1);
                    
                    string[] results2 = AssetDatabase.FindAssets(systemPair.System2.SystemName, new []{MapDataPath});
                    string systemPath2 = AssetDatabase.GUIDToAssetPath(results2[0]);
                    StarMap.StarSystem system2 = AssetDatabase.LoadAssetAtPath<StarMap.StarSystem>(systemPath2);

                    mapData.SystemPairs.Add(new MapData.SystemPair
                    {
                        System1 = system1,
                        System2 = system2
                    });
                }
            }
            
            AssetDatabase.SaveAssets();
        }
    }

    [CustomEditor(typeof(Map))]
    public class MapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            Map map = (Map) target;
            if (GUILayout.Button("Save Map"))
            {
                map.Save();
            }
        }
    }
}