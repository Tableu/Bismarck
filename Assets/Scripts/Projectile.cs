using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;

    public int damage;
    public LayerMask enemyLayer;
    public int speed;
    public Vector2 direction;
    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        transform.Translate(direction*speed*Time.deltaTime);
    }
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            var enemy = other.gameObject.GetComponent<IDamageable>();
            var dmg = new Damage
            {
                RawDamage = damage,
                Source = transform.position
            };
            if (enemy != null)
            {
                //pSoundManager.PlaySound(pSoundManager.Sound.eHit);
                enemy.TakeDamage(dmg);
            }
        }
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
