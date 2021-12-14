using System.Collections.Generic;
using System.Linq;
using MapGeneration;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SerializeField] private GameObject _starSystemPrefab;
    [SerializeField] private Texture2D _texture2D;
    [SerializeField] private Starmap _map;

    private readonly List<GameObject> _systemViews = new List<GameObject>();

    private void Awake()
    {

        var id = 0;
        foreach (var systemView in _map.StarSystems.Select(system =>
                     Instantiate(_starSystemPrefab, system.Coordinates, Quaternion.identity, transform)))
        {
            systemView.GetComponent<StarSystemView>().SystemID = id;
            _systemViews.Add(systemView);
            ++id;
        }
    }
}