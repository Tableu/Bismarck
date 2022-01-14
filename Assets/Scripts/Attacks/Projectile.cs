using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeedSegment
{
    public float speed;
    public float duration;
}
public class Projectile : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private GameObject visuals;
    [SerializeField] private List<SpeedSegment> speedSegments;

    public int damage;
    public LayerMask enemyLayer;
    public float speed;
    public Vector2 _direction;
    public CollisionType type;
    private float startTime;
    private int _speedSegmentIndex;
    private int _speedSegmentMax;
    private SpeedSegment _currentSegment;
    private bool _stopTimer;
    // Start is called before the first frame update
    protected void Start()
    {
        if (speedSegments != null && speedSegments.Count != 0)
        {
            startTime = Time.fixedTime;
            _speedSegmentIndex = 0;
            _speedSegmentMax = speedSegments.Count;
            _stopTimer = false;
            _currentSegment = speedSegments[0];
            speed = _currentSegment.speed;
        }
        else
        {
            _stopTimer = true;
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        transform.Translate(new Vector2(_direction.x, _direction.y)*speed*Time.fixedDeltaTime);
        
        if (!_stopTimer)
        {
            if (Time.fixedTime-startTime > _currentSegment.duration)
            {
                _speedSegmentIndex++;
                if (_speedSegmentIndex >= _speedSegmentMax)
                {
                    _stopTimer = true;
                    return;
                }
                startTime = Time.fixedTime;
                _currentSegment = speedSegments[_speedSegmentIndex];
                speed = _currentSegment.speed;
            }
            
        }
    }

    public void Init(Vector2 direction, float zRotation, int shipLayer)
    {
        _direction = direction;
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x*Mathf.Sign(direction.x), scale.y, scale.z);
        Vector3 rotation = visuals.transform.rotation.eulerAngles;
        visuals.transform.rotation = Quaternion.Euler(rotation.x, rotation.y,
            (rotation.z-zRotation)*Mathf.Sign(direction.x));
        gameObject.layer = shipLayer+1;
        enemyLayer = enemyLayer & ~LayerMask.GetMask(LayerMask.LayerToName(shipLayer+1), LayerMask.LayerToName(shipLayer));
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
                if(enemy.DestroyProjectile(CollisionType.energy))
                    Destroy(gameObject);
            }
        }
        return;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
