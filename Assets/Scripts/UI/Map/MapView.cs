using Map;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SerializeField] private GameObject _starSystemPrefab;
    [SerializeField] private Texture2D _texture2D;

    private void Awake()
    {
        var map = new Starmap(100, _texture2D);

        foreach (var system in map.StarSystems)
            Instantiate(_starSystemPrefab, system.Coordinates, Quaternion.identity, transform);
    }
}