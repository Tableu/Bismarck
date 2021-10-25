using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Attacks/SpawnFighters", order = 0)]
public class SpawnFighters : AttackScriptableObject
{
    public GameObject fighterPrefab;
    public float fireDelay;
    public float aggroRange;
    public override AttackCommand MakeAttack()
    {
        return new Attack(fighterPrefab, fireDelay, aggroRange);
    }

    private class Attack : AttackCommand
    {
        private GameObject _fighterPrefab;
        private float _fireDelay;
        private float _aggroRange;
        private bool Stop { get; set; }

        public Attack(GameObject fighterPrefab, float fireDelay, float aggroRange)
        {
            _fighterPrefab = fighterPrefab;
            _fireDelay = fireDelay;
            _aggroRange = aggroRange;
        }

        public void StopAttack()
        {
            Stop = true;
        }
        public IEnumerator DoAttack(GameObject attacker)
        {
            Stop = false;
            while (!Stop)
            {
                List<GameObject> ships = ShipManager.Instance.EnemyShips(attacker);
                if (ships == null || ships.Count == 0)
                {
                    yield return null;
                    continue;
                }
                
                GameObject target = DetectionController.DetectShip(_aggroRange, attacker);
                if (target != null)
                {
                    SpawnFighter(attacker, target);
                    yield return new WaitForSeconds(_fireDelay);
                }

                yield return null;
            }
        }
        public void SpawnFighter(GameObject mothership, GameObject target)
        {
            GameObject fighter = Instantiate(_fighterPrefab, mothership.transform.position, Quaternion.identity, mothership.transform.parent);
            FighterShipController controller = fighter.GetComponent<FighterShipController>();
            controller.target = target;
            controller.mothership = mothership;
            fighter.layer = mothership.layer;
        }
    }
}

