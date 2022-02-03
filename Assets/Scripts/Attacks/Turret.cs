using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circleCollider2D;

    public int damage;
    public LayerMask enemyLayer;
    public CollisionType type;
    // Start is called before the first frame update
    protected void Start()
    {
        gameObject.layer = transform.parent.gameObject.layer;
        if (gameObject.layer == LayerMask.NameToLayer("EnemyShips"))
        {
            enemyLayer = enemyLayer & ~LayerMask.GetMask("EnemyShips", "EnemyProjectiles");
        }
        else if (gameObject.layer == LayerMask.NameToLayer("PlayerShips"))
        {
            enemyLayer = enemyLayer & ~LayerMask.GetMask("PlayerShips", "PlayerProjectiles");
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            var enemy = other.gameObject.GetComponent<IDamageable>();
            var dmg = new Damage
            {
                RawDamage = damage,
                Source = transform.position,
                Type = type
            };
            if (enemy != null)
            {
                enemy.TakeDamage(dmg);
                if (enemy.DestroyProjectile(CollisionType.energy))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
