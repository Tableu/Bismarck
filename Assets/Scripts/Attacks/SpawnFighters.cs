using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Attacks/SpawnFighters", order = 0)]
public class SpawnFighters : AttackScriptableObject
{
    public GameObject fighterPrefab;
    public float fireDelay;
    public override AttackCommand MakeAttack()
    {
        return new Attack(fighterPrefab, fireDelay);
    }

    private class Attack : AttackCommand
    {
        private GameObject _fighterPrefab;
        private GameObject _target;
        private float _fireDelay;
        private int _coroutineCount = 0;
        private bool Stop { get; set; }

        public Attack(GameObject fighterPrefab, float fireDelay)
        {
            _fighterPrefab = fighterPrefab;
            _fireDelay = fireDelay;
        }

        public void StopAttack()
        {
            Stop = true;
        }

        public bool SetTarget(GameObject target)
        {
            if (_target == null)
            {
                _target = target;
                return true;
            }
            return false;
        }
        public IEnumerator DoAttack(GameObject attacker)
        {
            _coroutineCount++;
            int coroutineCount = _coroutineCount;
            Stop = false;
            while (!Stop && coroutineCount.Equals(_coroutineCount))
            {
                List<GameObject> ships = ShipManager.Instance.EnemyShips(attacker);
                if (_target == null || ships == null || ships.Count == 0)
                {
                    break;
                }
                
                SpawnFighter(attacker);
                yield return new WaitForSeconds(_fireDelay);
            }
        }
        private void SpawnFighter(GameObject mothership)
        {
            GameObject fighter = Instantiate(_fighterPrefab, mothership.transform.position, Quaternion.identity, mothership.transform.parent);
            FighterShipController controller = fighter.GetComponent<FighterShipController>();
            controller.target = _target;
            controller.mothership = mothership;
            fighter.layer = mothership.layer;
        }
    }
}

