using UI.InfoWindow;

public interface IDamageable
{
    public Subsystem Subsystem { get; }
    public void TakeDamage(Damage dmg);
    public bool DestroyProjectile(CollisionType type);
}
