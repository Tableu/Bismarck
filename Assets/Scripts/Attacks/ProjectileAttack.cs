using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Attacks/Projectile", order = 0)]
public class ProjectileAttack : AttackScriptableObject
{
    public GameObject projectilePrefab;
    public float fireDelay;
    public GameObject target;
    public override AttackCommand MakeAttack()
    {
        return new Attack(projectilePrefab, fireDelay, target);
    }

    private class Attack : AttackCommand
    {
        private GameObject _projectilePrefab;
        private GameObject _target;
        private float _fireDelay;
        private Vector2 _direction;
        private bool _useTarget = true;
        private bool Stop { get; set; }

        public Attack(GameObject projectilePrefab, float fireDelay, GameObject target)
        {
            _projectilePrefab = projectilePrefab;
            _fireDelay = fireDelay;
            _target = target;
            if (target == null)
            {
                _useTarget = false;
            }
        }

        public void StopAttack()
        {
            Stop = true;
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
            _useTarget = true;
        }
        public IEnumerator DoAttack(GameObject attacker)
        {
            Stop = false;
            yield return new WaitForSeconds(_fireDelay);
            while (!Stop)
            {
                SpawnProjectile(attacker);
                yield return new WaitForSeconds(_fireDelay);
            }
        }
        public void SpawnProjectile(GameObject attacker)
        {
            GameObject projectile = Instantiate(_projectilePrefab, attacker.transform.localPosition, _projectilePrefab.transform.rotation);
            Projectile controller = projectile.GetComponent<Projectile>();
            if (controller != null)
            {
                float rotation = 0;
                if (!_useTarget)
                {
                    _direction = new Vector2(attacker.transform.localScale.x, 0);
                }else if (_target != null)
                {
                    Vector2 diff = _target.transform.position - attacker.transform.position;
                    _direction = diff.normalized;
                    rotation = Vector2.Angle(Vector2.right, _direction);
                }
                controller.Init(_direction, rotation, attacker.layer);
            }
        }
    }
}
