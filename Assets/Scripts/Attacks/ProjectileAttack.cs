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
        private float _fireDelay;
        private bool Stop { get; set; }

        public Attack(GameObject projectilePrefab, float fireDelay)
        {
            _projectilePrefab = projectilePrefab;
            _fireDelay = fireDelay;
        }

        public void StopAttack()
        {
            Stop = true;
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
            controller.direction = new Vector2(attacker.transform.localScale.x,0);
            var rotation = attacker.transform.rotation.eulerAngles;
            projectile.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z+projectile.transform.rotation.eulerAngles.z);
            projectile.layer = attacker.layer;
            controller.enemyLayer = controller.enemyLayer & ~LayerMask.GetMask(LayerMask.LayerToName(attacker.layer));
        }
    }
}
