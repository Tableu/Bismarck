
using UnityEngine;

public struct Damage
{
    public Damage(Vector2 source, int rawDamage, CollisionType type)
    {
        Source = source;
        RawDamage = rawDamage;
        Type = type;
    }

    public Vector2 Source;
    public int RawDamage;
    public CollisionType Type;
}

public enum CollisionType{energy,kinetic,pointlaser,ship}
