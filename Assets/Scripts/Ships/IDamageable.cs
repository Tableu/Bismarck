using Ships.Components;

public interface IDamageable
{
    public ShipInfo ShipInfo { get; }
    public float Health { get; }
    public float DodgeChance { get; }
    public void TakeDamage(Damage dmg);
}