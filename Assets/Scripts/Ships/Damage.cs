using UnityEngine;

public struct Damage
{
    public Damage(Vector2 source, float rawDamage, CollisionType type)
    {
        Source = source;
        RawDamage = rawDamage;
        Type = type;
    }

    public Vector2 Source;
    public float RawDamage;
    public CollisionType Type;
}

public enum CollisionType
{
    energy,
    kinetic,
    pointlaser,
    ship
}
