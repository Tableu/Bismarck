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

        public void SetTarget(GameObject target)
        {
            _target = target;
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
                
                if (_target != null)
                {
                    SpawnFighter(attacker);
                    yield return new WaitForSeconds(_fireDelay);
                }

                yield return null;
            }
        }
        public void SpawnFighter(GameObject mothership)
        {
            GameObject fighter = Instantiate(_fighterPrefab, mothership.transform.position, Quaternion.identity, mothership.transform.parent);
            FighterShipController controller = fighter.GetComponent<FighterShipController>();
            controller.target = _target;
            controller.mothership = mothership;
            fighter.layer = mothership.layer;
        }
    }
}

