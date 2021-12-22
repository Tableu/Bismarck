using System.Collections.Generic;
using StarMap;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SerializeField] private GameObject _starSystemPrefab;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private Map _map;

    private readonly List<GameObject> _systemViews = new List<GameObject>();

    private void Awake()
    {
        foreach (var system in _map.StarSystems)
        {
            var systemView = Instantiate(_starSystemPrefab, (Vector3) system.Coordinates + Vector3.back,
                Quaternion.identity, transform);
            systemView.GetComponent<StarSystemView>().SystemModel = system;
            _systemViews.Add(systemView);

            foreach (var starLane in system.StarLanes)
            {
                var destIdx = _map.StarSystems.IndexOf(starLane.Destination);
                var srcIdx = _map.StarSystems.IndexOf(system);
                if (srcIdx <= destIdx)
                {
                    var startPos = system.Coordinates;
                    var endPos = starLane.Destination.Coordinates;
                    var lane = Instantiate(_linePrefab, Vector3.down, Quaternion.identity, transform);
                    var lr = lane.GetComponent<LineRenderer>();
                    lr.SetPosition(0, startPos);
                    lr.SetPosition(1, endPos);
                }
            }
            
        }
    }
}