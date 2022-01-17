using System.Collections.Generic;
using UnityEngine;

public class ShipTurrets : MonoBehaviour
{
    private List<AttackScriptableObject> attackScriptableObjects;
    public List<Transform> turretPositions;
    public Transform turretParent;
    public ShipSpawner ShipSpawner;

    // Start is called before the first frame update
    void Start()
    {
        var shipData = ShipSpawner.ShipDictionary.GetShip(gameObject.GetInstanceID());
        attackScriptableObjects = shipData.Weapons;
        
        List<Transform>.Enumerator turretPos = turretPositions.GetEnumerator();
        foreach (AttackScriptableObject attackScriptableObject in attackScriptableObjects)
        {
            if (!turretPos.MoveNext())
            {
                break;
            }
            GameObject turret = Instantiate(attackScriptableObject.Turret, turretParent, false);
            turret.transform.localPosition = turretPos.Current.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        foreach (Transform child in turretParent)
        {
            Destroy(child.gameObject);
        }
        
        var shipData = ShipSpawner.ShipDictionary.GetShip(gameObject.GetInstanceID());
        attackScriptableObjects = shipData.Weapons;
        
        List<Transform>.Enumerator turretPos = turretPositions.GetEnumerator();
        foreach (AttackScriptableObject attackScriptableObject in attackScriptableObjects)
        {
            if (!turretPos.MoveNext())
            {
                break;
            }
            GameObject turret = Instantiate(attackScriptableObject.Turret, turretParent, false);
            turret.transform.localPosition = turretPos.Current.localPosition;
        }
    }
}
