public interface IDamageEvent : IHealthEvent {

    float Amount { get; }
    DamageType DamageType { get; }

    IDamageSource DamageSource { get; }

}
