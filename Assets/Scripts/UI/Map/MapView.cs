using System.Collections.Generic;
using System.Linq;
using Map;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SerializeField] private GameObject _starSystemPrefab;
    [SerializeField] private Texture2D _texture2D;

    private readonly List<GameObject> _systems = new List<GameObject>();

    private void Awake()
    {
        var map = new Starmap(_texture2D);

        var id = 0;
        foreach (var systemView in map.StarSystems.Select(system =>
                     Instantiate(_starSystemPrefab, system.Coordinates, Quaternion.identity, transform)))
        {
            systemView.GetComponent<StarSystemView>().SystemID = id;
            _systems.Add(systemView);
            ++id;
        }
    }
}