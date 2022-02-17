using System.Collections;
using Ships.Components;
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
        private Vector2 _direction;
        private float _fireDelay;
        private float _fireTime;
        private Transform _parent;
        private GameObject _projectilePrefab;
        private Transform _spawnPosition;
        private ShipInfo _target;

        public Attack(GameObject projectilePrefab, float fireDelay)
        {
            _projectilePrefab = projectilePrefab;
            _fireDelay = fireDelay;
            _fireTime = Time.deltaTime;
        }

        public void SetTarget(ShipInfo target)
        {
            if (target != null)
            {
                _target = target;
            }
        }

        public void SetParent(Transform parent)
        {
            _parent = parent;
        }

        public bool DoAttack(GameObject attacker, Transform spawnPosition)
        {
            if (spawnPosition != null)
            {
                _spawnPosition = spawnPosition;
            }

            if (Time.deltaTime - _fireTime > _fireDelay)
            {
                _fireTime = Time.deltaTime;
                SpawnProjectile(attacker);
                return true;    
            }

            return false;
        }

        private void SpawnProjectile(GameObject attacker)
        {
            GameObject projectile = Instantiate(_projectilePrefab, _spawnPosition.position,
                _projectilePrefab.transform.rotation);
            if (_parent != null)
            {
                projectile.transform.parent = _parent;
            }

            Projectile controller = projectile.GetComponent<Projectile>();
            if (controller != null)
            {
                float rotation = 0;
                if (_target != null)
                {
                    Vector2 diff = _target.transform.position - _spawnPosition.position;
                    _direction = diff.normalized;
                }

                rotation = Vector2.SignedAngle(Vector2.right, _direction);
                controller.Init(_direction, -Mathf.Sign(_direction.x) * rotation, attacker.layer);
            }
        }
    }
}
