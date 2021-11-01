using Unity.Mathematics;
using UnityEngine;

public class PointLaserOrb : Projectile
{
    [SerializeField] private GameObject pointLaserPrefab;
    [SerializeField] private int lasersPerTick;
    private new void Start()
    {
        base.Start();
        lasersPerTick = 2;
    }
    
    private new void Update()
    {
        base.Update();
        lasersPerTick = 2;
    }

    private new void OnTriggerEnter2D(Collider2D other)
    {
        if (lasersPerTick > 0)
        {
            GameObject pointLaser = Instantiate(pointLaserPrefab,transform.position,Quaternion.identity);
            Stretch(pointLaser, transform.position, other.transform.position, true);
            Destroy(other.gameObject);
            lasersPerTick--;
        }
    }
    //https://answers.unity.com/questions/844792/unity-stretch-sprite-between-two-points-at-runtime.html
    public void Stretch(GameObject _sprite,Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ) {
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        _sprite.transform.position = centerPos;
        Vector3 direction = _finalPosition - _initialPosition;
        direction = Vector3.Normalize(direction);
        _sprite.transform.right = direction;
        if (_mirrorZ) _sprite.transform.right *= -1f;
        Vector3 scale = new Vector3(1,1,1);
        scale.x = Vector3.Distance(_initialPosition, _finalPosition);
        _sprite.transform.localScale = scale;
    }
}
