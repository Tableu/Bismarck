using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrapnelProjectile : Projectile
{
    [SerializeField] private float maxDistance;
    [SerializeField] private int shrapnelCount;
    [SerializeField] private GameObject shrapnelPrefab;
    private Vector2 _startPos;
    private new void Start()
    {
        base.Start();
        _startPos = transform.position;
    }
    
    private new void Update()
    {
        base.Update();
        if (Mathf.Abs(transform.position.x - _startPos.x) > maxDistance)
        {
            SpawnShrapnel();
        }
    }

    private void SpawnShrapnel()
    {
        for (int index = 0; index < shrapnelCount; index++)
        {
            GameObject shrapnel = Instantiate(shrapnelPrefab,transform.position,Quaternion.identity);
            Projectile projectile = shrapnel.GetComponent<Projectile>();
            if (projectile != null)
            {
                Vector2 randomVector = RandomVector2(Mathf.Deg2Rad * 40, Mathf.Deg2Rad * -20);
                Vector2 shrapnelDirection = new Vector2(randomVector.x*direction.x, randomVector.y);
                projectile.Init(shrapnelDirection, 0, gameObject.layer);
            }
        }
        Destroy(gameObject);
    }
    //https://gamedev.stackexchange.com/questions/162998/how-to-get-a-random-vector-2-within-a-range
    public Vector2 RandomVector2(float angle, float angleMin){
        float random = Random.value * angle + angleMin;
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }
}
