using System;
using DefaultNamespace;
using DefaultNamespace.Systems;
using Ships.Components;
using Ships.DataManagment;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawners", menuName = "Spawners/ShipSpawner", order = 0)]
[Serializable]
public class ShipSpawner : ScriptableObject
{
    public ShipList FactionShipList;
    public LayerMask LayerMask;
    public string ShipLayer;
    public int StartDirection;
    [SerializeField] private TagReference projectileParentTag;

    public string ProjectileParentTag => projectileParentTag.Tag;

    public GameObject SpawnShip(ShipData data, Transform parent, Vector2 position)
    {
        var ship = Instantiate(data.prefab, position, Quaternion.identity, parent);
        ship.AddComponent<ShipStats>();
        var tags = ship.AddComponent<ShipTags>();
        ship.GetComponent<ShipLogic>().enabled =
            GameContext.Instance.CurrentState != GameContext.GameState.StoreMode;

        foreach (var initializableComponent in ship.GetComponents<IInitializableComponent>())
            initializableComponent.Initialize(data, this);


        var scale = ship.transform.localScale;
        ship.transform.localScale = new Vector3(scale.x * Math.Sign(StartDirection), scale.y, scale.z);
        ship.layer = LayerMask.NameToLayer(ShipLayer);
        return ship;
    }
}