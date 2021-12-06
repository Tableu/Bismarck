using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;

    public int damage;
    public LayerMask enemyLayer;
    public int speed;
    public Vector2 direction;
    public CollisionType type;
    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        transform.Translate(new Vector2(direction.x, direction.y)*speed*Time.deltaTime);
    }

    public void Init(Vector2 direction, float zRotation, int projectileLayer)
    {
        this.direction = direction;
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x*Mathf.Sign(direction.x), scale.y, scale.z);
        Vector3 rotation = Quaternion.identity.eulerAngles;
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y,
            rotation.z + zRotation);
        gameObject.layer = projectileLayer;
        enemyLayer = enemyLayer & ~LayerMask.GetMask(LayerMask.LayerToName(projectileLayer));
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
                //pSoundManager.PlaySound(pSoundManager.Sound.eHit);
                enemy.TakeDamage(dmg);
                if(enemy.DestroyProjectile(CollisionType.energy))
                    Destroy(gameObject);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
