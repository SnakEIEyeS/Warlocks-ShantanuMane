public interface IDamageInfo {

    float Amount { get; }
    DamageType DamageType { get; }
    IDamageSource Source { get; }

}
