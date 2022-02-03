public interface IDamageable
{
    public void TakeDamage(Damage dmg);
    public bool DestroyProjectile(CollisionType type);
}
