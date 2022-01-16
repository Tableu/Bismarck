using System.Collections.Generic;
using UnityEngine;

namespace UI.Map
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private GameObject _starSystemPrefab;
        [SerializeField] private GameObject _linePrefab;
        [SerializeField] private StarMap.Map _map;

        private readonly List<GameObject> _systemViews = new List<GameObject>();

        private void Awake()
        {
            foreach (var system in _map.StarSystems)
            {
                var systemView = Instantiate(_starSystemPrefab, (Vector3) system.Coordinates + Vector3.back,
                    Quaternion.identity, transform);
                systemView.GetComponent<StarSystemView>().SystemModel = system;
                _systemViews.Add(systemView);
            }

            foreach (var systemPair in _map.SystemPairs)
            {
                var srcSystem = systemPair.System1;
                var dstSystem = systemPair.System2;

                var srcPos = srcSystem.Coordinates;
                var dstPos = dstSystem.Coordinates;
                var lane = Instantiate(_linePrefab, Vector3.down, Quaternion.identity, transform);
                var lr = lane.GetComponent<LineRenderer>();
                lr.SetPosition(0, srcPos);
                lr.SetPosition(1, dstPos);
            }
        }
    }
}