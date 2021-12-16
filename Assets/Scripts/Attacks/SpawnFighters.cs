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
            GameObject fighter = Instantiate(_fighterPrefab, mothership.transform.position, Quaternion.identity, mothership.transform.parent);
            FighterShipBattleController battleController = fighter.GetComponent<FighterShipBattleController>();
            battleController.target = _target;
            battleController.mothership = mothership;
            fighter.layer = mothership.layer;
        }
    }
}

