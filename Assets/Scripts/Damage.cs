
using UnityEngine;

public struct Damage
{
    public Damage(Vector2 source, int rawDamage)
    {
        Source = source;
        RawDamage = rawDamage;
    }

    public Vector2 Source;
    public int RawDamage;
}
