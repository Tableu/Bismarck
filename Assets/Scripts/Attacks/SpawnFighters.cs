using System.Collections;
using Ships.Components;
using Ships.DataManagment;
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
        private readonly float _fireDelay;
        private readonly ShipData _shipData;
        private int _coroutineCount;
        private GameObject _target;

        public SpawnUnit(float fireDelay, ShipData shipData)
        {
            _fireDelay = fireDelay;
            _shipData = shipData;
        }

        private bool Stop { get; set; }

        public void StopAttack()
        {
            Stop = true;
        }

        public void SetTarget(GameObject target)
        {
            if (target != null) _target = target;
        }

        public void SetParent(Transform parent)
        {
        }

        public IEnumerator DoAttack(GameObject attacker, Transform spawnPosition)
        {
            _coroutineCount++;
            var coroutineCount = _coroutineCount;
            Stop = false;
            while (!Stop && coroutineCount.Equals(_coroutineCount))
            {
                if (_target == null) break;

                SpawnFighter(attacker);
                yield return new WaitForSeconds(_fireDelay);
            }
        }

        private void SpawnFighter(GameObject mothership)
        {
            var spawner = mothership.GetComponent<ShipTags>().ShipSpawner;
            var ship = spawner.SpawnShip(_shipData, mothership.transform.parent, mothership.transform.position);
            if (ship != null)
            {
                var shipLogic = ship.GetComponent<FighterShipLogic>();
                if (shipLogic != null) shipLogic.mothership = mothership;
            }
        }
    }
}