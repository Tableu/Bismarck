using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnUnit", menuName = "SpawnUnit/SpawnFighters", order = 0)]
public class SpawnFighters : SpawnUnitScriptableObject
{
    public ShipData shipData;
    public float fireDelay;
    public override SpawnUnitCommand MakeSpawnUnit(ShipSpawner shipSpawner)
    {
        return new SpawnUnit(fireDelay, shipData, shipSpawner);
    }

    private class SpawnUnit : SpawnUnitCommand
    {
        private ShipSpawner _shipSpawner;
        private ShipData _shipData;
        private GameObject _target;
        private float _fireDelay;
        private int _coroutineCount = 0;
        private bool Stop { get; set; }

        public SpawnUnit(float fireDelay, ShipData shipData, ShipSpawner shipSpawner)
        {
            _fireDelay = fireDelay;
            _shipData = shipData;
            _shipSpawner = shipSpawner;
        }

        public void StopSpawnUnit()
        {
            Stop = true;
        }

        public void SetTarget(GameObject target)
        {
            if (target != null)
            {
                _target = target;
            }
        }
        
        public IEnumerator DoSpawnUnit(GameObject attacker)
        {
            _coroutineCount++;
            int coroutineCount = _coroutineCount;
            Stop = false;
            while (!Stop && coroutineCount.Equals(_coroutineCount))
            {
                if (_target == null)
                {
                    break;
                }
                
                SpawnFighter(attacker);
                yield return new WaitForSeconds(_fireDelay);
            }
        }
        private void SpawnFighter(GameObject mothership)
        {
            _shipSpawner.SpawnShip(_shipData.Copy(), mothership.transform.parent);
        }
    }
}

