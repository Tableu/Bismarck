using System.Collections.Generic;
using MapGeneration;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SerializeField] private GameObject _starSystemPrefab;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private Texture2D _texture2D;
    [SerializeField] private Starmap _map;

    private readonly List<GameObject> _systemViews = new List<GameObject>();

    private void Awake()
    {
        var id = 0;
        var drawnLanes = new HashSet<(int, int)>();
        foreach (var system in _map.StarSystems)
        {
            var systemView = Instantiate(_starSystemPrefab, (Vector3) system.Coordinates + Vector3.back,
                Quaternion.identity, transform);
            foreach (var end in system.ConnectedSystems)
            {
                if (drawnLanes.Contains((end, id)) || end == id) continue;
                var lane = Instantiate(_linePrefab, Vector3.down, Quaternion.identity, transform);
                var lr = lane.GetComponent<LineRenderer>();
                lr.SetPosition(0, system.Coordinates);
                lr.SetPosition(1, _map.StarSystems[end].Coordinates);
                drawnLanes.Add((id, end));
                drawnLanes.Add((end, id));
            }

            systemView.GetComponent<StarSystemView>().SystemID = id;
            _systemViews.Add(systemView);
            ++id;
        }
    }
}