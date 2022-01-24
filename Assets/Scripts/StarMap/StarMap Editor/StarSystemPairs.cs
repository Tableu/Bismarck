using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace StarMapEditor
{
    [ExecuteInEditMode]
    public class StarSystemPairs : MonoBehaviour
    {
        public List<SystemPair> SystemPairs;
        public GameObject LinePrefab;
        private List<GameObject> _hyperlanes;

        // Start is called before the first frame update
        void Start()
        {
            SystemPairs = new List<SystemPair>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RefreshHyperlanes()
        {
            if (_hyperlanes != null)
            {
                foreach (GameObject lane in _hyperlanes)
                {
                    DestroyImmediate(lane);
                }
            }

            _hyperlanes = new List<GameObject>();
            if (SystemPairs != null)
            {
                foreach (SystemPair systemPair in SystemPairs.ToList())
                {
                    if (systemPair.System1 != null && systemPair.System2 != null)
                    {
                        var lane = Instantiate(LinePrefab, Vector3.down, Quaternion.identity, transform);
                        var lr = lane.GetComponent<LineRenderer>();
                        lr.SetPosition(0, systemPair.System1.transform.position);
                        lr.SetPosition(1, systemPair.System2.transform.position);
                        _hyperlanes.Add(lane);
                    }
                }
            }
        }
    }
    [Serializable]
    public struct SystemPair
    {
        public StarSystem System1;
        public StarSystem System2;
    }
    [CustomEditor(typeof(StarSystemPairs))]
    public class StarSystemPairsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            StarSystemPairs map = (StarSystemPairs) target;
            if (GUILayout.Button("Draw Hyperlanes"))
            {
                map.RefreshHyperlanes();
            }
        }
    }
}