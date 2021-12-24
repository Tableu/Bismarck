using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnUnit", menuName = "SpawnUnit/SpawnFighters", order = 0)]
public class SpawnFighters : AttackScriptableObject
{
    public ShipData shipData;
    public float fireDelay;
    public override AttackCommand MakeAttack()
    {
        return new SpawnUnit(fireDelay, shipData);
    }

    private class SpawnUnit : AttackCommand
    {
        private ShipData _shipData;
        private GameObject _target;
        private float _fireDelay;
        private int _coroutineCount = 0;
        private bool Stop { get; set; }

        public SpawnUnit(float fireDelay, ShipData shipData)
        {
            _fireDelay = fireDelay;
            _shipData = shipData;
        }

        public void StopAttack()
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
        
        public IEnumerator DoAttack(GameObject attacker)
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
            ShipSpawner shipSpawner = mothership.GetComponent<ShipLogic>().ShipSpawner;
            shipSpawner.SpawnShip(_shipData.Copy(), mothership.transform.parent);
        }
    }
}

