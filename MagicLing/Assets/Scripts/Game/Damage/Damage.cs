public record Damage
{
    public float Amount { get; }
    public DamageType DamageType { get; }

    public Damage(DamageType damageType, float amount = 1f)
    {
        Amount = amount;
        DamageType = damageType;
    }
}
