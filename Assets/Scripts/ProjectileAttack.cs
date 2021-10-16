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
            while (!Stop)
            {
                SpawnProjectile(attacker);
                yield return new WaitForSeconds(_fireDelay);
            }
        }
        public void SpawnProjectile(GameObject attacker)
        {
            GameObject projectile = Instantiate(_projectilePrefab, attacker.transform.localPosition, _projectilePrefab.transform.rotation);
            projectile.GetComponent<Projectile>().direction = (int)attacker.transform.localScale.x;
            var rotation = attacker.transform.rotation.eulerAngles;
            projectile.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z+projectile.transform.rotation.eulerAngles.z);
        }
    }
}
