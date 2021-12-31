using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Attacks/Projectile", order = 0)]
public class ProjectileAttack : AttackScriptableObject
{
    public GameObject projectilePrefab;
    public float fireDelay;
    public override AttackCommand MakeAttack()
    {
        return new Attack(projectilePrefab, fireDelay);
    }

    private class Attack : AttackCommand
    {
        private GameObject _projectilePrefab;
        private GameObject _target;
        private GameObject _turret;
        private float _fireDelay;
        private Vector2 _direction;
        private bool _useTarget = true;
        private int _coroutineCount = 0;
        private bool Stop { get; set; }

        public Attack(GameObject projectilePrefab, float fireDelay)
        {
            _projectilePrefab = projectilePrefab;
            _fireDelay = fireDelay;
            _useTarget = false;
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
                _useTarget = true;
            }
        }

        public IEnumerator DoAttack(GameObject attacker, GameObject turret)
        {
            _coroutineCount++;
            if (turret != null)
            {
                _turret = turret;
            }

            int coroutineCount = _coroutineCount;
            Stop = false;
            yield return new WaitForSeconds(_fireDelay / 2);
            while (!Stop && coroutineCount.Equals(_coroutineCount))
            {
                SpawnProjectile(attacker);
                yield return new WaitForSeconds(_fireDelay);
            }
        }

        private void SpawnProjectile(GameObject attacker)
        {
            GameObject projectile = Instantiate(_projectilePrefab, _turret.transform.position,
                _projectilePrefab.transform.rotation);
            Projectile controller = projectile.GetComponent<Projectile>();
            if (controller != null)
            {
                float rotation = 0;
                float direction = attacker.transform.localScale.x;
                if (!_useTarget)
                {
                    _direction = new Vector2(direction, 0);
                }
                else if (_target != null)
                {
                    Vector2 diff = _target.transform.position - _turret.transform.position;
                    _direction = diff.normalized;
                }
                else
                {
                    Stop = true;
                    return;
                }

                rotation = Vector2.SignedAngle(Vector2.right, _direction);
                controller.Init(_direction, -direction * rotation, attacker.layer);
            }
        }
    }
}
